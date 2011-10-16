using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codaxy.CodeReports.Styling
{
	public class Color
	{
		public byte r;
		public byte g;
		public byte b;
		public byte a;

		public static Color FromHtml(String c)
		{			
			if (String.IsNullOrEmpty(c))
				return new Color { a = 0, r = 0, g = 0, b = 0 };
			var skip = c.StartsWith("#") ? 1 : 0;
			var step = c.Length - skip > 3 ? 2 : 1;

			var length = skip + 3 * step;
			while (c.Length < length)
				c += "0";

			return new Color
			{
				a = 255,
				r = byte.Parse(c.Substring(skip, step), System.Globalization.NumberStyles.HexNumber),
				g = byte.Parse(c.Substring(skip + step, step), System.Globalization.NumberStyles.HexNumber),
				b = byte.Parse(c.Substring(skip + 2 * step, step), System.Globalization.NumberStyles.HexNumber),
			};
		}

		public String ToHtml()
		{
			return "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
		}

		public static Color White { get { return new Color { a = 255, b = 255, g = 255, r = 255 }; } }
	}

    public class FontStyle
    {
        public String FontName { get; set; }
        public int FontSize { get; set; }
        public Color FontColor { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
    }

    public enum LineStyle
    {
        None,
        Solid,
        Dashed,
        Dotted,
        DashDot
    }

    public class BorderEdgeStyle
    {
        public Color Color { get; set; }
        public LineStyle LineStyle { get; set; }
        public float LineWidth { get; set; }
    }

    public class BorderStyle
    {
        public BorderEdgeStyle Left { get; set; }
        public BorderEdgeStyle Right { get; set; }
        public BorderEdgeStyle Top { get; set; }
        public BorderEdgeStyle Bottom { get; set; }
    }  

    public class CellStyle
    {
        public BorderStyle BorderStyle { get; set; }
        public FontStyle FontStyle { get; set; }
        public Color BackgroundColor { get; set; }
        public float CellPadding { get; set; }
    }
}
