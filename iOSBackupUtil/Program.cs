using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iOSBackupLib;

namespace iOSBackupUtil
{
	class Program
	{
		static void Main(string[] args)
		{
			MobileBackup mobileBackup = new MobileBackup(
				@"C:\Users\gkaiser\Documents\Visual Studio 2010\Projects\iOSBackupUtil\iOSBackupLib\Resources\Manifest.mbdx",
				@"C:\Users\gkaiser\Documents\Visual Studio 2010\Projects\iOSBackupUtil\iOSBackupLib\Resources\Manifest.mbdb");


			Console.ReadLine();
		}
	}
}
