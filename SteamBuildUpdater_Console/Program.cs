using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SteamBuildUpdaterConsole
{
	public class Settings
	{
		public string steamContentFolder;
		public string projectBuildFolder;
		public string steamBuildBatFileFolder;
		public string steamBuildBatFile;
	}

	class Program
	{
		private static string MyDirectory()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		private static void CopyAllFiles(string sourceDir, string targetDir)
		{
			foreach (var file in Directory.GetFiles(sourceDir))
				File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

			foreach (var directory in Directory.GetDirectories(sourceDir))
				CopyAllFiles(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
		}

		private static void DeleteAllFiles(string targetDir)
		{
			System.IO.DirectoryInfo di = new DirectoryInfo(targetDir);

			foreach (FileInfo file in di.GetFiles())
			{
				file.Delete();
			}
			foreach (DirectoryInfo dir in di.GetDirectories())
			{
				dir.Delete(true);
			}
		}


		private static void ExecuteBatFile(string targetDir, string targetFile)
		{
			string batDir = string.Format(targetDir);
			var proc = new Process();
			proc.StartInfo.WorkingDirectory = batDir;
			proc.StartInfo.FileName = targetFile;
			proc.StartInfo.CreateNoWindow = false;
			proc.Start();
			proc.WaitForExit();
		}

		static void Main(string[] args)
		{
			Console.WriteLine("Insert project name\n>");
			var projectName = Console.ReadLine();

			Console.WriteLine("Reading settings file...");

			string fileContent;
			try
			{
				var fileName = @"\" + projectName + ".json";
				var location = MyDirectory() + fileName;
				Console.WriteLine(location);

				fileContent = File.ReadAllText(location);
			}
			catch (IOException e)
			{
				Console.WriteLine("[ERROR] There is no settings file associated to that project.");
				Console.Read();

				return;
			}

			var readSettings = JsonConvert.DeserializeObject<Settings>(fileContent);

			Console.WriteLine("File read successfully");
		
			DeleteAllFiles(readSettings.steamContentFolder);
			Console.WriteLine("Old content deleted successfully");

			CopyAllFiles(readSettings.projectBuildFolder, readSettings.steamContentFolder);
			Console.WriteLine("New files copied successfully");

			ExecuteBatFile(readSettings.steamBuildBatFileFolder, readSettings.steamBuildBatFile);
			Console.WriteLine("Batch file process terminated. Press any key to exit...");

			Console.Read();
		}
	}
}
