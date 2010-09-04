/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Collections.Generic;

namespace ElasticLogic.FreshSight.Model
{

	class ItemRelations : ICloneable
	{
		private int id = Values.NotSet;
		private int parent = Values.NotSet;
		private List<int> childs;

		public int Id
		{
			get { return id; }
		}

		public int Parent
		{
			get { return parent; }
			set { parent = (value < 0 ? Values.NotSet : value); }
		}

		public List<int> Childs
		{
			get { return childs; }
		}

		public ItemRelations(int id)
		{
			this.id = id;
			childs = new List<int>();
		}

		public object Clone()
		{
			ItemRelations copy = new ItemRelations(id);
			copy.parent = this.parent;

			foreach (int child in this.childs)
				copy.childs.Add(child);

			return copy;
		}
	}

}
