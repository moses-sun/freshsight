/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Collections.Generic;

namespace ElasticLogic.FreshSight.Model
{

	public class Base
	{
		static readonly public string Version = "1.0"; // Base model version

		private Dictionary<int, Tree> trees;
		private int treeIds = Values.NotSet; // trees id counter

		private int id = Values.NotSet;
		private DateTime created;
		private DateTime saved;

		#region Properties

		public IEnumerable<Tree> Trees
		{
			get { return trees.Values; }
		}

		public int Id
		{
			get { return id; }
		}

		public DateTime Created
		{
			get { return created; }
		}

		public DateTime Saved
		{
			get { return saved; }
		}

		#endregion

		public Base(int id)
		{
			this.id = id;			
			trees = new Dictionary<int, Tree>();

			created = DateTime.Now;
			saved = created;

			Item.CaptionChangeEvent += ItemCaptionChanged;
			Tree.TitleChangeEvent += TreeTitleChanged;
		}

		public Tree GetTree(int id)
		{
			return trees[id];
		}

		public Tree AddTree(string title)
		{
			int id = NewTreeId();

			Tree tree = new Tree(id, title, this);
			trees.Add(id, tree);

			return tree;
		}

		public void RemoveTree(Tree tree)
		{
			// remove link: tree -> item
			Item item = tree.LinkedItem;
			if (item != null)
				item.Link = Values.NotSet;

			// remove links: tree items -> trees
			DeleteItemsLink(tree.Items);

			// remove from list
			trees.Remove(tree.Id);
		}

		private void DeleteItemsLink(IEnumerable<Item> items)
		{
			foreach (Item item in items)
			{
				Tree tree = item.LinkedTree;
				if (tree != null)
				{
					tree.LinkItem = Values.NotSet;
					tree.LinkTree = Values.NotSet;
				}

				DeleteItemsLink(item.Childs);
			}
		}

		public bool LinkItemTree(Item item, Tree tree)
		{
			if (item.Tree.Id != tree.Id)
			{
				item.Link = tree.Id;

				tree.LinkTree = item.Tree.Id;
				tree.LinkItem = item.Id;

				tree.SetTitle(item.Caption);
				return true;
			}
			else
				return false;
		}

		private void ItemCaptionChanged(object source, EventArgs args)
		{
			Item item = (Item)source;
			Tree tree = item.LinkedTree;

			if (tree != null)
				tree.SetTitle(item.Caption);
		}

		private void TreeTitleChanged(object source, EventArgs args)
		{
			Tree tree = (Tree)source;
			Item item = tree.LinkedItem;

			if (item != null)
				item.SetCaption(tree.Title);
		}

		public void DoSaving()
		{
			saved = DateTime.Now;
		}

		public void LoadIds()
		{
			treeIds = Values.NotSet;
			foreach (Tree tree in trees.Values)
			{
				if (tree.Id > treeIds)
					treeIds = tree.Id;
			}
		}

		private int NewTreeId()
		{
			return ++treeIds;
		}

	}

}
