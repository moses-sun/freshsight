using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;

namespace ElasticLogic.FreshSight.GUI.WPF
{
	
	class IconConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			string icon = (string)values[0];
			string overlay = (string)values[1];
			Uri uri;

			if (!string.IsNullOrEmpty(icon) || !string.IsNullOrEmpty(overlay))
				uri = new Uri("img/icon.bmp", UriKind.Relative);
			else
				uri = new Uri("img/empty.bmp", UriKind.Relative);

			return new BitmapImage(uri);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
