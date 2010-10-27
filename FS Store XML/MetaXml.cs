/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Xml.Linq;
using ElasticLogic.FreshSight.Model;

namespace ElasticLogic.FreshSight.Repository.Xml
{

	static class MetaXml
	{
		static internal XElement Save(Meta save)
		{
			// metadata-
			XElement metadata = new XElement("metadata");

			foreach (string key in save.Keys)
			{
				metadata.Add(
					new XElement("record", save[key].Text, // record-
						new XAttribute("name", key), // -name
						new XAttribute("kind", save[key].Kind) // -kind
					)
				);
			}

			return metadata;
		}

		static internal Meta Load(XElement metadata)
		{
			throw new NotImplementedException();
		}
	}

}
