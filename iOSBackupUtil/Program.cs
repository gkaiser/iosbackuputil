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
			MbdbFile mbdbFile = new MbdbFile(@"C:\Users\gkaiser\Documents\Visual Studio 2010\Projects\iOSBackupUtil\iOSBackupLib\Resources\Manifest.mbdb");
			mbdbFile.ReadFile();
			
			foreach (string mbdbDomain in mbdbFile.UniqueDomains)
				Console.WriteLine(mbdbDomain);

			Console.Write("Done, press ENTER to quit...");
			Console.ReadLine();
		}
	}
}
