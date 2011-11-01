using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iOSBackupLib
{
	public class MobileBackup
	{
		private string _mbdxFile;
		private string _mbdbFile;

		public MobileBackup(string mbdxFile, string mbdbFile)
		{
			_mbdxFile = mbdxFile;
			_mbdbFile = mbdbFile;
		}

		public static void ReadBackup()
		{

		}

	}
}
