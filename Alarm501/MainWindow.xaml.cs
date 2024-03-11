using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Alarm501
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public delegate void AlarmItemSelectedEventHandler(object sender, RoutedEventArgs e);
		public delegate void AlarmItemRemovedEventHandler(object sender, RoutedEventArgs e, int index);
		public delegate void EditBtnClickedEventHandler(object sender, RoutedEventArgs e);
		public delegate void AddBtnClickedEventHandler(object sender, RoutedEventArgs e);
		public delegate void RemoveBtnClickedEventHandler(object sender, RoutedEventArgs e);
		public delegate void AddEditAlarmFinishedEventHandler(object sender, RoutedEventArgs e, AlarmItem ai);
		public delegate void ClockEventHandler(object sender, EventArgs e, object alarm);
		public delegate void WentOffEventHandler(object sender, EventArgs e, object alarm);
		public delegate void SnoozeEventHandler(object sender, EventArgs e);
		public delegate void StopEventHandler(object sender, RoutedEventArgs e);
		public delegate void DropDownMenuClickEventHandler(object sender, RoutedEventArgs e, Alarm alarm);

		/// <summary>
		/// A path for alarms.txt
		/// </summary>
		private static readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alarms.txt");

		/// <summary>
		/// The instance of AlarmSettingSaveLoader used for loading and saving alarm settings
		/// </summary>
		private readonly AlarmSettingSaveLoader _alarmSettingSaveLoader;

		public MainWindow()
		{
			InitializeComponent();
			Alarm alarm = new Alarm();
			DataContext = alarm;
			LoadAlarmsFromFile();
			AlarmBinder alarmBinder = new AlarmBinder(alarm);
			if (alarm.MaximumItem <= alarm.Count)
			{
				UxAddControl.IsEnabled = false;
			}
			SubscribeEvent(alarmBinder);
			WireUpClosingEvent();
			_alarmSettingSaveLoader = new AlarmSettingSaveLoader("settings.txt");
			LoadAlarmSettings();
		}

		/// <summary>
		/// Subscribe and pass deligates
		/// </summary>
		/// <param name="alarmBinder">A Binder</param>
		private void SubscribeEvent(AlarmBinder alarmBinder)
		{
			UxAlarmListControl.Select += alarmBinder.OnAlarmItemSelectedHandler;
			UxAlarmListControl.Remove += alarmBinder.OnRemoveBtnClickedHandler;
			UxEditControl.Edit += alarmBinder.OnEditBtnClickedHandler;
			UxAddControl.Add += alarmBinder.OnAddBtnClickedHandler;
			UxTimeLeftControl.Clock += alarmBinder.OnClockTickedHandler;
			UxTimeLeftControl.WentOff += alarmBinder.OnWentOffHandler;
			UxSnoozeControl.Snooze += alarmBinder.OnSnoozeBtnClickedHandler;
			UxStopControl.Stop += alarmBinder.OnStopBtnClickedHandler;
			UxDropDownMenuControl.ClickMenu += alarmBinder.OnDropDownMenuBtnClickedHandler;
			UxStopControl.Stop += this.ListenStopBtnClicked;
			UxDropDownMenuControl.ClickMenu += this.ListenDropDownMenuClicked;
			alarmBinder.OnAlarmItemSelected += this.ListenAlarmItemSelected;
			alarmBinder.OnAlarmItemRemoved += this.ListenAlarmItemRemoved;
			alarmBinder.OnAddEditAlarmItemFinished += this.ListenAddEditAlarmItemFinished;
			UxTimeLeftControl.WentOff += this.ListenWentOff;
			UxSnoozeControl.Snooze += this.ListenSnoozeBtnClicked;
		}

		/// <summary>
		/// Load the alarms from the txt file
		/// </summary>
		private void LoadAlarmsFromFile()
		{
			List<IAlarmItem> loadedAlarms = AlarmSaveLoader.LoadAlarms(_filePath);

			if (DataContext is Alarm alarm)
			{
				foreach (AlarmItem ai in loadedAlarms)
				{
					alarm.Add(ai);
				}
			}
		}

		/// <summary>
		/// Loads the alarm settings from the file specified by _filePath
		/// and updates the current DataContext's Alarm instance accordingly
		/// </summary>
		private void LoadAlarmSettings()
		{
			if (DataContext is Alarm alarm)
			{
				Dictionary<string, string> settings = _alarmSettingSaveLoader.LoadAlarmSettings();
				if (settings.ContainsKey("MaximumItem") && settings.ContainsKey("SnoozeTime"))
				{
					if (int.TryParse(settings["MaximumItem"], out int maximumItem))
					{
						alarm.MaximumItem = maximumItem;
					}
					if (int.TryParse(settings["SnoozeTime"], out int snoozeTime))
					{
						alarm.SnoozeTime = snoozeTime;
					}
				}
			}
		}

		/// <summary>
		/// Wire up closing event
		/// </summary>
		private void WireUpClosingEvent()
		{
			Closing += MainWindowClosing!;
		}

		/// <summary>
		/// Turn off alarms so that no alarms left running in the background
		/// and save alarms to txt file
		/// </summary>
		/// <param name="sender">MainWindow</param>
		/// <param name="e">Cancel event argument</param>
		private void MainWindowClosing(object sender, CancelEventArgs e)
		{
			if (DataContext is Alarm alarm)
			{
				foreach (AlarmItem ai in alarm)
				{
					if (ai.IsActivated)
					{
						ai.IsActivated = false;
					}
				}
				SaveAlarmsToFile();
				SaveAlarmSettings();
			}
		}

		/// <summary>
		/// Save the alarms to the txt file
		/// </summary>
		private void SaveAlarmsToFile()
		{
			if (DataContext is Alarm alarm)
			{
				AlarmSaveLoader.SaveAlarms([.. alarm], _filePath);
			}
		}

		/// <summary>
		/// Saves the alarm settings using the current DataContext's Alarm instance
		/// </summary>
		private void SaveAlarmSettings()
		{
			if (DataContext is Alarm alarm)
			{
				_alarmSettingSaveLoader.SaveAlarmSettings(alarm);
			}
		}

		/// <summary>
		/// Enables the Edit button when an alarm item is selected
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">Event arguments</param>
		private void ListenAlarmItemSelected(object sender, RoutedEventArgs e)
		{
			if (!UxEditControl.IsEnabled) { UxEditControl.IsEnabled = true; }
			if (e.Source is ListView lv)
			{
				if (lv.SelectedItem is AlarmItem ai)
				{
					if (ai.IsActivated)
					{
						UxSoundDisplayControl.Visibility = Visibility.Hidden;
						UxTimeLeftControl.Visibility = Visibility.Visible;
					}
					else { UxTimeLeftControl.Visibility = Visibility.Hidden; }

					if (ai.WentOff)
					{
						UxSoundDisplayControl.DataContext = ai;
						UxSoundDisplayControl.Visibility = Visibility.Visible;
						UxSnoozeControl.IsEnabled = true;
						UxStopControl.IsEnabled = true;
					}
					else
					{
						UxSoundDisplayControl.Visibility = Visibility.Hidden;
						UxSnoozeControl.IsEnabled = false;
						UxStopControl.IsEnabled = false;
					}
					UxTimeLeftControl.DataContext = ai;
				}
			}
		}

		/// <summary>
		/// Enables the Edit button when an alarm item is selected
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">Event arguments</param>
		/// <param name="index">An index of the removed item</param>
		private void ListenAlarmItemRemoved(object sender, RoutedEventArgs e, int index)
		{
			if (DataContext is Alarm alarm)
			{
				if (sender is ListView lv)
				{
					if (lv.SelectedIndex == -1)
					{
						if (alarm.Count > 0)
						{
							if (alarm.Count <= index)
							{
								lv.SelectedItem = alarm.ElementAt(index - 1);
							}
							else
							{
								lv.SelectedItem = alarm.ElementAt(index);
							}
						}
					}
				}
				if (alarm.Count < alarm.MaximumItem)
				{
					UxAddControl.IsEnabled = true;
				}
				if (alarm.Count == 0)
				{
					alarm.SelectedItem = null!;
					UxEditControl.IsEnabled = false;
					UxSnoozeControl.IsEnabled = false;
					UxStopControl.IsEnabled = false;
					UxTimeLeftControl.Visibility = Visibility.Collapsed;
				}
			}

		}

		/// <summary>
		/// Handles the event when adding or editing an alarm item is finished
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		/// <param name="ai">The alarm item that was added or edited</param>
		private void ListenAddEditAlarmItemFinished(object sender, RoutedEventArgs e, AlarmItem ai)
		{
			if (DataContext is Alarm alarm)
			{
				if (alarm.Count >= alarm.MaximumItem)
				{
					UxAddControl.IsEnabled = false;
				}
				else { UxAddControl.IsEnabled = true; }
				if (alarm.SelectedItem != null)
				{
					if (alarm.SelectedItem.IsActivated)
					{
						UxSoundDisplayControl.Visibility = Visibility.Collapsed;
						UxTimeLeftControl.Visibility = Visibility.Visible;
					}
					else { UxTimeLeftControl.Visibility = Visibility.Hidden; }
				}
			}
		}

		/// <summary>
		/// Handles the event when an alarm goes off
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		/// <param name="alarm">The alarm that went off</param>
		private void ListenWentOff(object sender, EventArgs e, object alarm)
		{
			if (DataContext is Alarm a)
			{
				if (alarm is AlarmItem ai)
				{
					UxSoundDisplayControl.DataContext = ai;
					UxTimeLeftControl.Visibility = Visibility.Hidden;
					UxSoundDisplayControl.Visibility = Visibility.Visible;
				}
			}
			UxSnoozeControl.IsEnabled = true;
			UxStopControl.IsEnabled = true;
		}

		/// <summary>
		/// Listens to the drop-down menu click event
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		/// <param name="a">The alarm associated with the drop-down menu</param>
		private void ListenDropDownMenuClicked(object? sender, RoutedEventArgs e, Alarm a)
		{
			if (sender is Button btn)
			{
				if (DataContext is Alarm alarm)
				{
					UxDropDownMenuControl.uxSnoozeCntBox.Count = (uint)alarm.SnoozeTime;
					UxDropDownMenuControl.uxSnoozeCntBox.UpperBound = 30;
					UxDropDownMenuControl.uxMaxAlarmCntBox.Count = (uint)alarm.MaximumItem;
					switch (UxDropDownMenuControl.uxOption.Visibility)
					{
						case Visibility.Visible:
							UxEditControl.Visibility = Visibility.Hidden;
							UxAddControl.Visibility = Visibility.Hidden;
							UxAlarmListControl.Visibility = Visibility.Hidden;
							UxSnoozeControl.Visibility = Visibility.Hidden;
							UxStopControl.Visibility = Visibility.Hidden;
							UxTimeLeftControl.Visibility = Visibility.Hidden;
							UxSoundDisplayControl.Visibility = Visibility.Hidden;
							break;
						case Visibility.Collapsed:
							UxEditControl.Visibility = Visibility.Visible;
							UxAddControl.Visibility = Visibility.Visible;
							UxAlarmListControl.Visibility = Visibility.Visible;
							UxSnoozeControl.Visibility = Visibility.Visible;
							UxStopControl.Visibility = Visibility.Visible;
							if (alarm.SelectedItem != null)
							{
								if (alarm.SelectedItem.WentOff)
								{
									UxSoundDisplayControl.Visibility = Visibility.Visible;
								}
								else
								{
									if (alarm.SelectedItem is AlarmItem aim)
									{
										if (aim.IsActivated)
										{
											UxTimeLeftControl.Visibility = Visibility.Visible;
										}
									}
								}
							}
							if (alarm.MaximumItem <= alarm.Count)
							{
								UxAddControl.IsEnabled = false;
							}
							else { UxAddControl.IsEnabled = true; }
							break;
						default:
							break;
					}
				}
			}
		}

		/// <summary>
		/// Handles the event when the snooze button is clicked
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void ListenSnoozeBtnClicked(object sender, EventArgs e)
		{
			if (UxSoundDisplayControl.Visibility == Visibility.Visible)
			{
				UxSoundDisplayControl.Visibility = Visibility.Collapsed;
				UxTimeLeftControl.Visibility = Visibility.Visible;
			}
		}

		/// <summary>
		/// Handles the event when the stop button is clicked
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void ListenStopBtnClicked(object sender, RoutedEventArgs e)
		{
			if (UxSoundDisplayControl.Visibility == Visibility.Visible)
			{
				UxSoundDisplayControl.Visibility = Visibility.Collapsed;
				UxSnoozeControl.IsEnabled = false;
				UxStopControl.IsEnabled = false;
			}
		}
	}
}