/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml.Linq;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	static class TreeXml
	{
		static internal XElement Save(Tree save)
		{
			save.DoSaving();
			XElement tree = new XElement("tree");

			tree.Add(
				new XAttribute("id", save.Id), // id
				new XAttribute("version", Tree.Version), // version
				new XElement("date", // date-
					new XAttribute("created", save.Created.Ticks), // -created
					new XAttribute("saved", save.Saved.Ticks) // -saved
				),
				new XElement("strings", // strings-
					new XElement("title", save.Title) // -title
				),
				new XElement("icon", // icon-
					new XAttribute("init", save.InitIcon), // -init
					new XAttribute("inherit", save.InheritIcon) // -inherit
				),
				new XElement("check", // check-
					new XAttribute("inherit", save.InheritCheckbox) // -inherit
				),
				new XElement("link", // link-
					new XAttribute("item", save.LinkItem), // -item
					new XAttribute("tree", save.LinkTree) // -tree
				),
				new XElement("columns", // columns-
					SaveSelColumns(save) // -select
				),
				SaveItems(save) // items
			);

			return tree;
		}

		static private XElement SaveSelColumns(Tree save)
		{
			XElement select = new XElement("select");

			foreach (string col in save.SelectCols)
			{
				select.Add(
					new XElement("column",
						new XAttribute("name", col)
					)
				);
			}

			return select;
		}

		static private XElement SaveItems(Tree save)
		{
			// items-
			XElement items = new XElement("items");

			foreach (Item item in save.Items)
			{
				items.Add(ItemXml.Save(item)); // -item
			}

			return items;
		}

		static internal void SaveContents(Tree save, XElement contents)
		{
			foreach (Item item in save.Items)
			{
				ItemXml.SaveContents(item, contents);
			}
		}

		static internal Tree Load(XElement tree)
		{
			throw new NotImplementedException();

			//tree.LoadIds();
		}
	}

}
