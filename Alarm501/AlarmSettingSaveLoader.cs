using System;
using System.IO;

namespace Alarm501
{
	/// <summary>
	/// This class saves and loads the alarm settings from a text file
	/// </summary>
	public class AlarmSettingSaveLoader
	{
		/// <summary>
		/// The file path used for loading and saving alarm settings
		/// </summary>
		private readonly string _filePath;

		public AlarmSettingSaveLoader(string filePath)
		{
			_filePath = filePath;
		}

		/// <summary>
		/// Saves the alarm settings to the specified file path
		/// </summary>
		/// <param name="alarm">The alarm whose settings are to be saved</param>
		public void SaveAlarmSettings(Alarm alarm)
		{
			try
			{
				using StreamWriter writer = new StreamWriter(_filePath);
				writer.WriteLine($"MaximumItem={alarm.MaximumItem}");
				writer.WriteLine($"SnoozeTime={alarm.SnoozeTime}");
			}
			catch (Exception) { }
		}

		/// <summary>
		/// Loads the alarm settings from the specified file path
		/// </summary>
		/// <returns>A dictionary containing the loaded alarm settings</returns>
		public Dictionary<string, string> LoadAlarmSettings()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();

			try
			{
				using StreamReader reader = new StreamReader(_filePath);
				string line;
				while ((line = reader.ReadLine()!) != null)
				{
					string[] parts = line.Split('=');
					if (parts.Length == 2)
					{
						settings.Add(parts[0], parts[1]);
					}
				}
			}
			catch (Exception) { }

			return settings;
		}
	}
}
