using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iOSBackupLib;
using System.IO;

namespace iOSBackupUtil
{
	class Program
	{
		public static bool IsInDebugMode = System.Diagnostics.Debugger.IsAttached;

		static void Main(string[] args)
		{
			// A test "Manifest.mbdb" file is currently copied
			// into the working-directory prior to build.
			var go = false;
			var dirs = new string[0];
			int selection = -1;

			do
			{
				Console.WriteLine("******************************");
				Console.WriteLine("IOSBACKUPUTIL v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
				Console.WriteLine("******************************");
				Console.WriteLine("Please select a backup to analyze:");

				dirs = System.IO.Directory.GetDirectories(@"C:\Users\gkaiser\AppData\Roaming\Apple Computer\MobileSync\Backup\");
				for (int i = 0; i < dirs.Length; i++)
				{
					Console.Write(i.ToString(new string('0', dirs.Length.ToString().Length)));
					Console.Write(") ");
					Console.Write(new DirectoryInfo(dirs[i]).LastWriteTime.ToString("yyyy-MM-dd"));
					Console.WriteLine(dirs[i].Substring(dirs[i].LastIndexOf(Path.DirectorySeparatorChar)));
				}

				Console.Write("Backup to analyze: ");
				var readVal = Console.ReadLine();

				Console.WriteLine();
				Console.WriteLine();

				selection = -1;
				go = (int.TryParse(readVal, out selection) && selection >= 0 && selection < dirs.Length);
			} while (go == false);

      var dbgFile = @"C:\Users\gkaiser\AppData\Roaming\Apple Computer\MobileSync\Backup\3d73f1e6319fcafbb8feb21ba94355d9c4a6d99d\Manifest.mbdb";
      var mbdbFile = new MbdbFile(dirs[selection] + @"\Manifest.mbdb");
			mbdbFile.ReadFile();
			
			foreach (var mbdbDomain in mbdbFile.UniqueDomains)
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
