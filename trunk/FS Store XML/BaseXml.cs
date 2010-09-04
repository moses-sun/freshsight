/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	public static class BaseXml
	{
		static public void Save(Base save, string folder)
		{
			save.DoSaving();

			// content
			XmlDocument cont = new XmlDocument();
			cont.PreserveWhitespace = false;

			XmlDeclaration decl = cont.CreateXmlDeclaration("1.0", "UTF-8", null);
			cont.AppendChild(decl);

			XmlElement contents = cont.CreateElement("contents");
			cont.AppendChild(contents);

			// manage
			XmlDocument all = new XmlDocument();
			all.PreserveWhitespace = false;

			decl = all.CreateXmlDeclaration("1.0", "UTF-8", null);
			all.AppendChild(decl);

			XmlElement _base = all.CreateElement("base");
			all.AppendChild(_base);

			SaveAll(save, _base, contents);

			// saving
			folder = folder.TrimEnd(new[] { '\\' });
			all.Save(folder + "\\base.xml");
			cont.Save(folder + "\\content.xml");
		}

		static private void SaveAll(Base save, XmlElement _base, XmlElement contents)
		{
			XmlDocument doc = _base.OwnerDocument;
			XmlAttribute attr;

			// id
			attr = doc.CreateAttribute("id");
			attr.Value = save.Id.ToString();
			_base.Attributes.Append(attr);

			// version
			attr = doc.CreateAttribute("version");
			attr.Value = Base.Version;
			_base.Attributes.Append(attr);

			// date-
			XmlElement date = doc.CreateElement("date");
			_base.AppendChild(date);

			// -created
			attr = doc.CreateAttribute("created");
			attr.Value = save.Created.Ticks.ToString();
			date.Attributes.Append(attr);

			// -saved
			attr = doc.CreateAttribute("saved");
			attr.Value = save.Saved.Ticks.ToString();
			date.Attributes.Append(attr);

			// trees-
			XmlElement trees = doc.CreateElement("trees");
			_base.AppendChild(trees);

			// -tree
			XmlElement tree;

			foreach (Tree val in save.Trees)
			{
				tree = doc.CreateElement("tree");
				TreeXml.Save(val, tree, contents);
				trees.AppendChild(tree);
			}
		}

		static public Base Load(string folder)
		{
			throw new NotImplementedException();

			//db.LoadIds();
		}
	}

}
