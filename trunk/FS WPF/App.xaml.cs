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

		private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount > 1)
			{
				Panel cell = (Panel)sender;
				FrameworkElement label = (FrameworkElement)cell.Children[0];
				TextBox editor = (TextBox)cell.Children[1];

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
			Panel cell = (Panel)editor.Parent;
			FrameworkElement label = (FrameworkElement)cell.Children[0];

			editor.Visibility = Visibility.Collapsed;
			label.Visibility = Visibility.Visible;
		}

		private void Editor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				FrameworkElement editor = (FrameworkElement)sender;
				editor.Visibility = Visibility.Collapsed;

				e.Handled = true;
			}
			else if (e.Key == Key.Escape)
			{
				TextBox editor = (TextBox)sender;
				Panel cell = (Panel)editor.Parent;
				TextBlock label = (TextBlock)cell.Children[0];

				editor.Text = label.Text;
				editor.Visibility = Visibility.Collapsed;

				e.Handled = true;
			}
		}

	}

}
