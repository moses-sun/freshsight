using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace ElasticLogic.FreshSight.GUI.WPF
{

	class TextLines : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool underlined = (bool)values[0];
			bool striked = (bool)values[1];

			TextDecorationCollection decors = new TextDecorationCollection();

			if (underlined)
				decors.Add(TextDecorations.Underline[0]);
			if (striked)
				decors.Add(TextDecorations.Strikethrough[0]);

			return decors;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			TextDecorationCollection decors = (TextDecorationCollection)value;

			bool underlined = decors.Contains(TextDecorations.Underline[0]);
			bool striked = decors.Contains(TextDecorations.Strikethrough[0]);

			return new object[] { underlined, striked };
		}
	}

}
