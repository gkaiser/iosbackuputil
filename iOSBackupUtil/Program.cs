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
			MbdbFile mbdbFile = new MbdbFile(@"Manifest.mbdb");
			mbdbFile.ReadFile();
			
			foreach (string mbdbDomain in mbdbFile.UniqueDomains)
				Console.WriteLine(mbdbDomain);

			Console.Write("Done, press ENTER to quit...");
			Console.ReadLine();
		}
	}
}
