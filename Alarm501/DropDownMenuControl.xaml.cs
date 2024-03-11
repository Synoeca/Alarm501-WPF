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
    /// Interaction logic for DropDownMenuControl.xaml
    /// </summary>
    public partial class DropDownMenuControl : UserControl
    {
		/// <summary>
		/// An event triggered when the set button clicked
		/// </summary>
		public event MainWindow.DropDownMenuClickEventHandler ClickMenu = delegate { };

		public DropDownMenuControl()
        {
            InitializeComponent();
            this.uxOption.Visibility = Visibility.Collapsed;
			this.uxOKMenuBtn.Visibility = Visibility.Collapsed;
			this.uxCancelMenuBtn.Visibility = Visibility.Collapsed;
        }

		/// <summary>
		/// Handles the button click event for toggling the visibility of the menu options
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void ButtonClick(object sender, RoutedEventArgs e)
		{
            if (this.uxOption.Visibility == Visibility.Visible)
            {
				this.uxOption.Visibility = Visibility.Collapsed;
				this.uxOKMenuBtn.Visibility = Visibility.Collapsed;
				this.uxCancelMenuBtn.Visibility = Visibility.Collapsed;
			}
            else
            {
				this.uxOption.Visibility = Visibility.Visible;
				this.uxOKMenuBtn.Visibility = Visibility.Visible;
				this.uxCancelMenuBtn.Visibility = Visibility.Visible;
			}
			Alarm newAlarm = [];
			if (sender is Button btn)
			{
				if (btn.Name == "uxOKMenuBtn")
				{
					newAlarm.SnoozeTime = (int)this.uxSnoozeCntBox.Count;
					newAlarm.MaximumItem = (int) this.uxMaxAlarmCntBox.Count;
				}
			}
			ClickMenu?.Invoke(sender, e, newAlarm);
		}
	}
}
