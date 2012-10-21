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
		private MemoryStream _msMbdb;
		private bool? _validHeader = null;

		public string HeaderId;
		public uint RecordCount;
		public MbdbRecordCollection MbdbRecords = new MbdbRecordCollection();

		/// <summary>
		/// Initializes a new instance of the <see cref="MbdbFile"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public MbdbFile(string fileName)
		{
			FileStream fsMbdb = File.OpenRead(fileName);

			if (!this.HasValidHeader(fsMbdb))
				throw new FormatException(
					"The MBDB file specified is invalid. " + Environment.NewLine +
					"Expected \"" + InternalUtilities.MBDX_HEADER_ID + "\"" + Environment.NewLine + 
					"Recieved: \"" + this.HeaderId + "\"");

			_msMbdb = new MemoryStream();
			byte[] bBuffer = new byte[4096];
			int bytesRead = 0;

			while (true)
			{
				bytesRead = fsMbdb.Read(bBuffer, 0, bBuffer.Length);

				if (bytesRead == 0)
					break;

				_msMbdb.Write(bBuffer, 0, bytesRead);
			}

			fsMbdb.Close();
			_msMbdb.Seek(0, SeekOrigin.Begin);
		}

		/// <summary>
		/// Returns a value indicating whether a file has a valid MBDB header.
		/// </summary>
		/// <param name="fsMbdb"></param>
		/// <returns></returns>
		public bool HasValidHeader(FileStream fsMbdb)
		{
			if (_validHeader == null)
			{
				byte[] bSig = new byte[6];
				fsMbdb.Read(bSig, 0, bSig.Length);

				this.HeaderId = BitConverter.ToString(bSig);

				_validHeader = this.HeaderId != InternalUtilities.MBDB_HEADER_ID; // 6D-62-64-62-05-00 <==> m-b-d-b-5-0
			}

			return (_validHeader ?? false);
		}

		/// <summary>
		/// Reads the file.
		/// </summary>
		public void ReadFile()
		{
			while (_msMbdb.Position < _msMbdb.Length)
			{
				MbdbRecord mbdbRec = new MbdbRecord();
				mbdbRec.Domain = InternalUtilities.GetString(_msMbdb);
				mbdbRec.Path = InternalUtilities.GetString(_msMbdb);
				mbdbRec.LinkTarget = InternalUtilities.GetString(_msMbdb);
				mbdbRec.DataHash = InternalUtilities.GetString(_msMbdb);
				mbdbRec.Unknown_I = InternalUtilities.GetString(_msMbdb);
				mbdbRec.Mode = InternalUtilities.GetUInt16(_msMbdb);
				mbdbRec.Unknown_II = InternalUtilities.GetUInt32(_msMbdb);
				mbdbRec.Unknown_III = InternalUtilities.GetUInt32(_msMbdb);
				mbdbRec.UserId = InternalUtilities.GetUInt32(_msMbdb);
				mbdbRec.GroupId = InternalUtilities.GetUInt32(_msMbdb);
				mbdbRec.Time_I = InternalUtilities.GetUInt32(_msMbdb);
				mbdbRec.Time_II = InternalUtilities.GetUInt32(_msMbdb);
				mbdbRec.Time_III = InternalUtilities.GetUInt32(_msMbdb);
				mbdbRec.FileLength = InternalUtilities.GetUInt64(_msMbdb);
				mbdbRec.Flag = (byte)_msMbdb.ReadByte();
				mbdbRec.PropertyCount = (byte)_msMbdb.ReadByte();

				if (mbdbRec.Properties == null)
					mbdbRec.Properties = new Dictionary<string, string>();

				for (int i = 0; i < mbdbRec.PropertyCount; i++)
				{
					string propName = InternalUtilities.GetString(_msMbdb);
					string propVal = InternalUtilities.GetPropertyValue(_msMbdb);
					mbdbRec.Properties.Add(propName, propVal);
				}

				this.MbdbRecords.Add(mbdbRec);

				Console.WriteLine(_msMbdb.Position.ToString("000000000") + " / " + _msMbdb.Length.ToString("000000000"));
			}
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
