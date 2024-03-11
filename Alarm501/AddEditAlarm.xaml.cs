using System.Windows;
using static Alarm501.AlarmBinder;

namespace Alarm501
{
	/// <summary>
	/// Interaction logic for AddEditAlarm.xaml
	/// </summary>
	public partial class AddEditAlarm : Window
	{
		public event EditSetBtnClickedEventHandler? OnEditSetBtnClicked;
		public event AddSetBtnClickedEventHandler? OnAddSetBtnClicked;

		public AddEditAlarm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Sets the state of the On/Off slider based on the IsActivated property of the selected alarm item
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void SetOnOffSlider(object sender, RoutedEventArgs e)
		{
			if (DataContext is AlarmItem ai)
			{
				if (ai != null)
				{
					if (ai.IsActivated)
					{
						this.UxOnOffSlider.uxCheckBox.IsChecked = true;
					}
				}
			}
		}

		/// <summary>
		/// Sets the Sound of the EnumBox based on the Sound property of the selected alarm item
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void SetEnumBox(object sender, RoutedEventArgs e)
		{
			if (DataContext is AlarmItem ai)
			{
				this.UxSoundEnumBox.uxRadioButtonBox.SelectedItem = ai.Sound;
			}
			else if (DataContext is Alarm alarm)
			{
				this.UxSoundEnumBox.uxRadioButtonBox.SelectedItem = AlarmSound.Radar;
			}
		}

		/// <summary>
		/// Parse times and hourse and seconds
		/// </summary>
		/// <returns>A parsed AlarmItem</returns>
		private AlarmItem ParseTime()
		{
			DateTime time = (DateTime)UxPickDateTime.uxTimePicker.Value!;
			AlarmSound sound = (AlarmSound)UxSoundEnumBox.uxRadioButtonBox.SelectedItem;
			bool activated = (bool)UxOnOffSlider.uxCheckBox.IsChecked!;
			string timestring = time.ToString();
			bool isAM = timestring.Contains("AM");
			int hours = time.Hour;
			int minutes = time.Minute;
			int seconds = time.Second;

			DateTime currentDate = DateTime.Today;
			DateTime newtime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, hours, minutes, seconds);
			AlarmItem newAlarmItem = new()
			{
				Time = newtime,
				Sound = sound,
				IsAm = isAM,
				IsActivated = activated
			};

			return newAlarmItem;
		}

		/// <summary>
		/// On event user clicked the Set button
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void OnSetAlarm(object sender, RoutedEventArgs e)
		{
			AlarmItem returnAlarmItem = ParseTime();
			if (DataContext is Alarm alarm)
			{
				OnAddSetBtnClicked?.Invoke(sender, e, returnAlarmItem);
			}
			else if (DataContext is AlarmItem ai)
			{
				OnEditSetBtnClicked?.Invoke(sender, e, returnAlarmItem);
			}
			this.Close();
		}

		/// <summary>
		/// On event user clicked the cancel button
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void OnCancel(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
