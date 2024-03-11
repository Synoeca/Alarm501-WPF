using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Alarm501
{
	/// <summary>
	/// Interaction logic for AlarmListControl.xaml
	/// </summary>
	public partial class AlarmListControl : UserControl
	{
		/// <summary>
		/// Event triggered when an AlarmItem is selected
		/// </summary>
		public event MainWindow.AlarmItemSelectedEventHandler Select = delegate { };

		/// <summary>
		/// An event triggered when the Remove Button is Clicked
		/// </summary>
		public event MainWindow.RemoveBtnClickedEventHandler Remove = default!;

		public AlarmListControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Auto resize gridview columns when the main windows size changes
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void AutoResizeGridColumns(object sender, SizeChangedEventArgs e)
		{
			if (sender is ListView listView)
			{
				if (listView.View is GridView gView)
				{
					var workingWidth = (listView.ActualWidth - SystemParameters.VerticalScrollBarWidth) * 0.96;
					var col1 = 0.39;
					var col2 = 0.25;
					var col3 = 0.25;
					var col4 = 0.11;

					gView.Columns[0].Width = workingWidth * col1;
					gView.Columns[1].Width = workingWidth * col2;
					gView.Columns[2].Width = workingWidth * col3;
					gView.Columns[3].Width = workingWidth * col4;
				}
			}
		}

		/// <summary>
		/// The event handler for when the user selected menu items.
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		public void AlarmsListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ListView lvi)
			{
				Select?.Invoke(lvi, e);
			}
		}

		/// <summary>
		/// The event handler for when the user clicked the button of remove item
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void RemoveItem(object sender, RoutedEventArgs e)
		{
			var button = (Button)sender;
			if (DataContext is Alarm alarm)
			{
				Remove?.Invoke(AlarmsListView, e);
			}
		}
	}
}
