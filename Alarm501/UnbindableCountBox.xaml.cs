using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UnbindableCountBox.xaml
    /// </summary>
    public partial class UnbindableCountBox : UserControl, INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler? PropertyChanged;

		public UnbindableCountBox()
        {
            InitializeComponent();
        }

		/// <summary>
		/// A private backing field for the property Count
		/// </summary>
		private uint _count;

		/// <summary>
		/// Count property of selected item
		/// </summary>
		public uint Count
		{
			get => _count;
			set
			{
				if(value >= LowerBound && value <= UpperBound)
				{
					_count = value;
				}
				else if (value < LowerBound)
				{
					_count = LowerBound;
				}
				else
				{
					_count = UpperBound;
				}
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
			}
		}

		/// <summary>
		/// Lower bound of the CountBox
		/// </summary>
		public uint LowerBound { get; set; } = 0;

		/// <summary>
		/// Upper bound of the CountBox
		/// </summary>
		public uint UpperBound { get; set; } = uint.MaxValue;

		/// <summary>
		/// Handle increment
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void HandleIncrement(object sender, RoutedEventArgs e)
		{
			if (Count != uint.MaxValue) Count++;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
			e.Handled = true;
		}

		/// <summary>
		/// Handle decrement
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">Metadata for the event</param>
		private void HandleDecrement(object sender, RoutedEventArgs e)
		{
			if (Count != uint.MinValue) Count--;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
			e.Handled = true;
		}
	}
}
