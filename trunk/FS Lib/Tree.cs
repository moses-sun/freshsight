/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ElasticLogic.FreshSight.Model
{

	public class Tree
	{
		static readonly public string Version = "1.0"; // Tree model version

		private string title = "";
		private Dictionary<int, Item> items;
		private Dictionary<int, ItemRelations> relations;
		private List<string> presentCols; // item	metadata keys
		private string initIconKey = "";

		private int id = Values.NotSet;
		private Base owner;
		private int itemIds = Values.NotSet; // items id counter
		private DateTime created;
		private DateTime saved;

		internal static event EventHandler TitleChangeEvent;

		#region Properties

		public string Title
		{
			get { return title; }
			set
			{
				SetTitle(value);
				OnTitleChange();
			}
		}

		// list of the first-level items
		public ReadOnlyCollection<Item> Items
		{
			get { return GetItemChilds(null); }
		}

		public IList<string> PresentCols
		{
			get { return presentCols.AsReadOnly(); }
		}

		public List<string> SelectCols
		{
			get;
			private set;
		}

		public int Id
		{
			get { return id; }
		}

		public Base Base
		{
			get { return owner; }
		}

		public string InitIcon
		{
			get { return initIconKey; }
			set
			{
				initIconKey = value.Trim() == string.Empty ? null : value;
			}
		}

		public bool InheritIcon { get; set; }

		public bool InheritCheckbox { get; set; }

		public int LinkItem { get; internal set; }

		public int LinkTree { get; internal set; }

		public Tree LinkedTree
		{
			get
			{
				if (LinkTree > Values.NotSet)
				{
					return owner.GetTree(LinkTree);
				}
				else
					return null;
			}
		}

		public Item LinkedItem
		{
			get
			{
				if (LinkTree > Values.NotSet && LinkItem > Values.NotSet)
				{
					return owner.GetTree(LinkTree).GetItem(LinkItem);
				}
				else
					return null;
			}
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

		public Tree(int id, string title, Base owner)
		{
			this.id = id;
			this.SetTitle(title);
			this.owner = owner;

			items = new Dictionary<int, Item>();
			relations = new Dictionary<int, ItemRelations>();

			// '-1' element - for invisible root
			relations.Add(Values.NotSet, new ItemRelations(Values.NotSet));

			presentCols = new List<string>();
			SelectCols = new List<string>();

			InheritCheckbox = true;
			InheritIcon = false;
			LinkItem = Values.NotSet;
			LinkTree = Values.NotSet;

			created = DateTime.Now;
			saved = created;

			Item.KeyAddEvent += ItemKeyAdded;
		}

		public Tree Clone(int id, Base owner)
		{
			Tree copy = new Tree(id, this.title, owner);

			foreach (Item item in this.items.Values)
			{
				int itemId = item.Id;
				Item newItem = item.Clone(itemId, copy);
				copy.items.Add(itemId, newItem);
			}

			foreach (ItemRelations rel in this.relations.Values)
			{
				int relId = rel.Id;
				ItemRelations newRel = (ItemRelations)rel.Clone();
				copy.relations.Add(relId, newRel);
			}

			foreach (string key in this.presentCols)
				copy.presentCols.Add(key);

			foreach (string key in this.SelectCols)
				copy.SelectCols.Add(key);

			copy.initIconKey = this.initIconKey;
			copy.InheritIcon = this.InheritIcon;
			copy.InheritCheckbox = this.InheritCheckbox;
			//copy.LinkItem = this.LinkItem;
			//copy.LinkTree = this.LinkTree;

			copy.itemIds = this.itemIds;
			copy.created = this.created;
			copy.saved = DateTime.Now;

			return copy;
		}

		#region Item getters

		internal Item GetItem(int id)
		{
			return items[id];
		}

		internal Item GetItemParent(Item item)
		{
			int id = item.GetId();
			int parId = relations[id].Parent;

			if (parId != Values.NotSet)
				return items[parId];
			else
				return null;
		}

		internal ReadOnlyCollection<Item> GetItemChilds(Item item)
		{
			int id = item.GetId();
			List<Item> res = new List<Item>();

			foreach (int child in relations[id].Childs)
			{
				res.Add(items[child]);
			}

			return res.AsReadOnly();
		}

		internal int GetItemIndex(Item item)
		{
			int parId = item.ParentId;
			return relations[parId].Childs.IndexOf(item.Id);
		}

		// next item on this level
		internal Item GetNextItem(Item item)
		{
			int parId = item.ParentId;
			int pos = item.Index;

			if (relations[parId].Childs.Count > pos + 1)
			{
				int id = relations[parId].Childs[pos + 1];
				return items[id];
			}
			else
				return null;
		}

		// previous item on this level
		internal Item GetPreviousItem(Item item)
		{
			int parId = item.ParentId;
			int pos = item.Index;

			if (pos > 0)
			{
				int id = relations[parId].Childs[pos - 1];
				return items[id];
			}
			else
				return null;
		}

		#endregion

		#region Item add

		public Item AddFirstItem(string caption)
		{
			return AddSubItem(null, caption, parId => 0);
		}

		public Item AddSubItemFirst(Item parent, string caption)
		{
			return AddSubItem(parent, caption, parId => 0);
		}

		public Item AddSubItemLast(Item parent, string caption)
		{
			return AddSubItem(parent, caption,
				parId => relations[parId].Childs.Count);
		}
		
		public Item AddItemFirst(Item near, string caption)
		{
			return AddSubItem(near.Parent, caption, parId => 0);
		}

		public Item AddItemLast(Item near, string caption)
		{
			return AddSubItem(near.Parent, caption,
				parId => relations[parId].Childs.Count);
		}

		public Item AddItemBefore(Item near, string caption)
		{
			return AddSubItem(near.Parent, caption, parId => near.Index);
		}

		public Item AddItemAfter(Item near, string caption)
		{
			return AddSubItem(near.Parent, caption, parId => near.Index + 1);
		}

		private Item AddSubItem(Item parent, string caption, InsertPosition Insert)
		{
			// items
			int id = NewItemId();
			Item item = new Item(id, caption, this);
			items.Add(id, item);

			if (parent != null)
				InitItem(item, parent);

			// relations
			int parId = parent.GetId();
			int pos = Insert(parId);
			relations[parId].Childs.Insert(pos, id);

			ItemRelations relation = new ItemRelations(id);
			relation.Parent = parId;
			relations.Add(id, relation);

			// result
			return item;
		}

		private void InitItem(Item item, Item parent)
		{
			if (InheritCheckbox)
				item.HasCheckbox = parent.HasCheckbox;

			if (InheritIcon)
				item.Icon = parent.Icon;
			else
				item.Icon = initIconKey;
		}

		#endregion

		#region Item copy

		public void DeepCloneItemBefore(Item item, Item near)
		{
			DeepCloneItemNear(item, near, false);
		}

		public void DeepCloneItemAfter(Item item, Item near)
		{
			DeepCloneItemNear(item, near, true);
		}

		private void DeepCloneItemNear(Item item, Item near, bool after)
		{
			int parId = near.ParentId;
			int pos = near.Index;

			if (after) pos++;
			DeepCloneStep(item, parId, pos);
		}

		private void DeepCloneStep(Item item, int parId, int pos)
		{
			int id = NewItemId();
			Item copy = item.Clone(id, this);
			items.Add(id, copy);

			ItemRelations relation = new ItemRelations(id);
			relation.Parent = parId;
			relations.Add(id, relation);

			relations[parId].Childs.Insert(pos, id);

			foreach (Item child in item.Childs)
			{
				pos = 0;
				DeepCloneStep(child, id, pos++);
			}
		}

		#endregion

		#region Item remove

		public void RemoveItem(Item item)
		{
			List<int> dels = new List<int>();
			DeleteItems(item.Id, ref dels);
		}

		private void DeleteItems(int id, ref List<int> dels)
		{
			foreach (int child in relations[id].Childs)
			{
				DeleteItems(child, ref dels);
			}

			int parId = relations[id].Parent;
			relations[parId].Childs.Remove(id);
			relations.Remove(id);

			items.Remove(id);
			dels.Add(id);
		}

		#endregion

		#region Item moving

		public bool MoveItemUp(Item item)
		{
			int id = item.Id;
			int parId = item.ParentId;

			List<int> childs = relations[parId].Childs;
			int pos = childs.IndexOf(id);

			if (pos > 0)
			{
				childs.Remove(id);
				childs.Insert(pos - 1, id);
				return true;
			}
			else
				return false;
		}

		public bool MoveItemDown(Item item)
		{
			int id = item.Id;
			int parId = item.ParentId;

			List<int> childs = relations[parId].Childs;
			int pos = childs.IndexOf(id);

			if (pos < childs.Count - 1)
			{
				childs.Remove(id);
				childs.Insert(pos + 1, id);
				return true;
			}
			else
				return false;
		}

		public bool ShiftItemLevelUp(Item item)
		{
			Item parent = item.Parent;
			if (parent == null)
				return false; // already first level

			int id = item.Id;
			int parId = parent.Id;
			int grandId = parent.ParentId;

			relations[parId].Childs.Remove(id);
			
			int pos = relations[grandId].Childs.IndexOf(parId);
			relations[grandId].Childs.Insert(pos + 1, id);

			relations[id].Parent = grandId;

			return true;
		}

		public void MoveItemBefore(Item item, Item near)
		{
			MoveItemNear(item, near, false);
		}

		public void MoveItemAfter(Item item, Item near)
		{
			MoveItemNear(item, near, true);
		}

		private void MoveItemNear(Item item, Item near, bool after)
		{
			int id = item.Id;
			int parId = item.ParentId;
			relations[parId].Childs.Remove(id);

			int pos = near.Index;
			if (after) pos++;

			parId = near.ParentId;
			relations[parId].Childs.Insert(pos, id);
			relations[id].Parent = parId;
		}

		public void SortItems(Item parent, bool reverse)
		{
			int parId = parent.GetId();

			relations[parId].Childs.Sort();
			if (reverse)
				relations[parId].Childs.Reverse();
		}

		public void SortItems(Item parent)
		{
			SortItems(parent, false);
		}

		public void DeepSortItems(Item parent, bool reverse)
		{
			SortItems(parent, reverse);

			foreach (Item item in parent.Childs)
				DeepSortItems(item, reverse);
		}

		public void DeepSortItems(Item parent)
		{
			DeepSortItems(parent, false);
		}

		#endregion

		private void ItemKeyAdded(object source, Item.KeyAddEventArgs args)
		{
			string key = args.Key;
			if (!presentCols.Contains(key))
				presentCols.Add(key);
		}

		private void OnTitleChange()
		{
			if (TitleChangeEvent != null)
				TitleChangeEvent(this, EventArgs.Empty);
		}

		public void DoSaving()
		{
			saved = DateTime.Now;
		}

		public void LoadIds()
		{
			itemIds = Values.NotSet;

			foreach (int id in items.Keys)
			{
				if (id > itemIds)
					itemIds = id;
			}
		}

		public void LoadColumns()
		{
			foreach (Item item in items.Values)
			{
				foreach (string key in item.Metadata.Keys)
				{
					if (!presentCols.Contains(key))
						presentCols.Add(key);
				}
			}
		}

		internal void SetTitle(string value)
		{
			if (value == null || value.Trim() == string.Empty)
				throw new ArgumentException("Incorrect caption");
			title = StringWork.Prepare(value);
		}

		private int NewItemId()
		{
			return ++itemIds;
		}

		public override string ToString()
		{
			return title;
		}

		private delegate int InsertPosition(int parentId);

	}

}
