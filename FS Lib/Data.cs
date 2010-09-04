/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

using System;

namespace ElasticLogic.FreshSight.Model
{

	public class Data : ICloneable
	{
		private string name;
		private string value;
		private Type kind;

		public string Name
		{
			get { return name; }
		}

		public Type Kind
		{
			get { return kind; }
		}

		public string Text
		{
			get { return value; }
		}

		public long Integer
		{
			get { return long.Parse(value); }
		}

		public double Real
		{
			get { return double.Parse(value); }
		}

		public bool Boolean
		{
			get { return bool.Parse(value); }
		}

		public DateTime Date
		{
			get { return DateTime.Parse(value); }
		}

		public Data(string name, Type kind)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("Incorrect name");

			this.name = name;
			this.kind = kind;

			InitValue();
		}

		public Data(string name, object value, Type kind)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("Incorrect name");

			this.name = name;
			this.kind = kind;

			if (!SetValue(value))
				throw new ArgumentException("Incorrect value");
		}

		public Data(string name, object value)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("Incorrect name");
			else
				this.name = name;

			if (!TryParse(value))
				throw new ArgumentException("Incorrect value");
		}

		public bool SetValue(object val)
		{
			bool result = false;
			string txt;
			try
			{
				txt = val.ToString();
			}
			catch
			{
				return result;
			}

			if (txt == null)
				return result;

			switch (kind)
			{
				case Type.Text:
					result = true;
					break;
				case Type.Integer:
					long _int;
					result = long.TryParse(txt, out _int);
					break;
				case Type.Real:
					double _real;
					result = double.TryParse(txt, out _real);
					break;
				case Type.Boolean:
					bool _bool;
					result = bool.TryParse(txt, out _bool);
					break;
				case Type.Date:
					DateTime _date;
					result = DateTime.TryParse(txt, out _date);
					break;
				default:
					throw new NotImplementedException();
			}

			if (result)
				value = txt;
			return result;
		}

		private void InitValue()
		{
			switch (kind)
			{
				case Type.Text:
					value = string.Empty;
					break;
				case Type.Integer:
				case Type.Real:
					value = 0.ToString();
					break;
				case Type.Boolean:
					value = bool.FalseString;
					break;
				case Type.Date:
					value = DateTime.MinValue.ToString();
					break;
				default:
					throw new NotImplementedException();
			}
		}

		private bool TryParse(object val)
		{
			string txt;
			try
			{
				txt = val.ToString();
			}
			catch
			{
				return false;
			}

			if (txt == null)
				return false;
			else
				value = txt;

			long _int;
			if (long.TryParse(value, out _int))
			{
				kind = Type.Integer;
				return true;
			}

			double _real;
			if (double.TryParse(value, out _real))
			{
				kind = Type.Real;
				return true;
			}

			DateTime _date;
			if (DateTime.TryParse(value, out _date))
			{
				kind = Type.Date;
				return true;
			}

			bool _bool;
			if (bool.TryParse(value, out _bool))
			{
				kind = Type.Boolean;
				return true;
			}

			kind = Type.Text;
			return true;
		}

		public object Clone()
		{
			return new Data(name, value, kind);
		}

		public override string ToString()
		{
			return value;
		}

		public enum Type
		{
			Text,
			Integer,
			Real,
			Boolean,
			Date
		}
	}

}
