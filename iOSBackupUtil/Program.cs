using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iOSBackupLib;

namespace iOSBackupUtil
{
	class Program
	{
		public static bool IsInDebugMode = System.Diagnostics.Debugger.IsAttached;

		static void Main(string[] args)
		{
			// A test "Manifest.mbdb" file is currently copied
			// into the working-directory prior to build.
			MbdbFile mbdbFile = new MbdbFile(@"C:\Users\gkaiser\AppData\Roaming\Apple Computer\MobileSync\Backup\90a6456ba232442db2715bcead0110521f585141\Manifest.mbdb");
			mbdbFile.ReadFile();
			
			foreach (string mbdbDomain in mbdbFile.UniqueDomains)
				Console.WriteLine(mbdbDomain);

			Console.WriteLine();
			Console.Write("Done, press ENTER to quit...");
			Console.ReadLine();
		}

		internal static string ByteArrayAsStringOfHexDigits(byte[] bytes)
		{
			return BitConverter.ToString(bytes);
		}
	}
}
