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
using Xceed.Wpf.Toolkit;

namespace Alarm501
{
    /// <summary>
    /// Interaction logic for PickDateTime.xaml
    /// </summary>
    public partial class PickDateTime : UserControl
    {
		/// <summary>
		/// An event triggered when the set button clicked
		/// </summary>
		public event EventHandler<RoutedEventArgs> Set = default!;

		/// <summary>
		/// An event triggered when the cancel button clicked
		/// </summary>
		public event EventHandler<RoutedEventArgs> Cancel = default!;

		public PickDateTime()
        {
            InitializeComponent();
        }

		/// <summary>
		/// The event handler for when the user set the alarm
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void SetBtnClicked(object sender, RoutedEventArgs e)
		{
			Set?.Invoke(sender, e);
		}

		/// <summary>
		/// The event handler for when the user click the cancel button
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void CancleBtnClicked(object sender, RoutedEventArgs e)
		{
			Cancel?.Invoke(sender, e);
		}
	}
}
