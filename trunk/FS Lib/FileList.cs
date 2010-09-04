/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;
using System.Collections;

namespace ElasticLogic.FreshSight.Model
{

	public class FileList : ICloneable, IEnumerable
	{
		private const int minCapacity = 4;

		private int[] files;
		private int count;
		private int capacity;

		public int Count
		{
			get { return count; }
		}

		public FileList()
		{
			Clean();
		}

		public void Clean()
		{
			files = null;
			capacity = 0;
			count = 0;
		}

		public bool Add(int file)
		{
			if (Contains(file))
				return false;

			if (capacity == 0)
			{
				capacity = minCapacity;
				files = new int[capacity];
			}
			else if (count == capacity)
			{
				capacity *= 2;
				int[] copy = new int[capacity];

				files.CopyTo(copy, 0);
				files = copy;
			}

			files[count++] = file;
			return true;
		}

		public bool Remove(int file)
		{
			int pos = IndexOf(file);

			if (pos == Values.NotSet)
				return false;

			--count;
			if (pos < count)
				files[pos] = files[count];

			AutoTrim();
			return true;
		}

		public bool Contains(int file)
		{
			return (IndexOf(file) > Values.NotSet);
		}

		private int IndexOf(int file)
		{
			for (int i = 0; i < count; i++)
			{
				if (files[i] == file)
					return i;
			}
			return Values.NotSet;
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
				int[] copy = new int[capacity];

				for (int i = 0; i < count; i++)
					copy[i] = files[i];

				files = copy;
				return true;
			}

			return false;
		}

		public object Clone()
		{
			FileList copy = new FileList();

			copy.files = (int[])this.files.Clone();
			copy.capacity = this.capacity;
			copy.count = this.count;

			return copy;
		}
		
		public IEnumerator GetEnumerator()
		{
			for (int i = 0; i < count; i++)
			{
				yield return files[i];
			}
		}
	}

}
