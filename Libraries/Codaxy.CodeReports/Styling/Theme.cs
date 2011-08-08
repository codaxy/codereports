using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Codaxy.CodeReports.Styling
{
    public class Theme
    {
        List<CellStyle> styles;

        public Theme()
        {
            styles = new List<CellStyle>();
        }

        public CellStyle GetCellStyle(CellStyleIndex style)
        {
            var index = (int)style;
            if (index < styles.Count)
                return styles[index];
            return null;           
        }        

        public void SetCellStyle(CellStyleIndex index, CellStyle value)
        {
            var ind = (int)index;
            while (styles.Count <= ind)
                styles.Add(null);
            styles[ind] = value;
        }

        public CellStyle this[CellStyleIndex index]
        {
            get { return GetCellStyle(index); }
            set { SetCellStyle(index, value); }
        }
       
        public CellStyle PageNumberStyle { get; set; }
        public CellAlignment PageNumberAlignment { get; set; }
    }

    public class Themes
    {
        static Theme def = CreateDefaultTheme();

        public static Theme Default { get { return def; } }

        private static Theme CreateDefaultTheme()
        {
            var res = new Theme
            {                   
                PageNumberAlignment = CellAlignment.Right
            };

            res[CellStyleIndex.Group3Caption] = new CellStyle
            {

                BorderStyle = new BorderStyle
                {
                    Bottom = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#99BBE8"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 2
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 13,
                    Bold = true
                },
                BackgroundColor = Color.White,
                CellPadding = 2
            };

            res[CellStyleIndex.Group3Footer] = res[CellStyleIndex.Group3FooterHeader] = new CellStyle
            {

                BorderStyle = new BorderStyle
                {
                    Top = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#C0C0C0"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 2
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 9,
                    Bold = true
                },
                BackgroundColor = ColorTranslator.FromHtml("#EEEEEE"),
                CellPadding = 2

            };

            res[CellStyleIndex.Group3FooterFooter] = new CellStyle
            {

                BorderStyle = new BorderStyle
                {
                    Top = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#C0C0C0"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 2
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 9,
                    Bold = true
                },
                BackgroundColor = ColorTranslator.FromHtml("#DFDFDF"),
                CellPadding = 2

            };

            res[CellStyleIndex.Group3Header] = res[CellStyleIndex.Group3HeaderHeader] = res[CellStyleIndex.Group3HeaderFooter] = new CellStyle
            {

                BorderStyle = new BorderStyle
                {
                    Bottom = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#99BBE8"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 9,
                    Bold = true
                },
                BackgroundColor = ColorTranslator.FromHtml("#E7EFFA"),
                CellPadding = 2

            };


            res[CellStyleIndex.Group2Caption] = new CellStyle
            {

                BorderStyle = new BorderStyle
                {
                    Bottom = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#99BBE8"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 2
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 11,
                    Bold = true
                },
                CellPadding = 2

            };

            res[CellStyleIndex.Group2Footer] = res[CellStyleIndex.Group2FooterHeader] = new CellStyle
            {

                BorderStyle = new BorderStyle
                {
                    Top = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#C0C0C0"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 8,
                    Bold = true
                },
                BackgroundColor = ColorTranslator.FromHtml("#EEEEEE"),
                CellPadding = 2

            };

            res[CellStyleIndex.Group2FooterFooter] = new CellStyle
            {
                BorderStyle = new BorderStyle
                {
                    Top = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#C0C0C0"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 8,
                    Bold = true
                },
                BackgroundColor = ColorTranslator.FromHtml("#DFDFDF"),
                CellPadding = 2
            };

            res[CellStyleIndex.Group2Header] = res[CellStyleIndex.Group2HeaderHeader] = res[CellStyleIndex.Group2HeaderFooter] = new CellStyle
            {
                BorderStyle = new BorderStyle
                {
                    Bottom = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#99BBE8"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 8,
                    Bold = true
                },
                BackgroundColor = ColorTranslator.FromHtml("#E7EFFA"),
                CellPadding = 2

            };

            res[CellStyleIndex.Group1Caption] = new CellStyle
            {
                BorderStyle = new BorderStyle
                {
                    Bottom = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#99BBE8"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 2
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 9,
                    Bold = true
                },
                CellPadding = 2

            };

            res[CellStyleIndex.Group1Footer] = res[CellStyleIndex.Group1FooterHeader] = new CellStyle
            {

                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 8
                },
                BackgroundColor = ColorTranslator.FromHtml("#EEEEEE"),
                CellPadding = 2

            };

            res[CellStyleIndex.Group1FooterFooter] = new CellStyle
            {

                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 8
                },
                BackgroundColor = ColorTranslator.FromHtml("#DFDFDF"),
                CellPadding = 2

            };

            res[CellStyleIndex.Group1Header] = res[CellStyleIndex.Group1HeaderHeader] = res[CellStyleIndex.Group1HeaderFooter] = new CellStyle
            {

                BorderStyle = new BorderStyle
                {
                    Bottom = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#99BBE8"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 8,
                    Bold = true
                },
                BackgroundColor = ColorTranslator.FromHtml("#E7EFFA"),
                CellPadding = 2
            };

            res[CellStyleIndex.TableRow] = res[CellStyleIndex.TableRowHeader] = new CellStyle
            {

                BorderStyle = new BorderStyle
                {
                    Bottom = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#EEEEEE"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 8
                },
                CellPadding = 2
            };

            res[CellStyleIndex.TableRowFooter] = new CellStyle
            {
                BorderStyle = new BorderStyle
                {
                    Bottom = new BorderEdgeStyle
                    {
                        Color = ColorTranslator.FromHtml("#E5E5E5"),
                        LineStyle = LineStyle.Solid,
                        LineWidth = 1
                    }
                },
                FontStyle = new FontStyle
                {
                    FontName = "Segoe UI",
                    FontSize = 8
                },
                BackgroundColor = ColorTranslator.FromHtml("#EEEEEE"),
                CellPadding = 2
            };

            res[CellStyleIndex.Body] = new CellStyle { CellPadding = 2, FontStyle = new FontStyle { FontName = "Segoe UI", FontSize = 8 } };
            res[CellStyleIndex.Note] = new CellStyle { CellPadding = 2, FontStyle = new FontStyle { FontName = "Segoe UI", FontSize = 7, Bold = true } };
            res[CellStyleIndex.H3] = new CellStyle { CellPadding = 2, FontStyle = new FontStyle { FontName = "Segoe UI", FontSize = 9, Bold = true } };
            res[CellStyleIndex.H2] = new CellStyle { CellPadding = 3, FontStyle = new FontStyle { FontName = "Segoe UI", FontSize = 11, Bold = true } };
            res[CellStyleIndex.H1] = new CellStyle { CellPadding = 3, FontStyle = new FontStyle { FontName = "Segoe UI", FontSize = 13, Bold = true } };
            res[CellStyleIndex.Title] = new CellStyle { CellPadding = 4, FontStyle = new FontStyle { FontName = "Segoe UI", FontSize = 15, Bold = true } };
            res[CellStyleIndex.Highlight] = new CellStyle { CellPadding = 2, FontStyle = new FontStyle { FontName = "Segoe UI", FontSize = 8, Bold = true } };

            res.PageNumberStyle = res[CellStyleIndex.Body];

            return res;
        }
    }
}
