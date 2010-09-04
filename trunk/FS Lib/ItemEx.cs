/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System.Collections.Generic;

namespace ElasticLogic.FreshSight.Model
{

	public partial class Item
	{

		public bool DeepVisible
		{
			get
			{
				Item item = this;
				do
				{
					if (!item.Visible)
						return false;
					item = item.Parent;
				}
				while (item != null);
				return true;
			}
		}

		public Item Parent
		{
			get { return owner.GetItemParent(this); }
		}

		public int ParentId
		{
			get
			{
				Item parent = Parent;
				return parent == null ? Values.NotSet : parent.Id;
			}
		}

		public IEnumerable<Item> Childs
		{
			get { return owner.GetItemChilds(this); }
		}

		public Item Next
		{
			get { return owner.GetNextItem(this); }
		}

		public Item Previous
		{
			get { return owner.GetPreviousItem(this); }
		}

		public int Index
		{
			get { return owner.GetItemIndex(this); }
		}

		public void DeepExpand()
		{
			Expand();

			foreach (Item child in Childs)
				child.DeepExpand();
		}

		public void DeepCollapse()
		{
			Collapse();

			foreach (Item child in Childs)
				child.DeepCollapse();
		}

		public void DeepCheck()
		{
			Check();

			foreach (Item child in Childs)
				child.DeepCheck();
		}

		public void DeepUncheck()
		{
			Uncheck();

			foreach (Item child in Childs)
				child.DeepUncheck();
		}

		public Tree LinkedTree
		{
			get
			{
				if (Link > Values.NotSet)
				{
					return owner.Base.GetTree(Link);
				}
				else
					return null;
			}
		}

		static public int GetId(Item item)
		{
			return item == null ? Values.NotSet : item.Id;
		}

	}

}
