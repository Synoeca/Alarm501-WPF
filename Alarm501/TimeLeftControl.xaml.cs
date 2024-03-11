using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using static Alarm501.MainWindow;

namespace Alarm501
{
	/// <summary>
	/// Represents a control for displaying the time left until an alarm goes off.
	/// </summary>
	public partial class TimeLeftControl : UserControl
	{
		public event ClockEventHandler Clock = delegate { };
		public event WentOffEventHandler WentOff = delegate { };

		/// <summary>
		/// Event triggered when the clock ticks.
		/// </summary>
		private Timer _timer = default!;

		/// <summary>
		/// Event triggered when an alarm goes off.
		/// </summary>
		public TimeLeftControl()
		{
			InitializeComponent();
			this.Visibility = Visibility.Collapsed;
			this.uxSoundLabel.Visibility = Visibility.Hidden;
			InitializeTimer();
			Application.Current.Exit += CurrentExit;
		}

		/// <summary>
		/// Initializes the timer for updating the time left display
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown when the dispatcher is not the UI dispatcher</exception>
		private void InitializeTimer()
		{
			if (this.Dispatcher != Application.Current.Dispatcher)
			{
				throw new InvalidOperationException("Dispatcher is not UI dispatcher.");
			}
			_timer = new System.Threading.Timer(TimerTick!, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
		}

		/// <summary>
		/// Handles the tick event of the timer
		/// </summary>
		/// <param name="state">The state object</param>
		private void TimerTick(object state)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				OnClockEvent(null, EventArgs.Empty);
			});
		}

		/// <summary>
		/// Disposes the timer
		/// </summary>
		private void CleanupTimer()
		{
			_timer?.Change(Timeout.Infinite, Timeout.Infinite);
			_timer?.Dispose();
		}

		/// <summary>
		/// Handles the exit event of the application
		/// </summary>
		/// <param name="sender">The sender of the event</param>
		/// <param name="e">The event arguments</param>
		private void CurrentExit(object sender, ExitEventArgs e)
		{
			CleanupTimer();
		}

		/// <summary>
		/// Handles the clock tick event
		/// </summary>
		/// <param name="sender">The sender of the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClockEvent(object? sender, EventArgs e)
		{
			Clock?.Invoke(sender!, e, DataContext);

			if (DataContext is AlarmItem ai)
			{
				if (ai.IsActivated && ai.TimeLeft <= TimeSpan.Zero)
				{
					this.uxTimeLeftLabel.Content = ai.DisplayTimeLeft;
					this.uxSoundLabel.Visibility = Visibility.Visible;
					WentOff?.Invoke(sender!, e, DataContext);
				}
				else
				{
					this.uxTimeLeftLabel.Content = ai.DisplayTimeLeft;
					this.uxSoundLabel.Visibility = Visibility.Hidden;
				}
			}
			else if (DataContext is Alarm alarm && alarm.SelectedItem is AlarmItem ait)
			{
				if (ait.IsActivated && ait.TimeLeft <= TimeSpan.Zero)
				{
					this.uxTimeLeftLabel.Content = ait.DisplayTimeLeft;
					this.uxSoundLabel.Visibility = Visibility.Visible;
					WentOff?.Invoke(sender!, e, DataContext);
				}
				else
				{
					this.uxTimeLeftLabel.Content = ait.DisplayTimeLeft;
					this.uxSoundLabel.Visibility = Visibility.Hidden;
				}
			}
		}
	}
}
