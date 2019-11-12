using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace SteamBuildUpdaterConsole
{
	public class Settings
	{
		public string steamContentFolder;
		public string projectBuildFolder;
		public string steamBuildBatFile;
	}

	class Program
	{
		private static string MyDirectory()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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

			Console.Read();
		}
	}
}
