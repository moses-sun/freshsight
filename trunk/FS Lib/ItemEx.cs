/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;

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

					if (item != null)
					{
						if (!item.Expanded)
							return false;
					}
				}
				while (item != null);
				return true;
			}
		}

		public Item Parent
		{
			get { return owner.GetItemParent(this); }
		}

		internal int ParentId
		{
			get
			{
				Item parent = Parent;
				return parent == null ? Values.NotSet : parent.Id;
			}
		}

		public ReadOnlyCollection<Item> Childs
		{
			get { return owner.GetItemChilds(this); }
		}

		public bool HasChilds
		{
			get { return Childs.Count > 0; }
		}
		
		public Item Next
		{
			get { return owner.GetNextItem(this); }
		}

		public Item Previous
		{
			get { return owner.GetPreviousItem(this); }
		}

		internal int Index
		{
			get { return owner.GetItemIndex(this); }
		}

		public int Level
		{
			get
			{
				int n = 1;
				Item item = this.Parent;

				while (item != null)
				{
					n++;
					item = item.Parent;
				}

				return n;
			}
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

		public void SetCheck(bool hasCheckBox, bool isChecked)
		{
			this.HasCheckbox = hasCheckBox;
			this.Checked = isChecked;
		}

		public void SetIcons(string main, string overlay)
		{
			this.Icon = main;
			this.OverlayIcon = overlay;
		}

	}

	internal static class ItemExt
	{
		internal static int GetId(this Item item)
		{
			return item == null ? Values.NotSet : item.Id;
		}
	}

}
