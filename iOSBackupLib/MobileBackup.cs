using System;

namespace iOSBackupLib
{
	public class MobileBackup
	{
		private string _mbdbFile;
		private MbdbFile _bakMbdb;

		public MobileBackup(string mbdbFile)
		{
			_mbdbFile = mbdbFile;
		}

		public void ReadBackup()
		{
			_bakMbdb = new MbdbFile(_mbdbFile);
			_bakMbdb.ReadFile();
		}

	}
}
