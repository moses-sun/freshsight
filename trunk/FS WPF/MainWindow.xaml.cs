using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElasticLogic.FreshSight.Model;
using ElasticLogic.FreshSight.Repository.Xml;
using Ricciolo.Controls;

namespace ElasticLogic.FreshSight.GUI.WPF
{

	public partial class MainWindow : Window
	{
		private Base db;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			db = LoadBase();

			Base2Tabs(db, Tabs);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//BaseXml.Save(db, @"d:\TEST");
		}

		// faking Base loading
		private Base LoadBase()
		{
			Base db = new Base(0);
			Tree tree = db.AddTree("Test tree");

			Item root = tree.AddFirstItem("Caption 1");
			root.Comment = "Comment 1";
			root.Icon = "some";

			Item child = tree.AddSubItemLast(root, "Caption 11");
			child.Comment = "Comment 11";
			child.SetCheck(true, true);

			child = tree.AddSubItemLast(root, "Caption 12");
			child.Comment = "Comment 12";
			child.SetCheck(true, false);

			return db;
		}

		private void Base2Tabs(Base db, TabControl tabs)
		{
			foreach (Tree tree in db.Trees)
			{
				TreeListView view = Tree2View(tree);

				TabItem tab = new TabItem();
				tab.Header = tree.Title;
				tab.Content = view;

				tabs.Items.Add(tab);
			}

			if (tabs.Items.Count > 0)
				tabs.SelectedIndex = 0;
		}

		private TreeListView Tree2View(Tree tree)
		{
			TreeListView view = new TreeListView();
			
			// make columns
			GridViewColumn col = new GridViewColumn();
			col.Width = 150;
			col.Header = "Caption";
			col.CellTemplate = (DataTemplate)Application.Current.Resources["Caption"];
			view.Columns.Add(col);

			col = new GridViewColumn();
			col.Width = 200;
			col.Header = "Comment";
			col.CellTemplate = (DataTemplate)Application.Current.Resources["Comment"];
			view.Columns.Add(col);

			// load items
			Item2Branch(tree.Items, view.Items);

			return view;
		}

		private void Item2Branch(IEnumerable<Item> items, ItemCollection branches)
		{
			foreach (Item item in items)
			{
				TreeListViewItem branch = new TreeListViewItem();
				branch.Header = item;
				branches.Add(branch);

				branch.Expanded += delegate(object sender, RoutedEventArgs e) { (branch.Header as Item).Expand(); };
				branch.Collapsed += delegate(object sender, RoutedEventArgs e) { (branch.Header as Item).Collapse(); };
				branch.IsVisibleChanged += delegate(object sender, DependencyPropertyChangedEventArgs e)
				{
					if (branch.Visibility == Visibility.Visible)
						(branch.Header as Item).Show();
					else
						(branch.Header as Item).Hide();
				};
				//branch.Visibility = item.Visible ? Visibility.Visible : Visibility.Collapsed;

				if (item.HasChildren)
				{
					Item2Branch(item.Children, branch.Items);
					branch.IsExpanded = item.Expanded;
				}
			}
		}

	}

}
