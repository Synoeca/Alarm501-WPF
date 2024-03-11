using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Alarm501
{
	/// <summary>
	/// A controller for managing alarms, providing event-driven communication with UI elements
	/// </summary>
	public class AlarmBinder
	{

		/// <summary>
		/// The underlying model representing the collection of alarms
		/// </summary>
		private Alarm _alarmData;

		public event MainWindow.AlarmItemSelectedEventHandler? OnAlarmItemSelected;
		public event MainWindow.AlarmItemRemovedEventHandler? OnAlarmItemRemoved;
		public event MainWindow.AddEditAlarmFinishedEventHandler? OnAddEditAlarmItemFinished;
		public delegate void EditSetBtnClickedEventHandler(object sender, RoutedEventArgs e, AlarmItem ai);
		public delegate void AddSetBtnClickedEventHandler(object sender, RoutedEventArgs e, AlarmItem ai);

		public AlarmBinder(Alarm model)
		{
			_alarmData = model;
		}

		/// <summary>
		/// Handles alarm item selection events from AlarmListControl
		/// </summary>
		/// <param name="sender">The sender of the event </param>
		/// <param name="e">The event arguments</param>
		public void OnAlarmItemSelectedHandler(object sender, RoutedEventArgs e)
		{
			if (e.Source is ListView lvi)
			{
				if (lvi.SelectedItem is AlarmItem ai)
				{
					_alarmData.SelectedItem = ai;
					OnAlarmItemSelected?.Invoke(sender, e);
				}
			}
		}

		/// <summary>
		/// Handles the Edit button click, opening the AddEditAlarm window for editing the selected alarm
		/// </summary>
		/// <param name="sender">The Button that was clicked</param>
		/// <param name="e">The event arguments </param>
		public void OnEditBtnClickedHandler(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn)
			{
				if (_alarmData.SelectedItem is AlarmItem ai)
				{
					AddEditAlarm addEditAlarm = new()
					{
						DataContext = _alarmData.SelectedItem
					};
					addEditAlarm.OnEditSetBtnClicked += SaveEditAlarmItem;

					if (addEditAlarm.DataContext is AlarmItem eai)
					{
						addEditAlarm.UxPickDateTime.uxTimePicker.Value = eai.Time;
						addEditAlarm.UxOnOffSlider.uxCheckBox.IsChecked = eai.IsActivated;
					}
					addEditAlarm.ShowDialog();
					addEditAlarm.OnEditSetBtnClicked -= SaveEditAlarmItem;
				}
			}
		}

		/// <summary>
		/// Event handler for saving edits made to an alarm item
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		/// <param name="ai">An edited AlarmItem</param>
		private void SaveEditAlarmItem(object sender, RoutedEventArgs e, AlarmItem ai)
		{
			if (sender is Button btn)
			{
				if (btn.DataContext is AlarmItem old)
				{
					DateTime newtime = ai.Time;
					if (ai.Time < DateTime.Now)
					{
						newtime = ai.Time.AddDays(1);
					}
					old.Time = newtime;
					old.IsAm = ai.IsAm;
					old.Sound = ai.Sound;
					old.IsActivated = ai.IsActivated;
					if (ai.IsActivated) { old.WentOff = false; }
					_alarmData.Refresh();
				}
			}
			OnAddEditAlarmItemFinished?.Invoke(sender, e, ai);
		}

		/// <summary>
		/// Handles the Add button click, opening the AddEditAlarm window to create a new alarm
		/// </summary>
		/// <param name="sender">The Button that was clicked</param>
		/// <param name="e">The event arguments</param>
		public void OnAddBtnClickedHandler(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn)
			{
				AddEditAlarm addEditAlarm = new()
				{
					DataContext = _alarmData
				};
				addEditAlarm.OnAddSetBtnClicked += AddNewAlarmItem;
				if (addEditAlarm.DataContext is Alarm alarm)
				{
					addEditAlarm.UxPickDateTime.uxTimePicker.Value = DateTime.Now;
					addEditAlarm.UxSoundEnumBox.uxRadioButtonBox.SelectedItem = AlarmSound.Radar;
				}
				addEditAlarm.ShowDialog();
				addEditAlarm.OnAddSetBtnClicked -= AddNewAlarmItem;
			}
		}

		/// <summary>
		/// Event handler for add a new AlarmItem to the Alarm
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		/// <param name="ai">A new AlarmItem</param>
		private void AddNewAlarmItem(object sender, RoutedEventArgs e, AlarmItem ai)
		{
			if (sender is Button btn)
			{
				DateTime newtime = ai.Time;
				if (ai.Time < DateTime.Now)
				{
					newtime = ai.Time.AddDays(1);
				}
				ai.Time = newtime;
				_alarmData.Add(ai);
			}
			OnAddEditAlarmItemFinished?.Invoke(sender, e, ai);
		}

		/// <summary>
		/// Handles the Remove button click, deleting the selected alarm and notifying subscribers
		/// </summary>
		/// <param name="sender">The Button that was clicked</param>
		/// <param name="e">The event arguments</param>
		public void OnRemoveBtnClickedHandler(object sender, RoutedEventArgs e)
		{
			if (e.Source is Button btn)
			{
				int index = _alarmData.ToList().IndexOf((IAlarmItem)btn.DataContext);
				_alarmData.Remove((IAlarmItem)btn.DataContext);
				OnAlarmItemRemoved?.DynamicInvoke(sender, e, index);
			}
		}

		/// <summary>
		/// Handles the ClockTick event, updating the time left for alarms
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		/// <param name="alarm">The associated alarm</param>
		public void OnClockTickedHandler(object sender, EventArgs e, object alarm)
		{
			if (alarm is Alarm al)
			{
				if (al.SelectedItem is AlarmItem ait)
				{
					ait.TimeLeft = ait.Time - DateTime.Now;
				}
			}
			else if (alarm is AlarmItem ai)
			{
				ai.TimeLeft = ai.Time - DateTime.Now;
			}
		}

		/// <summary>
		/// Handles the WentOff event, setting alarms as not activated and refreshing the data
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		/// <param name="alarm">The associated alarm</param>
		public void OnWentOffHandler(object sender, EventArgs e, object alarm)
		{
			if (alarm is Alarm al)
			{
				if (al.SelectedItem is AlarmItem ait)
				{
					ait.IsActivated = false;
					ait.WentOff = true;
					_alarmData.Refresh();
				}
			}
			else if (alarm is AlarmItem ai)
			{
				ai.IsActivated = false;
				ai.WentOff = true;
				_alarmData.Refresh();
			}
		}

		/// <summary>
		/// Handles the Snooze button click event, snoozing the selected alarm
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		public void OnSnoozeBtnClickedHandler(object sender, EventArgs e)
		{
			if (_alarmData.SelectedItem != null)
			{
				if (_alarmData.SelectedItem is AlarmItem ai)
				{
					if (!ai.IsActivated)
					{
						ai.Time = DateTime.Now.AddMinutes(_alarmData.SnoozeTime);
						ai.WentOff = false;
						ai.IsActivated = true;
						_alarmData.Refresh();
					}
				}
				
			}
		}

		/// <summary>
		/// Handles the drop-down menu button click event, updating alarm settings
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		/// <param name="alarm">The original Alarm data</param>
		public void OnDropDownMenuBtnClickedHandler(object sender, RoutedEventArgs e, Alarm alarm)
		{
			if (sender is Button btn)
			{
				switch (btn.Name)
				{
					case "uxGearBtn":
						break;
					case "uxOKMenuBtn":
						_alarmData.SnoozeTime = alarm.SnoozeTime;
						_alarmData.MaximumItem = alarm.MaximumItem;
						break;
					case "uxCancelMenuBtn":
						break;
				}
			}
		}

		/// <summary>
		/// Handles the Stop button click event, resetting the alarm that went off
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		public void OnStopBtnClickedHandler(object sender, RoutedEventArgs e)
		{
			if (_alarmData.SelectedItem != null)
			{
				if (_alarmData.SelectedItem is AlarmItem ai)
				{
					ai.WentOff = false;
				}
			}
		}
	}
}
