/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	static class TreeXml
	{
		static internal void Save(Tree save, XmlElement tree, XmlElement contents)
		{
			save.DoSaving();

			XmlDocument doc = tree.OwnerDocument;
			XmlAttribute attr;

			// id
			attr = doc.CreateAttribute("id");
			attr.Value = save.Id.ToString();
			tree.Attributes.Append(attr);

			// version
			attr = doc.CreateAttribute("version");
			attr.Value = Tree.Version;
			tree.Attributes.Append(attr);

			// date-
			XmlElement date = doc.CreateElement("date");
			tree.AppendChild(date);

			// -created
			attr = doc.CreateAttribute("created");
			attr.Value = save.Created.Ticks.ToString();
			date.Attributes.Append(attr);

			// -saved
			attr = doc.CreateAttribute("saved");
			attr.Value = save.Saved.Ticks.ToString();
			date.Attributes.Append(attr);

			// strings-
			XmlElement strings = doc.CreateElement("strings");
			tree.AppendChild(strings);

			// -title
			XmlElement title = doc.CreateElement("title");
			title.Value = save.Title;
			strings.AppendChild(title);

			// icon-
			XmlElement icon = doc.CreateElement("icon");
			tree.AppendChild(icon);

			// -init
			attr = doc.CreateAttribute("init");
			attr.Value = save.InitIcon;
			icon.Attributes.Append(attr);

			// -inherit
			attr = doc.CreateAttribute("inherit");
			attr.Value = save.InheritIcon.ToString().ToLower();
			icon.Attributes.Append(attr);

			// check-
			XmlElement check = doc.CreateElement("check");
			tree.AppendChild(check);

			// -inherit
			attr = doc.CreateAttribute("inherit");
			attr.Value = save.InheritCheckbox.ToString().ToLower();
			check.Attributes.Append(attr);

			// link-
			XmlElement link = doc.CreateElement("link");
			tree.AppendChild(link);

			// -item
			attr = doc.CreateAttribute("item");
			attr.Value = save.LinkItem.ToString();
			link.Attributes.Append(attr);

			// -tree
			attr = doc.CreateAttribute("tree");
			attr.Value = save.LinkTree.ToString();
			link.Attributes.Append(attr);

			// columns-
			XmlElement columns = doc.CreateElement("columns");
			tree.AppendChild(columns);

			// -select-
			XmlElement select = doc.CreateElement("select");
			columns.AppendChild(select);

			// -column
			XmlElement column;

			foreach (string col in save.SelectCols)
			{
				column = doc.CreateElement("column");
				select.AppendChild(column);

				attr = doc.CreateAttribute("name");
				attr.Value = col;
				column.Attributes.Append(attr);
			}

			// items-
			XmlElement items = doc.CreateElement("items");
			tree.AppendChild(items);

			// -item
			XmlElement item;

			foreach (Item val in save.Items)
			{
				item = doc.CreateElement("item");
				ItemXml.Save(val, item, contents);
				items.AppendChild(item);
			}
		}

		static internal Tree Load(XmlElement tree)
		{
			throw new NotImplementedException();

			//tree.LoadIds();
		}
	}

}
