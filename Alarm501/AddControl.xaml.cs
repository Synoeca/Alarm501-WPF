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
	/// Interaction logic for AddControl.xaml
	/// </summary>
	public partial class AddControl : UserControl
	{
		/// <summary>
		/// Event triggered when an AlarmItem is selected
		/// </summary>
		public event MainWindow.AddBtnClickedEventHandler Add = delegate { };

		public AddControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// The event handler for when the user clicked Edit button
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void AddAlarmBtnClicked(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn)
			{
				Add?.Invoke(btn, e);
			}
		}
	}
}
