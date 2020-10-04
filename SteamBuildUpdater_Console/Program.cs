using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SteamBuildUpdaterConsole
{
	public class Settings
	{
		public string m_sSteamContentFolder;
		public string m_sProjectBuildFolder;
		public string m_sSteamBuildBatFileFolder;
		public string m_sSteamBuildBatFile;
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
			{
				File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
			}

			foreach (var directory in Directory.GetDirectories(sourceDir))
			{
				CopyAllFiles(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
			}
		}

		private static void DeleteAllFiles(string targetDir)
		{
			DirectoryInfo di = new DirectoryInfo(targetDir);

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
			Utils.Log("Insert project name\n>");
			var projectName = Console.ReadLine();

			Utils.Log("Reading settings file...");

			string fileContent;
			try
			{
				var fileName = @"\" + projectName + ".json";
				var location = MyDirectory() + fileName;
				Utils.Log(location);

				fileContent = File.ReadAllText(location);
			}
			catch (IOException e)
			{
				Utils.Log("There is no settings file associated to that project.", Utils.ELogType.Error);
				Console.Read();

				return;
			}

			var readSettings = JsonConvert.DeserializeObject<Settings>(fileContent);

			Utils.Log("File read successfully");
		
			DeleteAllFiles(readSettings.m_sSteamContentFolder);
			Utils.Log("Old content deleted successfully");

			CopyAllFiles(readSettings.m_sProjectBuildFolder, readSettings.m_sSteamContentFolder);
			Utils.Log("New files copied successfully");

			ExecuteBatFile(readSettings.m_sSteamBuildBatFileFolder, readSettings.m_sSteamBuildBatFile);
			Utils.Log("Batch file process terminated. Press any key to exit...");

			Console.Read();
		}
	}
}
