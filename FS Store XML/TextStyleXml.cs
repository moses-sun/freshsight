/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	static class TextStyleXml
	{
		static internal void Save(TextStyle save, XmlElement style)
		{
			XmlDocument doc = style.OwnerDocument;
			XmlAttribute attr;

			attr = doc.CreateAttribute("b");
			attr.Value = save.Bold.ToString();
			style.Attributes.Append(attr);

			attr = doc.CreateAttribute("i");
			attr.Value = save.Italic.ToString();
			style.Attributes.Append(attr);

			attr = doc.CreateAttribute("u");
			attr.Value = save.Underlined.ToString();
			style.Attributes.Append(attr);

			attr = doc.CreateAttribute("s");
			attr.Value = save.Striked.ToString();
			style.Attributes.Append(attr);

			attr = doc.CreateAttribute("font");
			attr.Value = save.FontScale.ToString();
			style.Attributes.Append(attr);
		}

		static internal TextStyle Load(XmlElement style)
		{
			throw new NotImplementedException();
		}
	}

}
