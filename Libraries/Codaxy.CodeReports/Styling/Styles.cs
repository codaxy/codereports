using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Codaxy.CodeReports.Styling
{ 
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
        public Color? BackgroundColor { get; set; }
        public float CellPadding { get; set; }
    }
}
