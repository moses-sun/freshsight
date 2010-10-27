/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml.Linq;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	static class ItemXml
	{
		static internal XElement Save(Item save)
		{
			save.DoSaving();
			XElement item = new XElement("item");

			item.Add(
				new XAttribute("id", save.Id), // id
				new XAttribute("version", Item.Version), // version
				new XElement("date", // date -
					new XAttribute("create", save.Created.Ticks), // -create
					new XAttribute("saved", save.Saved.Ticks) // -saved
				),
				new XElement("caption", save.Caption, // caption-
					TextStyleXml.Save(save.CaptionStyle) // -style
				),
				new XElement("comment", save.Comment, // comment-
					TextStyleXml.Save(save.CommentStyle) // -style
				),
				new XElement("check", // check-
					new XAttribute("has", save.HasCheckbox), // -hasCheckbox
					new XAttribute("checked", save.Checked) // -checked
				),
				new XElement("icons", // icons-
					new XAttribute("main", save.Icon), // -main
					new XAttribute("overlay", save.OverlayIcon) // -overlay
				),
				new XElement("state", // state-
					new XAttribute("expanded", save.Expanded), // -expanded
					new XAttribute("visible", save.Visible) // -visible
				),
				new XElement("link", // link-
					new XAttribute("tree", save.Link) // -tree
				),
				SaveFiles(save), // files
				MetaXml.Save(save.Metadata), // metadata
				SaveChilds(save) // childs
			);

			return item;
		}

		static internal void SaveContents(Item save, XElement contents)
		{
			contents.Add(
				new XElement("content", save.Content, // content-
					new XAttribute("base", save.Tree.Base.Id), // -base id
					new XAttribute("tree", save.Tree), // -tree id
					new XAttribute("item", save.Id) // -item id
				)
			);

			foreach (Item val in save.Childs)
			{
				SaveContents(val, contents);
			}
		}

		static private XElement SaveFiles(Item save)
		{
			// files-
			XElement files = new XElement("files");

			// -file id
			foreach (int id in save.Files)
			{
				files.Add(
					new XElement("file",
						new XAttribute("id", id)
					)
				);
			}

			return files;
		}

		static private XElement SaveChilds(Item save)
		{
			// childs-
			XElement childs = new XElement("childs");

			foreach (Item item in save.Childs)
			{
				childs.Add(ItemXml.Save(item)); // -item
			}

			return childs;
		}

		static internal Item Load(XElement items)
		{
			throw new NotImplementedException();
		}
	}

}
