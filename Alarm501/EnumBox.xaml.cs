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
	/// Interaction logic for EnumBox.xaml
	/// </summary>
	public partial class EnumBox : UserControl
	{
		/// <summary>
		/// An event triggered when the menu item picked
		/// </summary>
		public event EventHandler<RoutedEventArgs> EnumBoxLoaded = default!;

		public EnumBox()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Register dependency property
		/// </summary>
		public static readonly DependencyProperty EnumProperty = DependencyProperty.Register(
			nameof(Enums),
			typeof(Enum),
			typeof(EnumBox),
			new FrameworkPropertyMetadata(AlarmSound.Beacon, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


		/// <summary>
		/// Get selected item's enum
		/// </summary>
		public Enum Enums
		{
			get => (Enum)GetValue(EnumProperty);
			set
			{
				SetValue(EnumProperty, value);
			}
		}

		/// <summary>
		/// Handles the event when the EnumBox is loaded
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void EnumboxLoaded(object sender, RoutedEventArgs e)
		{
			EnumBoxLoaded?.Invoke(sender, e);
		}
    }
}
