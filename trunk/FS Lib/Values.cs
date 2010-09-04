/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

namespace ElasticLogic.FreshSight.Model
{

	static class Values
	{
		public const int NotSet = -1;
	}

	static class StringWork
	{
		static public string Prepare(string src)
		{
			src = src.Replace('\t', ' ');
			src = src.Replace('\n', ' ');
			src = src.Replace('\r', ' ');
			src = src.TrimEnd(new[] { ' ' });
			return src;
		}
	}

}
