using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace ElasticLogic.FreshSight.GUI.WPF
{

	class IconVisibility : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			string icon = (string)values[0];
			string overlay = (string)values[1];

			if (!string.IsNullOrEmpty(icon) || !string.IsNullOrEmpty(overlay))
				return Visibility.Visible;
			else
				return Visibility.Collapsed;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
