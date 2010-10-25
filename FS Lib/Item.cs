/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Collections.Generic;

namespace ElasticLogic.FreshSight.Model
{

	public partial class Item
	{
		static readonly public string Version = "1.0"; // Item model version

		private string caption = "";
		private string comment = "";
		private string content = "";

		private bool hasCheckbox = false;
		private bool _checked = false;
		private string mainIconKey = "";
		private string overlayIconKey = "";

		private int id = Values.NotSet;
		private Tree owner;
		private DateTime created;
		private DateTime saved;

		internal static event KeyAddHandler KeyAddEvent;
		internal static event EventHandler CaptionChangeEvent;

		#region Properties

		public string Caption
		{
			get { return caption; }
			set
			{
				SetCaption(value);
				OnCaptionChange();
			}
		}

		public string Comment
		{
			get { return comment; }
			set
			{
				comment = value == null ? string.Empty : StringWork.Prepare(value);
			}
		}

		public string Content
		{
			get { return content; }
			set { content = value == null ? string.Empty : value; }
		}

		public FileList Files // attached files
		{
			get;
			private set;
		}

		public Meta Metadata // name - value
		{
			get;
			private set;
		}

		public bool Expanded { get; private set; }

		public bool Visible { get; private set; }

		public bool HasCheckbox
		{
			get { return hasCheckbox; }
			set
			{
				hasCheckbox = value;
				if (!value)
					_checked = false;
			}
		}

		public bool Checked
		{
			get { return _checked; }
			set
			{
				if (hasCheckbox)
					_checked = value;
			}
		}

		public string Icon
		{
			get { return mainIconKey; }
			set
			{
				mainIconKey = value.Trim();
			}
		}

		public string OverlayIcon
		{
			get { return overlayIconKey; }
			set
			{
				overlayIconKey = value.Trim();
			}
		}

		public TextStyle CaptionStyle { get; set; }

		public TextStyle CommentStyle { get; set; }

		public int Link { get; internal set; }

		public DateTime Created
		{
			get { return created; }
		}

		public DateTime Saved
		{
			get { return saved; }
		}

		public int Id
		{
			get { return id; }
		}

		public Tree Tree
		{
			get { return owner; }
		}

		#endregion

		public Item(int id, string caption, Tree owner)
		{
			this.id = id;
			this.SetCaption(caption);
			this.owner = owner;

			Files = new FileList();
			Metadata = new Meta();

			Expanded = true;
			Visible = true;
			Link = Values.NotSet;

			CaptionStyle = new TextStyle();
			CommentStyle = new TextStyle();

			created = DateTime.Now;
			saved = created;

			Metadata.NewDataEvent += DataKeyAdded;
		}

		public Item Clone(int id, Tree owner)
		{
			Item copy = new Item(id, this.caption, owner);

			copy.comment = this.comment;
			copy.content = this.content;

			copy.Files = (FileList)this.Files.Clone();
			copy.Metadata = (Meta)this.Metadata.Clone();

			copy.Expanded = this.Expanded;
			copy.Visible = this.Visible;
			copy.hasCheckbox = this.hasCheckbox;
			copy._checked = this._checked;
			copy.mainIconKey = this.mainIconKey;
			copy.overlayIconKey = this.overlayIconKey;
			//copy.Link = this.Link;

			copy.CaptionStyle = this.CaptionStyle;
			copy.CommentStyle = this.CommentStyle;
			copy.created = this.created;
			copy.saved = DateTime.Now;

			return copy;
		}

		public bool Expand()
		{
			if (!Expanded)
			{
				Expanded = true;
				return true;
			}
			else
				return false;
		}

		public bool Collapse()
		{
			if (Expanded)
			{
				if (HasChilds)
				{
					Expanded = false;
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}

		public bool ToggleExpand()
		{
			if (HasChilds)
			{
				Expanded = !Expanded;
				return true;
			}
			else
				return false;
		}

		public bool Show()
		{
			if (!Visible)
			{
				Visible = true;
				return true;
			}
			else
				return false;
		}

		public bool Hide()
		{
			if (Visible)
			{
				Visible = false;
				return true;
			}
			else
				return false;
		}

		public bool Check()
		{
			if (hasCheckbox)
			{
				_checked = true;
				return true;
			}
			else
				return false;
		}

		public bool Uncheck()
		{
			if (hasCheckbox)
			{
				_checked = false;
				return true;
			}
			else
				return false;
		}

		public bool ToggleCheck()
		{
			if (hasCheckbox)
			{
				_checked = !_checked;
				return true;
			}
			else
				return false;
		}

		public void EmptyIcon()
		{
			mainIconKey = string.Empty;
			overlayIconKey = string.Empty;
		}

		public void EmptyOverlayIcon()
		{
			overlayIconKey = null;
		}

		private void DataKeyAdded(string name)
		{
			if (KeyAddEvent != null)
				KeyAddEvent(this, new KeyAddEventArgs { Key = name });
		}

		private void OnCaptionChange()
		{
			if (CaptionChangeEvent != null)
				CaptionChangeEvent(this, EventArgs.Empty);
		}

		internal void SetCaption(string value)
		{
			if (value == null || value.Trim() == string.Empty)
				throw new ArgumentException("Incorrect caption");
			caption = StringWork.Prepare(value);
		}

		public void DoSaving()
		{
			saved = DateTime.Now;
		}

		public override string ToString()
		{
			return caption;
		}

		#region Sub-types

		internal class KeyAddEventArgs : EventArgs
		{
			public string Key { get; set; }
		}

		internal delegate void KeyAddHandler(object source, KeyAddEventArgs args);

		#endregion

	}

}
