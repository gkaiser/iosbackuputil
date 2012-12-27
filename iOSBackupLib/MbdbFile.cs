using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace iOSBackupLib
{
	/// <summary>
	/// 
	/// </summary>
	public class MbdbFile 
	{
		private FileStream _fsMbdb = null;
		private bool? _validHeader = null;

		public byte[] HeaderId;
		public uint RecordCount;
		public MbdbRecordCollection MbdbRecords = new MbdbRecordCollection();


		/// <summary>
		/// Initializes a new instance of the <see cref="MbdbFile"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public MbdbFile(string fileName)
		{
			_fsMbdb = File.OpenRead(fileName);

			if (!this.StreamHasValidHeader)
				throw new FormatException(
					"The MBDB file specified is invalid. " + Environment.NewLine +
					//"Expected \"" + InternalUtilities.MBDB_HEADER_ID + "\"" + Environment.NewLine + 
					"Recieved: \"" + this.HeaderId + "\"");
		}

		/// <summary>
		/// Returns a value indicating whether a file has a valid MBDB header.
		/// </summary>
		/// <param name="sMbdb"></param>
		/// <returns></returns>
		private bool StreamHasValidHeader
		{
			get
			{
				if (_validHeader == null)
				{
					try
					{
						if (_fsMbdb.Position != 0)
							_fsMbdb.Seek(0, SeekOrigin.Begin);

						byte[] bSig = new byte[6];

						_fsMbdb.Read(bSig, 0, bSig.Length);

						this.HeaderId = bSig;

						_validHeader = InternalUtilities.ByteArraysAreEqual(this.HeaderId, InternalUtilities.MBDB_HEADER_BYTES);
					}
					catch { return false; }
				}

				return (_validHeader ?? false);
			}
		}

		/// <summary>
		/// Reads the file, and closes the stream related to it.
		/// </summary>
		public void ReadFile()
		{
			while (_fsMbdb.Position < _fsMbdb.Length)
			{
				MbdbRecord mbdbRec = new MbdbRecord();
				mbdbRec.Domain = InternalUtilities.ReadStringValue(_fsMbdb);
				mbdbRec.Path = InternalUtilities.ReadStringValue(_fsMbdb);
				mbdbRec.LinkTarget = InternalUtilities.ReadStringValue(_fsMbdb);
				mbdbRec.DataHash = InternalUtilities.ReadStringValue(_fsMbdb);
				mbdbRec.Unknown_I = InternalUtilities.ReadStringValue(_fsMbdb);
				mbdbRec.Mode = InternalUtilities.ReadUInt16Value(_fsMbdb);
				mbdbRec.Unknown_II = InternalUtilities.ReadUInt32Value(_fsMbdb);
				mbdbRec.Unknown_III = InternalUtilities.ReadUInt32Value(_fsMbdb);
				mbdbRec.UserId = InternalUtilities.ReadUInt32Value(_fsMbdb);
				mbdbRec.GroupId = InternalUtilities.ReadUInt32Value(_fsMbdb);
				mbdbRec.Time_I = InternalUtilities.ReadUInt32Value(_fsMbdb);
				mbdbRec.Time_II = InternalUtilities.ReadUInt32Value(_fsMbdb);
				mbdbRec.Time_III = InternalUtilities.ReadUInt32Value(_fsMbdb);
				mbdbRec.FileLength = InternalUtilities.ReadUInt64Value(_fsMbdb);
				mbdbRec.Flag = (byte)_fsMbdb.ReadByte();
				mbdbRec.PropertyCount = (byte)_fsMbdb.ReadByte();

				if (mbdbRec.Properties == null)
					mbdbRec.Properties = new Dictionary<string, string>();

				for (int i = 0; i < mbdbRec.PropertyCount; i++)
				{
					string propName = InternalUtilities.ReadStringValue(_fsMbdb);
					string propVal = InternalUtilities.ReadPropertyValue(_fsMbdb);
					mbdbRec.Properties.Add(propName, propVal);
				}

				this.MbdbRecords.Add(mbdbRec);
			}

			_fsMbdb.Close();
		}

		/// <summary>
		/// Gets the unique domains.
		/// </summary>
		/// <value>The unique domains.</value>
		public List<string> UniqueDomains
		{
			get
			{
				List<string> lstDomains = new List<string>();

				foreach (MbdbRecord mbdbRec in MbdbRecords)
				{
					if (lstDomains.Contains(mbdbRec.Domain))
						continue;

					lstDomains.Add(mbdbRec.Domain);
				}

				lstDomains.Sort();

				return lstDomains;
			}
		}

	}

}
