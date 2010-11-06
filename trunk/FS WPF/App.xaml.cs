using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElasticLogic.FreshSight.GUI.WPF
{
	
	public partial class App : Application
	{

		private void Column_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount > 1)
			{
				Panel column = (Panel)sender;
				FrameworkElement label = (FrameworkElement)column.Children[0];
				TextBox editor = (TextBox)column.Children[1];

				label.Visibility = Visibility.Collapsed;
				editor.Visibility = Visibility.Visible;

				editor.Focus();
				editor.SelectAll();

				e.Handled = true;
			}
		}

		private void Editor_LostFocus(object sender, RoutedEventArgs e)
		{
			TextBox editor = (TextBox)sender;
			Panel column = (Panel)editor.Parent;
			FrameworkElement label = (FrameworkElement)column.Children[0];

			editor.Visibility = Visibility.Collapsed;
			label.Visibility = Visibility.Visible;
		}

	}

}
