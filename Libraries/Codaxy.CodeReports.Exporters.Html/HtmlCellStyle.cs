using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codaxy.CodeReports.Styling;

namespace Codaxy.CodeReports.Exporters.Html
{
    public class HtmlCellStyle
    {
        public String CssClass { get; set; }
        public String Style { get; set; }

		public static String BuildHtmlStyle(CellStyle style)
		{
			if (style == null)
				return null;
			StringBuilder sb = new StringBuilder();
			if (style.FontStyle != null)
			{
				if (style.FontStyle.FontColor != null)
					sb.Append("color:").Append(style.FontStyle.FontColor.ToHtml()).Append(";");			
			}
			return sb.ToString();
		}
    }

    public interface IHtmlReportTheme
    {
        HtmlCellStyle GetStyle(CellStyleIndex style);
    }

    public class DefaultHtmlReportTheme : IHtmlReportTheme
    {
        public HtmlCellStyle GetStyle(CellStyleIndex style)
        {
            switch (style)
            {
                case CellStyleIndex.Body: return null;                    
                
                case CellStyleIndex.Group1Caption: return new HtmlCellStyle { CssClass = "tg1-caption" };
                case CellStyleIndex.Group1Footer: return new HtmlCellStyle { CssClass = "tg1-f" };
                case CellStyleIndex.Group1FooterFooter: return new HtmlCellStyle { CssClass = "tg1-ff" };
                case CellStyleIndex.Group1FooterHeader: return new HtmlCellStyle { CssClass = "tg1-fh" };
                case CellStyleIndex.Group1Header: return new HtmlCellStyle { CssClass = "tg1-h" };
                case CellStyleIndex.Group1HeaderFooter: return new HtmlCellStyle { CssClass = "tg1-hf" };
                case CellStyleIndex.Group1HeaderHeader: return new HtmlCellStyle { CssClass = "tg1-hf" };

                case CellStyleIndex.Group2Caption: return new HtmlCellStyle { CssClass = "tg2-caption" };
                case CellStyleIndex.Group2Footer: return new HtmlCellStyle { CssClass = "tg2-f" };
                case CellStyleIndex.Group2FooterFooter: return new HtmlCellStyle { CssClass = "tg2-ff" };
                case CellStyleIndex.Group2FooterHeader: return new HtmlCellStyle { CssClass = "tg2-fh" };
                case CellStyleIndex.Group2Header: return new HtmlCellStyle { CssClass = "tg2-h" };
                case CellStyleIndex.Group2HeaderFooter: return new HtmlCellStyle { CssClass = "tg2-hf" };
                case CellStyleIndex.Group2HeaderHeader: return new HtmlCellStyle { CssClass = "tg2-hf" };

                case CellStyleIndex.Group3Caption: return new HtmlCellStyle { CssClass = "tg3-caption" };
                case CellStyleIndex.Group3Footer: return new HtmlCellStyle { CssClass = "tg3-f" };
                case CellStyleIndex.Group3FooterFooter: return new HtmlCellStyle { CssClass = "tg3-ff" };
                case CellStyleIndex.Group3FooterHeader: return new HtmlCellStyle { CssClass = "tg3-fh" };
                case CellStyleIndex.Group3Header: return new HtmlCellStyle { CssClass = "tg3-h" };
                case CellStyleIndex.Group3HeaderFooter: return new HtmlCellStyle { CssClass = "tg3-hf" };
                case CellStyleIndex.Group3HeaderHeader: return new HtmlCellStyle { CssClass = "tg3-hf" };

                case CellStyleIndex.H1: return new HtmlCellStyle { CssClass = "h1" };
                case CellStyleIndex.H2: return new HtmlCellStyle { CssClass = "h2" };
                case CellStyleIndex.H3: return new HtmlCellStyle { CssClass = "h3" };
                case CellStyleIndex.Highlight: return new HtmlCellStyle { CssClass = "highlight" };
                case CellStyleIndex.Note: return new HtmlCellStyle { CssClass = "note" };
                case CellStyleIndex.TableRow: return new HtmlCellStyle { CssClass = "r" };
                case CellStyleIndex.TableRowFooter: return new HtmlCellStyle { CssClass = "rf" };
                case CellStyleIndex.TableRowHeader: return new HtmlCellStyle { CssClass = "rh" };
                case CellStyleIndex.Title: return new HtmlCellStyle { CssClass = "title" };

                default: return null;                
            }
        }
    }
}
