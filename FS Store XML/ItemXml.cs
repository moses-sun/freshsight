/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	static class ItemXml
	{
		static internal void Save(Item save, XmlElement item, XmlElement contents)
		{
			save.DoSaving();

			XmlDocument doc = item.OwnerDocument;
			XmlAttribute attr;

			// id
			attr = doc.CreateAttribute("id");
			attr.Value = save.Id.ToString();
			item.Attributes.Append(attr);

			// version
			attr = doc.CreateAttribute("version");
			attr.Value = Item.Version;
			item.Attributes.Append(attr);

			// date-
			XmlElement date = doc.CreateElement("date");
			item.AppendChild(date);

			// -create
			attr = doc.CreateAttribute("create");
			attr.Value = save.Created.Ticks.ToString();
			date.Attributes.Append(attr);

			// -saved
			attr = doc.CreateAttribute("saved");
			attr.Value = save.Saved.Ticks.ToString();
			date.Attributes.Append(attr);

			// caption-
			XmlElement caption = doc.CreateElement("caption");
			caption.Value = save.Caption;
			item.AppendChild(caption);

			// -caption style
			XmlElement style = doc.CreateElement("style");
			TextStyleXml.Save(save.CaptionStyle, style);
			caption.AppendChild(style);

			// comment-
			XmlElement comment = doc.CreateElement("comment");
			comment.Value = save.Comment;
			item.AppendChild(comment);

			// -comment style
			style = doc.CreateElement("style");
			TextStyleXml.Save(save.CommentStyle, style);
			comment.AppendChild(style);

			// check-
			XmlElement check = doc.CreateElement("check");
			item.AppendChild(check);

			// -hasCheckbox
			attr = doc.CreateAttribute("has");
			attr.Value = save.HasCheckbox.ToString();
			check.Attributes.Append(attr);

			// -checked
			attr = doc.CreateAttribute("checked");
			attr.Value = save.Checked.ToString();
			check.Attributes.Append(attr);

			// icons-
			XmlElement icons = doc.CreateElement("icons");
			item.AppendChild(icons);

			// -main icon
			attr = doc.CreateAttribute("main");
			attr.Value = save.Icon;
			icons.Attributes.Append(attr);

			// -overlay icon
			attr = doc.CreateAttribute("overlay");
			attr.Value = save.OverlayIcon;
			icons.Attributes.Append(attr);

			// state-
			XmlElement state = doc.CreateElement("state");
			item.AppendChild(state);

			// -expanded
			attr = doc.CreateAttribute("expanded");
			attr.Value = save.Expanded.ToString();
			state.Attributes.Append(attr);

			// -visible
			attr = doc.CreateAttribute("visible");
			attr.Value = save.Visible.ToString();
			state.Attributes.Append(attr);

			// link-
			XmlElement link = doc.CreateElement("link");
			item.AppendChild(link);

			// -tree
			attr = doc.CreateAttribute("tree");
			attr.Value = save.Link.ToString();
			link.Attributes.Append(attr);

			// files-
			XmlElement files = doc.CreateElement("files");
			item.AppendChild(files);

			// -file id
			XmlElement file;

			foreach (int id in save.Files)
			{
				file = doc.CreateElement("file");
				files.AppendChild(file);

				attr = doc.CreateAttribute("id");
				attr.Value = id.ToString();
				file.Attributes.Append(attr);
			}

			// metadata
			XmlElement metadata = doc.CreateElement("metadata");
			MetaXml.Save(save.Metadata, metadata);
			item.AppendChild(metadata);

			// content
			SaveContent(save, contents);

			// childs-
			XmlElement childs = doc.CreateElement("childs");
			item.AppendChild(childs);

			// -item
			XmlElement child;

			foreach (Item val in save.Childs)
			{
				child = doc.CreateElement("item");
				ItemXml.Save(val, child, contents);
				childs.AppendChild(child);
			}
		}

		static private void SaveContent(Item save, XmlElement contents)
		{
			XmlDocument doc = contents.OwnerDocument;
			XmlAttribute attr;

			// content-
			XmlElement cont = doc.CreateElement("content");
			cont.Value = save.Content;
			contents.AppendChild(cont);

			// -base id
			attr = doc.CreateAttribute("base");
			attr.Value = save.Tree.Base.Id.ToString();
			cont.Attributes.Append(attr);

			// -tree id
			attr = doc.CreateAttribute("tree");
			attr.Value = save.Tree.ToString();
			cont.Attributes.Append(attr);

			// -item id
			attr = doc.CreateAttribute("item");
			attr.Value = save.Id.ToString();
			cont.Attributes.Append(attr);
		}

		static internal Item Load(XmlElement items)
		{
			throw new NotImplementedException();
		}
	}

}
