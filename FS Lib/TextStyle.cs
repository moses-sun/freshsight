/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

namespace ElasticLogic.FreshSight.Model
{

	public struct TextStyle
	{
		public bool Bold { get; set; }
		public bool Italic { get; set; }
		public bool Underlined { get; set; }
		public bool Striked { get; set; }

		public void Clear()
		{
			Bold = false;
			Italic = false;
			Underlined = false;
			Striked = false;
		}
	}

}
