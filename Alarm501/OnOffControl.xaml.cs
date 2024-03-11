using System.Windows;
using System.Windows.Controls;

namespace Alarm501
{
	/// <summary>
	/// Interaction logic for OnOffControl.xaml
	/// </summary>
	public partial class OnOffControl : UserControl
	{

		/// <summary>
		/// An event triggered when the menu item picked
		/// </summary>
		public event EventHandler<RoutedEventArgs> OnOffControlLoad = default!;

		public OnOffControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Register dependency property
		/// </summary>
		public static readonly DependencyProperty IsCheckedProperty =
			DependencyProperty.Register(
				nameof(IsChecked),
				typeof(bool),
				typeof(OnOffControl),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
			);

		/// <summary>
		/// Get selected item's bool
		/// </summary>
		public bool IsChecked
		{

			get => (bool)GetValue(IsCheckedProperty);
			set
			{
				SetValue(IsCheckedProperty, value);
			}
		}

		/// <summary>
		/// Handles the Checked event when the item is checked
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void Checked(object sender, RoutedEventArgs e)
		{
			this.IsChecked = true;
		}

		/// <summary>
		/// Handles the Unchecked event when the item is unchecked
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void Unchecked(object sender, RoutedEventArgs e)
		{
			this.IsChecked = false;
		}

		/// <summary>
		/// Handles the event when the OnOffControl is loaded
		/// </summary>
		/// <param name="sender">The object that raised the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			OnOffControlLoad?.Invoke(sender, e);
		}
	}
}
