using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace ElasticLogic.FreshSight.GUI.WPF
{

	[ValueConversion(typeof(bool), typeof(FontWeight))]
	class TextBold : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool bold = (bool)value;

			if (bold)
				return FontWeights.Bold;
			else
				return FontWeights.Normal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			FontWeight bold = (FontWeight)value;

			return (bold != FontWeights.Normal);
		}
	}

}
