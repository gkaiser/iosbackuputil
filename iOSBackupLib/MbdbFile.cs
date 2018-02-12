using System;
using System.IO;
using System.Linq;
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
		public List<MbdbRecord> MbdbRecords = new List<MbdbRecord>();


		/// <summary>
		/// Initializes a new instance of the <see cref="MbdbFile"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public MbdbFile(string fileName)
		{
			_fsMbdb = File.OpenRead(fileName);

			if (!this.StreamHasValidHeader)
				throw new FormatException(
					"The MBDB file specified does not have a valid header." + Environment.NewLine +
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
        if (_validHeader != null)
          return _validHeader ?? false;

        try
        {
          if (_fsMbdb.Position != 0)
            _fsMbdb.Seek(0, SeekOrigin.Begin);

          var bSig = new byte[6];

          _fsMbdb.Read(bSig, 0, bSig.Length);

          this.HeaderId = bSig;

          _validHeader = InternalUtilities.ByteArraysAreEqual(this.HeaderId, InternalUtilities.MBDB_HEADER_BYTES);

          return _validHeader ?? false;
        }
        catch { return false; }
      }
		}

		/// <summary>
		/// Reads the file, and closes the stream related to it.
		/// </summary>
		public void ReadFile()
		{
			while (_fsMbdb.Position < _fsMbdb.Length)
			{
			  var mbdbRec = new MbdbRecord
			  {
			    Domain = InternalUtilities.ReadStringValue(_fsMbdb),
			    Path = InternalUtilities.ReadStringValue(_fsMbdb),
			    LinkTarget = InternalUtilities.ReadStringValue(_fsMbdb),
			    DataHash = InternalUtilities.ReadStringValue(_fsMbdb),
			    Unknown_I = InternalUtilities.ReadStringValue(_fsMbdb),
			    Mode = InternalUtilities.ReadUInt16Value(_fsMbdb),
			    iNodeLookup = InternalUtilities.ReadUInt64Value(_fsMbdb),
			    UserId = InternalUtilities.ReadUInt32Value(_fsMbdb),
			    GroupId = InternalUtilities.ReadUInt32Value(_fsMbdb),
			    LastModifiedTime = InternalUtilities.ReadUInt32Value(_fsMbdb),
			    LastAccessTime = InternalUtilities.ReadUInt32Value(_fsMbdb),
			    CreationTime = InternalUtilities.ReadUInt32Value(_fsMbdb),
			    FileLength = InternalUtilities.ReadUInt64Value(_fsMbdb),
			    ProtectionClass = (byte)_fsMbdb.ReadByte(),
			    PropertyCount = (byte)_fsMbdb.ReadByte(),
			    Properties = new Dictionary<string, string>(),
			  };

			  for (int i = 0; i < mbdbRec.PropertyCount; i++)
				{
					var propName = InternalUtilities.ReadStringValue(_fsMbdb);
					var propVal = InternalUtilities.ReadPropertyValue(_fsMbdb);
					mbdbRec.Properties.Add(propName, propVal);
				}

				this.MbdbRecords.Add(mbdbRec);
			}

			_fsMbdb.Close();
		  _fsMbdb.Dispose();
		}

		/// <summary>
		/// Gets the unique domains.
		/// </summary>
		/// <value>The unique domains.</value>
		public List<string> UniqueDomains
		{
			get
			{
			  return this.MbdbRecords.Select(r => r.Domain).Distinct().ToList();
			}
		}

	}
}
