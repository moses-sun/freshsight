/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml.Linq;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	public static class BaseXml
	{
		static public void Save(Base save, string folder)
		{
			save.DoSaving();

			// base
			XDocument all = new XDocument(
				new XDeclaration("1.0", "UTF-8", null),
				SaveBase(save)
			);

			string path = System.IO.Path.Combine(folder, "base.xml");
			all.Save(path, SaveOptions.OmitDuplicateNamespaces);

			// contents
			XDocument contents = new XDocument(
				new XDeclaration("1.0", "UTF-8", null),
				SaveContents(save)
			);

			path = System.IO.Path.Combine(folder, "content.xml");
			contents.Save(path, SaveOptions.OmitDuplicateNamespaces);
		}

		static private XElement SaveBase(Base save)
		{
			XElement root = new XElement("base");

			root.Add(
				new XAttribute("id", save.Id), // id
				new XAttribute("version", Base.Version), // version
				new XElement("date", // date-
					new XAttribute("created", save.Created.Ticks), // -created
					new XAttribute("saved", save.Saved.Ticks) // -saved
				),
				SaveTrees(save)
			);

			return root;
		}

		static private XElement SaveTrees(Base save)
		{
			// trees-
			XElement trees = new XElement("trees");

			foreach (Tree tree in save.Trees)
			{
				trees.Add(TreeXml.Save(tree)); // -tree
			}

			return trees;
		}

		static private XElement SaveContents(Base save)
		{
			// contents-
			XElement contents = new XElement("contents");

			foreach (Tree tree in save.Trees)
			{
				TreeXml.SaveContents(tree, contents);
			}

			return contents;
		}

		static public Base Load(string folder)
		{
			throw new NotImplementedException();

			//db.LoadIds();
		}
	}

}
