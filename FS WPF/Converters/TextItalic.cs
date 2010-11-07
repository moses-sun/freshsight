using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace ElasticLogic.FreshSight.GUI.WPF
{

	[ValueConversion(typeof(bool), typeof(FontStyle))]
	class TextItalic : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool italic = (bool)value;

			if (italic)
				return FontStyles.Italic;
			else
				return FontStyles.Normal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			FontStyle italic = (FontStyle)value;

			return (italic != FontStyles.Normal);
		}
	}

}
