using System.ComponentModel;
using System.Windows.Threading;

namespace Alarm501
{
	/// <summary>
	/// A class for an alarm item
	/// </summary>
	public class AlarmItem : IAlarmItem
	{
		/// <summary>
		/// Event triggered when a property changes
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// The private backing field for the Time property
		/// </summary>
		private DateTime _time = new(1, 1, 1, 06, 00, 0);

		/// <summary>
		/// The time when the alarm go off
		/// </summary>
		public DateTime Time
		{
			get => _time;
			set
			{
				_time = value;
				if (value is DateTime time)
				{
					TimeLeft = time - DateTime.Now;
				}
				OnPropertyChanged(nameof(this.Time));
				OnPropertyChanged(nameof(this.TimeLeft));
				OnPropertyChanged(nameof(this.DisplayTime));
			}
		}

		/// <summary>
		/// The private backing field for the TimeLeft property
		/// </summary>
		private TimeSpan _timeLeft = TimeSpan.Zero;

		/// <summary>
		/// How much time left
		/// </summary>
		public TimeSpan TimeLeft
		{
			get => _timeLeft;
			set
			{
				_timeLeft = value;
				OnPropertyChanged(nameof(this.TimeLeft));
			}
		}

		/// <summary>
		/// The private backing field for the IsAm property
		/// </summary>
		private bool _isAm = true;

		/// <summary>
		/// If this alarm set to the AM
		/// </summary>
		public bool IsAm
		{
			get => _isAm;
			set
			{
				_isAm = value;
				OnPropertyChanged(nameof(this.IsAm));
				OnPropertyChanged(nameof(this.DisplayIsAm));
			}
		}

		/// <summary>
		/// The private backing field for the IsAm property
		/// </summary>
		private bool _isActivated = false;

		/// <summary>
		/// If is this alarm activated 
		/// </summary>
		public bool IsActivated
		{
			get => _isActivated;
			set
			{
				_isActivated = value;
				OnPropertyChanged(nameof(this.IsActivated));
				OnPropertyChanged(nameof(this.DisplayIsActivated));
			}
		}

		/// <summary>
		/// The private backing field for the WentOff property
		/// </summary>
		private bool _isWentOff = false;

		/// <summary>
		/// If this alarm went off
		/// </summary>
		public bool WentOff
		{
			get => _isWentOff;
			set
			{
				_isWentOff = value;
				OnPropertyChanged(nameof(this.WentOff));
			}
		}

		/// <summary>
		/// The Sound of Alarm
		/// </summary>
		public AlarmSound Sound { get; set; }

		/// <summary>
		/// How should this AlarmItem be displayed in the ListBox
		/// </summary>
		/// <returns>Displayed string</returns>
		public override string ToString()
		{
			string fetch = "";
			fetch += Time.ToString("hh:mm:ss");
			if (!IsAm) { fetch += " PM"; } else { fetch += " AM"; }
			if (!IsActivated) { fetch += " Off"; } else { fetch += " On"; }
			return fetch;
		}

		/// <summary>
		/// Display only the hours, minutes, and seconds
		/// </summary>
		public string DisplayTime => $"{(Time.Hour % 12 == 0 ? 12 : Time.Hour % 12):00}:{Time.Minute:00}:{Time.Second:00}";

		/// <summary>
		/// Display whether the time is AM or PM
		/// </summary>
		public string DisplayIsAm => IsAm ? "AM" : "PM";

		/// <summary>
		/// Display whether the alarm is activated or deactivated
		/// </summary>
		public string DisplayIsActivated => IsActivated ? "On" : "Off";

		/// <summary>
		/// Display the time left until the alarm goes off
		/// </summary>
		public string DisplayTimeLeft
		{
			get
			{
				TimeSpan timeLeft = Time - DateTime.Now;
				if (timeLeft < TimeSpan.Zero)
				{
					return "00:00:00";
				}
				else
				{
					return $"{timeLeft.Hours:00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
				}
			}
		}

		/// <summary>
		/// A helper method of invoking PropertyChangedEvent
		/// </summary>
		/// <param name="propertyName">A name of property that changed its contents</param>
		public void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
