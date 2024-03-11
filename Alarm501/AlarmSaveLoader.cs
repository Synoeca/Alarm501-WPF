using System;
using System.Collections.Generic;
using System.IO;

namespace Alarm501
{
	/// <summary>
	/// This class saves and loads the Alarm from the txt file
	/// </summary>
	public class AlarmSaveLoader
	{
		/// <summary>
		/// Loads a list of IAlarm objects from the specified file path
		/// </summary>
		/// <param name="filePath">The path to the text file containing alarm data</param>
		/// <returns>A list of IAlarm objects loaded from the file</returns>
		public static List<IAlarmItem> LoadAlarms(string filePath)
		{
			List<IAlarmItem> alarms = new List<IAlarmItem>();

			try
			{
				using StreamReader reader = new StreamReader(filePath);
				string line;
				while ((line = reader.ReadLine()!) != null)
				{
					string[] parts = line.Split(',');

					if (parts.Length == 5)
					{
						DateTime time = DateTime.Parse(parts[0]);
						bool isAM = bool.Parse(parts[1]);
						bool isActivated = bool.Parse(parts[2]);
						bool wentOff = bool.Parse(parts[3]);
						Enum.TryParse(parts[4], out AlarmSound sound);

						AlarmItem loadedAlarm = new AlarmItem
						{
							Time = time,
							IsAm = isAM,
							IsActivated = isActivated,
							WentOff = wentOff,
							Sound = sound
						};
						alarms.Add(loadedAlarm);
					}
				}
			}
			catch (Exception) { }
			return alarms;
		}

		/// <summary>
		/// Saves a list of IAlarm objects to the specified file path
		/// </summary>
		/// <param name="alarms">The list of IAlarm objects to be saved</param>
		/// <param name="filePath">The path to the text file where alarms will be saved</param>
		public static void SaveAlarms(List<IAlarmItem> alarms, string filePath)
		{
			try
			{
				using StreamWriter writer = new StreamWriter(filePath);
				foreach (var alarm in alarms)
				{
					string line = $"{alarm.Time},{alarm.IsAm},{alarm.IsActivated},{alarm.WentOff},{alarm.Sound}";
					writer.WriteLine(line);
				}
			}
			catch (Exception) { }
		}
	}
}
