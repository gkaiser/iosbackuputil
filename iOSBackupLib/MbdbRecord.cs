using System;
using System.Text;
using System.Collections.Generic;

namespace iOSBackupLib
{
	/// <summary>
	/// 
	/// </summary>
	public class MbdbRecord
	{
		private string _filenameAsHash = null;

		public string Domain = null;
		public string Path = null;
		public string LinkTarget = null;
		public string DataHash = null;
		public string Unknown_I = null; // EncryptionKey
		public ushort Mode = 0; 
		public ulong iNodeLookup = 0; // iNode Lookup Number
		public uint UserId = 0;
		public uint GroupId = 0;
		public uint LastModifiedTime = 0;
		public uint LastAccessTime = 0;
		public uint CreationTime = 0;
		public ulong FileLength = 0;
		public byte ProtectionClass = 0;
		public byte PropertyCount = 0;
		public Dictionary<string, string> Properties = new Dictionary<string, string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="MbdbRecord"/> class.
		/// </summary>
		public MbdbRecord() { }

		/// <summary>
		/// Gets the filename as hash.
		/// </summary>
		/// <value>The filename as hash.</value>
		public string FilenameAsHash
		{
			get
			{
				if (this.RecordMode != MbdbRecordFileMode.FILE)
					return "";
				if (_filenameAsHash != null)
					return _filenameAsHash;

				string fullFile = Domain + "-" + Path;
				byte[] bfullFile = Encoding.UTF8.GetBytes(fullFile);
				byte[] bHash = new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(bfullFile);

				_filenameAsHash = BitConverter.ToString(bHash).Replace("-", "").ToLower();

				return _filenameAsHash;
			}
		}

		/// <summary>
		/// Gets the record mode.
		/// </summary>
		/// <value>The record mode.</value>
		public MbdbRecordFileMode RecordMode
		{
			get
			{
				switch ((this.Mode & 0xF000) >> 12)
				{
					case 0xA:
						return MbdbRecordFileMode.LINK;
					case 0x4:
						return MbdbRecordFileMode.DIR;
					case 0x8:
						return MbdbRecordFileMode.FILE;
					default:
						return MbdbRecordFileMode.UNKNOWN;
				}
			}
		}

		/// <summary>
		/// Filenames as hash exists.
		/// </summary>
		/// <returns></returns>
		public bool FilenameAsHashExists()
		{
			if (this.RecordMode != MbdbRecordFileMode.FILE)
				return false;

			List<string> lstTemp = new List<string>(System.IO.File.ReadAllLines(@"FileListing.txt"));

			return lstTemp.Contains(this.FilenameAsHash);
		}

		/// <summary>
		/// Prints this instance.
		/// </summary>
		private void Print()
		{
			Console.WriteLine();
			Console.WriteLine("MBDB");
			Console.WriteLine("  Domain          : " + this.Domain);
			Console.WriteLine("  Path            : " + this.Path);
			Console.WriteLine("  Link Target     : " + this.LinkTarget == "");
			Console.WriteLine("  Data Hash       : " + this.DataHash);
			Console.WriteLine("  Unknown I       : " + this.Unknown_I);
			Console.WriteLine("  File Mode       : " + this.RecordMode);
			Console.WriteLine("  Unknown II      : " + this.Unknown_II);
			Console.WriteLine("  User ID         : " + this.UserId.ToString());
			Console.WriteLine("  Group ID        : " + this.GroupId.ToString());
			Console.WriteLine("  Time I          : " + InternalUtilities.EpochTimeToString((int)this.LastModifiedTime));
			Console.WriteLine("  Time II         : " + InternalUtilities.EpochTimeToString((int)this.LastAccessTime));
			Console.WriteLine("  Time III        : " + InternalUtilities.EpochTimeToString((int)this.CreationTime));
			Console.WriteLine("  File Length     : " + this.FileLength.ToString());
			Console.WriteLine("  Flag            : " + this.ProtectionClass.ToString());
			Console.WriteLine("  Property Ct     : " + this.PropertyCount.ToString());
			Console.WriteLine("  Filename (Hash) : " + this.FilenameAsHash + " (" + this.FilenameAsHashExists() + ")");

			foreach (string propKey in this.Properties.Keys)
				Console.WriteLine("    " + propKey + " ==> " + this.Properties[propKey]);

			Console.WriteLine();
		}

	}
}
