using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace iOSBackupLib
{
	/// <summary>
	/// 
	/// </summary>
	public enum MbdbRecordFileMode
	{
		/// <summary>
		/// 
		/// </summary>
		DIR,
		/// <summary>
		/// 
		/// </summary>
		LINK,
		/// <summary>
		/// 
		/// </summary>
		FILE,
		/// <summary>
		/// 
		/// </summary>
		UNKNOWN
	}

	/// <summary>
	/// 
	/// </summary>
	public class MbdbFile
	{
		private FileStream _fsMbdb;

		public string HeaderId;
		public uint RecordCount;
		public MbdbRecordCollection MbdbRecords = new MbdbRecordCollection();

		/// <summary>
		/// Initializes a new instance of the <see cref="MbdbFile"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public MbdbFile(string fileName)
		{
			_fsMbdb = File.OpenRead(fileName);

			byte[] bSig = new byte[6];
			_fsMbdb.Read(bSig, 0, bSig.Length);

			this.HeaderId = BitConverter.ToString(bSig);

			if (this.HeaderId != InternalUtilities.MBDB_HEADER_ID) // 6D-62-64-62-05-00 <==> m-b-d-b-5-0
				throw new FormatException("The MBDB file specified is invalid. Expected \"" + InternalUtilities.MBDX_HEADER_ID + "\" got \"" + this.HeaderId + "\".");
		}

		/// <summary>
		/// Reads the file.
		/// </summary>
		public void ReadFile()
		{
			while (_fsMbdb.Position < _fsMbdb.Length)
			{
				MbdbRecord mbdbRec = new MbdbRecord();
				mbdbRec.Domain = InternalUtilities.GetString(_fsMbdb);
				mbdbRec.Path = InternalUtilities.GetString(_fsMbdb);
				mbdbRec.LinkTarget = InternalUtilities.GetString(_fsMbdb);
				mbdbRec.DataHash = InternalUtilities.GetString(_fsMbdb);
				mbdbRec.Unknown_I = InternalUtilities.GetString(_fsMbdb);
				mbdbRec.Mode = InternalUtilities.GetUInt16(_fsMbdb);
				mbdbRec.Unknown_II = InternalUtilities.GetUInt32(_fsMbdb);
				mbdbRec.Unknown_III = InternalUtilities.GetUInt32(_fsMbdb);
				mbdbRec.UserId = InternalUtilities.GetUInt32(_fsMbdb);
				mbdbRec.GroupId = InternalUtilities.GetUInt32(_fsMbdb);
				mbdbRec.Time_I = InternalUtilities.GetUInt32(_fsMbdb);
				mbdbRec.Time_II = InternalUtilities.GetUInt32(_fsMbdb);
				mbdbRec.Time_III = InternalUtilities.GetUInt32(_fsMbdb);
				mbdbRec.FileLength = InternalUtilities.GetUInt64(_fsMbdb);
				mbdbRec.Flag = (byte)_fsMbdb.ReadByte();
				mbdbRec.PropertyCount = (byte)_fsMbdb.ReadByte();

				if (mbdbRec.Properties == null)
					mbdbRec.Properties = new Dictionary<string, string>();

				for (int i = 0; i < mbdbRec.PropertyCount; i++)
				{
					string propName = InternalUtilities.GetString(_fsMbdb);
					string propVal = InternalUtilities.GetPropertyValue(_fsMbdb);
					mbdbRec.Properties.Add(propName, propVal);
				}

				this.MbdbRecords.Add(mbdbRec);

				Console.WriteLine(_fsMbdb.Position.ToString("000000000") + " / " + _fsMbdb.Length.ToString("000000000"));
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
