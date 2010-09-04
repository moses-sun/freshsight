/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Collections.Generic;

namespace ElasticLogic.FreshSight.Model
{

	public class Meta : ICloneable
	{
		private const int minCapacity = 4;

		private Data[] records;
		private int count;
		private int capacity;

		internal event NewDataHandler NewDataEvent;

		public Data this[string name]
		{
			get { return GetData(name); }
			set { SetData(name, value); }
		}

		public int Count
		{
			get { return count; }
		}

		public IEnumerable<string> Keys
		{
			get
			{
				for (int i = 0; i < count; i++)
					yield return records[i].Name;
			}
		}

		public Meta()
		{
			Clean();
		}

		public void Clean()
		{
			records = null;
			count = 0;
			capacity = 0;
		}

		public void SetData(string name, object value)
		{
			PrepareName(ref name);
			Data data = new Data(name, value);

			SetData(name, data);
		}

		public void SetData(string name, object value, Data.Type type)
		{
			PrepareName(ref name);
			Data data = new Data(name, value, type);

			SetData(name, data);
		}

		private void SetData(string name, Data data)
		{
			int pos = IndexOf(name);

			if (pos == Values.NotSet)
			{
				pos = AddPlace();
				OnDataAdded(name);
			}

			records[pos] = data;
		}

		private Data GetData(string name)
		{
			int pos = IndexOf(name);

			if (pos > Values.NotSet)
				return records[pos];
			else
				return null;
		}

		public bool Remove(string name)
		{
			int pos = IndexOf(name);

			if (pos == Values.NotSet)
				return false;

			--count;
			if (pos < count)
				records[pos] = records[count];

			AutoTrim();
			return true;
		}

		public bool Contains(string name)
		{
			return (IndexOf(name) > Values.NotSet);
		}

		private int IndexOf(string name)
		{
			PrepareName(ref name);

			for (int i = 0; i < count; i++)
			{
				if (records[i].Name == name)
					return i;
			}
			return Values.NotSet;
		}

		private void PrepareName(ref string name)
		{
			name = (name == null ? string.Empty : StringWork.Prepare(name));
		}

		private int AddPlace()
		{
			if (capacity == 0)
			{
				capacity = minCapacity;
				records = new Data[capacity];
			}
			else if (count == capacity)
			{
				capacity *= 2;
				Data[] copy = new Data[capacity];

				for (int i = 0; i < count; i++)
					copy[i] = records[i];

				records = copy;
			}

			return ++count;
		}

		private bool AutoTrim()
		{
			if (capacity <= minCapacity)
				return false;

			if (count == 0)
			{
				Clean();
				return true;
			}

			if (capacity > count * 3)
			{
				capacity = count * 2;
				Data[] copy = new Data[capacity];

				for (int i = 0; i < count; i++)
					copy[i] = records[i];

				records = copy;
				return true;
			}

			return false;
		}

		private void OnDataAdded(string name)
		{
			if (NewDataEvent != null)
			{
				PrepareName(ref name);
				NewDataEvent(name);
			}
		}

		public object Clone()
		{
			Meta copy = new Meta();

			copy.capacity = this.capacity;
			copy.count = this.count;
			copy.records = new Data[this.capacity];

			for (int i = 0; i < this.count; i++)
				copy.records[i] = (Data)this.records[i].Clone();

			return copy;
		}

		internal delegate void NewDataHandler(string name);
	}

}
