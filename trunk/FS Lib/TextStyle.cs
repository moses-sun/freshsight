/*
		Copyright © Bogdan Kobets, 2010
		http://elasticlogic.com/
*/

namespace ElasticLogic.FreshSight.Model
{

	public struct TextStyle
	{
		private double fontScale;

		public bool Bold { get; set; }
		public bool Italic { get; set; }
		public bool Underlined { get; set; }
		public bool Striked { get; set; }
		public double FontScale
		{
			get { return fontScale; }
			set
			{
				if (value >= 0.1 && value <= 10)
					fontScale = value;
			}
		}

		public void Clear()
		{
			Bold = false;
			Italic = false;
			Underlined = false;
			Striked = false;
			FontScale = 1;
		}

		public void ToggleBold()
		{
			Bold = !Bold;
		}

		public void ToggleItalic()
		{
			Italic = !Italic;
		}

		public void ToggleUnderlined()
		{
			Underlined = !Underlined;
		}

		public void ToggleStriked()
		{
			Striked = !Striked;
		}

		public void ResetFont()
		{
			fontScale = 1;
		}
	}

}
