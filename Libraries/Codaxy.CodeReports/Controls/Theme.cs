using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GemBox.Spreadsheet;

namespace CodeNova.CodeReports.Controls
{
    public class Theme
    {
        public TableStyle TableStyle { get; set; }
        public LabelStyle LabelStyle { get; set; }
        public CellStyle PageNumberStyle { get; set; }
        public CellAlignment PageNumberAlignment { get; set; }
    }

    public class LabelStyle
    {
        public CellStyle Title { get; set; }
        public CellStyle H1 { get; set; }
        public CellStyle H2 { get; set; }
        public CellStyle H3 { get; set; }
        public CellStyle Body { get; set; }
        public CellStyle Note { get; set; }
        public CellStyle Highlight { get; set; }
    }

    public class RowStyle
    {
        public CellStyle RowHeader { get; set; }
        public CellStyle RowFooter { get; set; }
        public CellStyle RowCell { get; set; }
    }

    public class TableGroupStyle
    {
        public CellStyle Caption { get; set; }
        public RowStyle Header { get; set; }
        public RowStyle Footer { get; set; }
    }

    public class TableStyle
    {
        public TableGroupStyle Group1 { get; set; }
        public TableGroupStyle Group2 { get; set; }
        public TableGroupStyle Group3 { get; set; }
        public RowStyle RowStyle { get; set; }
    }

    public class Themes
    {
        public static Theme def = CreateDefaultTheme();

        public static Theme Default { get { return def; } }

        private static Theme CreateDefaultTheme()
        {
            TableGroupStyle g3 = new TableGroupStyle
            {
                Header = new RowStyle(),
                Footer = new RowStyle()
            };

            g3.Caption = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg3-caption" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Bottom = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#99BBE8"),
                            LineStyle = LineStyle.Medium
                        }
                    },
                    FontStyle = new ExcelFontStyle 
                    { 
                        FontName = "Segoe UI",
                        FontSize = 13,
                        Bold = true
                    },
                    BackgroundColor = Color.White,
                    CellPadding = 2
                }
            };

            g3.Footer.RowCell = g3.Footer.RowHeader = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg3-f" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Top = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#C0C0C0"),
                            LineStyle = LineStyle.Medium
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 9,
                        Bold = true
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#EEEEEE"),
                    CellPadding = 2
                }
            };

            g3.Footer.RowFooter = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg3-ff" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Top = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#C0C0C0"),
                            LineStyle = LineStyle.Medium
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 9,
                        Bold = true
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#DFDFDF"),
                    CellPadding = 2
                }
            };

            g3.Header.RowCell = g3.Header.RowFooter = g3.Header.RowHeader = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg3-h" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Bottom = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#99BBE8"),
                            LineStyle = LineStyle.Thin
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 9,
                        Bold = true
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#E7EFFA"),
                    CellPadding = 2
                }
            };

            TableGroupStyle g2 = new TableGroupStyle 
            {
                 Header = new RowStyle(), 
                 Footer = new RowStyle() 
            };

            g2.Caption = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg2-caption" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Bottom = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#99BBE8"),
                            LineStyle = LineStyle.Medium
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 11,
                        Bold = true
                    },
                    CellPadding = 2                   
                }
            };

            g2.Footer.RowCell = g2.Footer.RowHeader= new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg2-f" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Top = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#C0C0C0"),
                            LineStyle = LineStyle.Thin
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 8,
                        Bold = true
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#EEEEEE"),
                    CellPadding = 2
                }
            };

            g2.Footer.RowFooter = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg2-ff" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Top = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#C0C0C0"),
                            LineStyle = LineStyle.Thin
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 8,
                        Bold = true
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#DFDFDF"),
                    CellPadding = 2
                }
            };

            g2.Header.RowCell = g2.Header.RowFooter = g2.Header.RowHeader = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg2-h" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Bottom = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#99BBE8"),
                            LineStyle = LineStyle.Thin
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 8,
                        Bold = true
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#E7EFFA"),
                    CellPadding = 2
                }
            };


            TableGroupStyle g1 = new TableGroupStyle
            {
                Header = new RowStyle(),
                Footer = new RowStyle()
            };

            g1.Caption = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg1-caption" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Bottom = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#99BBE8"),
                            LineStyle = LineStyle.Medium
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 9,
                        Bold = true
                    },
                    CellPadding = 2
                }
            };

            g1.Footer.RowCell = g1.Footer.RowHeader = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg1-f" },
                ExcelCellStyle = new ExcelCellStyle
                {                    
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 8
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#EEEEEE"),
                    CellPadding = 2
                }
            };

            g1.Footer.RowFooter = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg1-ff" },
                ExcelCellStyle = new ExcelCellStyle
                {                    
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 8
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#DFDFDF"),
                    CellPadding = 2
                }
            };

            g1.Header.RowCell = g1.Header.RowFooter = g1.Header.RowHeader = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg1-h" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Bottom = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#99BBE8"),
                            LineStyle = LineStyle.Thin
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 8,
                        Bold = true
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#E7EFFA"),
                    CellPadding = 2
                }
            };

            
            var rowStyle = new RowStyle();

            rowStyle.RowHeader = rowStyle.RowCell = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg1-r" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Bottom = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#EEEEEE"),
                            LineStyle = LineStyle.Thin
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 8
                    },
                    CellPadding = 2                
                }
            };

            rowStyle.RowFooter = new CellStyle
            {
                HtmlCellStyle = new HtmlCellStyle { CssClass = "tg1-rf" },
                ExcelCellStyle = new ExcelCellStyle
                {
                    BorderStyle = new BorderStyle
                    {
                        Bottom = new BorderEdgeStyle
                        {
                            Color = ColorTranslator.FromHtml("#E5E5E5"),
                            LineStyle = LineStyle.Thin
                        }
                    },
                    FontStyle = new ExcelFontStyle
                    {
                        FontName = "Segoe UI",
                        FontSize = 8
                    },
                    BackgroundColor = ColorTranslator.FromHtml("#EEEEEE"),
                    CellPadding = 2
                }
            };

            TableStyle ts = new TableStyle
            {
                Group1 = g1,
                Group2 = g2,
                Group3 = g3, 
                RowStyle = rowStyle
            };
            var bodyStyle = new CellStyle { };

            LabelStyle labelStyle = new LabelStyle
            {
                Body = new CellStyle { HtmlCellStyle = null, ExcelCellStyle = new ExcelCellStyle { CellPadding = 2, FontStyle = new ExcelFontStyle { FontName = "Segoe UI", FontSize = 8 } } },
                Note = new CellStyle { HtmlCellStyle = new HtmlCellStyle { CssClass = "lab-note" }, ExcelCellStyle = new ExcelCellStyle { CellPadding = 2, FontStyle = new ExcelFontStyle { FontName = "Segoe UI", FontSize = 7, Bold = true } } },
                H3 = new CellStyle { HtmlCellStyle = new HtmlCellStyle { CssClass = "lab-h3" }, ExcelCellStyle = new ExcelCellStyle { CellPadding = 2, FontStyle = new ExcelFontStyle { FontName = "Segoe UI", FontSize = 9, Bold = true } } },
                H2 = new CellStyle { HtmlCellStyle = new HtmlCellStyle { CssClass = "lab-h2" }, ExcelCellStyle = new ExcelCellStyle { CellPadding = 3, FontStyle = new ExcelFontStyle { FontName = "Segoe UI", FontSize = 11, Bold = true } } },
                H1 = new CellStyle { HtmlCellStyle = new HtmlCellStyle { CssClass = "lab-h1" }, ExcelCellStyle = new ExcelCellStyle { CellPadding = 3, FontStyle = new ExcelFontStyle { FontName = "Segoe UI", FontSize = 13, Bold = true } } },
                Title = new CellStyle { HtmlCellStyle = new HtmlCellStyle { CssClass = "lab-title" }, ExcelCellStyle = new ExcelCellStyle { CellPadding = 4, FontStyle = new ExcelFontStyle { FontName = "Segoe UI", FontSize = 15, Bold = true } } },
                Highlight = new CellStyle { HtmlCellStyle = new HtmlCellStyle { CssClass = "lab-highlight" }, ExcelCellStyle = new ExcelCellStyle { CellPadding = 2, FontStyle = new ExcelFontStyle { FontName = "Segoe UI", FontSize = 8, Bold = true } } }
            };

            return new Theme
            {
                TableStyle = ts,
                LabelStyle = labelStyle,
                PageNumberStyle = labelStyle.Body,
                PageNumberAlignment = CellAlignment.Right
            };
        }
    }
}
