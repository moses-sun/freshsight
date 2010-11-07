using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace ElasticLogic.FreshSight.GUI.WPF
{

	[ValueConversion(typeof(double), typeof(double))]
	class TextSize : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double scale = (double)value;
			double std = SystemFonts.CaptionFontSize;

			return std * scale;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double size = (double)value;
			double std = SystemFonts.CaptionFontSize;

			return size / std;
		}
	}

}
