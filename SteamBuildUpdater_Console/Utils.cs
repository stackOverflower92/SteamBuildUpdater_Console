using System;

namespace SteamBuildUpdaterConsole
{
	public static class Utils
	{
		public enum ELogType
		{
			Info,
			Warning,
			Error
		}

		public static void Log(string sLog, ELogType eLogType = ELogType.Info, bool bNewLine = true)
		{
			var sPrefix = string.Empty;
			switch (eLogType)
			{
				case ELogType.Info:
					{
						sPrefix = "[INFO] ";
						break;
					}

				case ELogType.Warning:
					{
						sPrefix = "[WARN] ";
						break;
					}

				case ELogType.Error:
					{
						sPrefix = "[ERROR] ";
						break;
					}

				default: break;
			}

			var sLogText = sPrefix + sLog;

			if (bNewLine)
			{
				Console.WriteLine(sLogText);
			}
			else
			{
				Console.Write(sLogText);
			}
		}
	}
}
