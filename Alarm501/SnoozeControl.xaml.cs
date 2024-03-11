using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alarm501
{
	/// <summary>
	/// Interaction logic for SnoozeControl.xaml
	/// </summary>
	public partial class SnoozeControl : UserControl
	{
		/// <summary>
		/// Event triggered when an AlarmItem is selected
		/// </summary>
		public event MainWindow.SnoozeEventHandler Snooze = delegate { };

		public SnoozeControl()
		{
			InitializeComponent();
			this.IsEnabled = false;
		}

		/// <summary>
		/// The event handler for when the user clicked Snooze button
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void SnoozeBtnClicked(object sender, RoutedEventArgs e)
		{
			Snooze?.Invoke(sender, e);
		}
	}
}
