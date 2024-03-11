using System.ComponentModel;

namespace Alarm501
{
	/// <summary>
	/// The Interface represent the properties that AlarmItems share
	/// </summary>
	public interface IAlarmItem
	{
		/// <summary>
		/// Event triggered when a property changes
		/// </summary>
		event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// The time of the alarm item to be go off
		/// </summary>
		DateTime Time { get; }

		/// <summary>
		/// How much time left for the AlarmItem
		/// </summary>
		TimeSpan TimeLeft { get; }

		/// <summary>
		/// Is the AlarmItem set to AM
		/// </summary>
		bool IsAm { get; }

		/// <summary>
		/// Is the AlarmItem activated
		/// </summary>
		bool IsActivated { get; }

		/// <summary>
		/// Whether the AlarmItem went off
		/// </summary>
		bool WentOff { get; }

		/// <summary>
		/// Sound of AlarmItem when went off
		/// </summary>
		AlarmSound Sound { get; }
	}
}
