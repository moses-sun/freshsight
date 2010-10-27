/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml.Linq;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	static class TextStyleXml
	{
		static internal XElement Save(TextStyle save)
		{
			return
				new XElement("style",
					new XAttribute("b", save.Bold),
					new XAttribute("i", save.Italic),
					new XAttribute("u", save.Underlined),
					new XAttribute("s", save.Striked),
					new XElement("font",
						new XAttribute("scale", save.FontScale)
					)
				);
		}

		static internal TextStyle Load(XElement style)
		{
			throw new NotImplementedException();
		}
	}

}
