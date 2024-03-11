using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alarm501
{
	/// <summary>
	/// Converter to convert an enum value to its string representation in uppercase
	/// </summary>
	public class EnumToUpperCaseConverter : IValueConverter
	{
		/// <summary>
		/// Converts an enum value to its string representation in uppercase
		/// </summary>
		/// <param name="value">The enum value to convert</param>
		/// <param name="targetType">The type of the binding target property</param>
		/// <param name="parameter">The converter parameter to use</param>
		/// <param name="culture">The culture to use in the converter</param>
		/// <returns>The string representation of the enum value in uppercase</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Enum enumValue)
			{
				return enumValue.ToString().ToUpper();
			}
			return value;
		}

		/// <summary>
		/// Converts a string representation of an enum value back to its enum value
		/// </summary>
		/// <param name="value">The string representation of the enum value</param>
		/// <param name="targetType">The type to convert to</param>
		/// <param name="parameter">The converter parameter to use</param>
		/// <param name="culture">The culture to use in the converter</param>
		/// <returns>The converted enum value</returns>
		/// <exception cref="NotImplementedException">Thrown if conversion back is attempted, as it is not supported.</exception>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
