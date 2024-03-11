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
	/// Interaction logic for StopControl.xaml
	/// </summary>
	public partial class StopControl : UserControl
	{
		/// <summary>
		/// Event triggered when an AlarmItem is selected
		/// </summary>
		public event MainWindow.StopEventHandler Stop = delegate { };

		public StopControl()
		{
			InitializeComponent();
			this.IsEnabled = false;
		}

		/// <summary>
		/// Handles the event when the stop button is clicked
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void StopBtnClicked(object sender, RoutedEventArgs e)
		{
			Stop?.Invoke(sender, e);
		}
	}
}
