/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	static class MetaXml
	{
		static internal void Save(Meta save, XmlElement metadata)
		{
			XmlDocument doc = metadata.OwnerDocument;
			XmlAttribute attr;

			// record-
			XmlElement record;

			foreach (string key in save.Keys)
			{
				record = doc.CreateElement("record");
				record.Value = save[key].Text;
				metadata.AppendChild(record);

				// -name
				attr = doc.CreateAttribute("name");
				attr.Value = key;
				record.Attributes.Append(attr);

				// -kind
				attr = doc.CreateAttribute("kind");
				attr.Value = save[key].Kind.ToString();
				record.Attributes.Append(attr);
			}
		}

		static internal Meta Load(XmlElement metadata)
		{
			throw new NotImplementedException();
		}
	}

}
