using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.UI;
using Codaxy.CodeReports.Styling;

namespace Codaxy.CodeReports.Exporters.Html
{
    public class HtmlReportWriter
    {
        HtmlBuilder html;
        int row;
        bool tro;
        int col;
        Report report;
        HashSet<int> merged;
        Dictionary<int, int> rowSpan;
        Dictionary<int, int> colSpan;

        public HtmlReportWriter(HtmlTextWriter output)
        {
            html = new HtmlBuilder(output);
        }

        int CellIndex(int row, int col) { return row * 5000 + col; }

        public static String RenderReport(Report rep, IHtmlReportTheme theme) { return RenderReport(rep, theme, null); }

        public static String RenderReport(Report rep, IHtmlReportTheme theme, Dictionary<String, HtmlCellStyle> stylesheet)
        {
            using (var sw = new StringWriter())
            using (var hw = new HtmlTextWriter(sw))
            {
                var renderer = new HtmlReportWriter(hw);
                renderer.Render(rep, theme, stylesheet);
                return sw.ToString();
            }
        }

        public void Write(Report rep, IHtmlReportTheme theme) { Render(rep, theme, null); }

        public void Render(Report rep, IHtmlReportTheme theme, Dictionary<String, HtmlCellStyle> stylesheet)
        {
            report = rep;

            html.e("table").att("cellpadding", 0).att("cellspacing", 0);
            html.nl();
            html.e("tbody");
            html.nl();

            row = -1;
            tro = false;
            col = -1;

            List<HtmlCellStyle> styleCache = new List<HtmlCellStyle>();
            foreach (var v in Enum.GetValues(typeof(CellStyleIndex)))
            {
                styleCache.Add(theme != null ? theme.GetStyle((CellStyleIndex)v) : null);
            }

            merged = new HashSet<int>();
            colSpan = new Dictionary<int, int>();
            rowSpan = new Dictionary<int, int>();

            if (report.MergedCells!=null)
                foreach (var mc in report.MergedCells)
                {
                    for (int c = mc.Col1; c <= mc.Col2; c++)
                        for (int r = mc.Row1; r <= mc.Row2; r++)
                            merged.Add(CellIndex(r, c));
                    int ci = CellIndex(mc.Row1, mc.Col1);
                    merged.Remove(ci);
                    if (mc.Col2 > mc.Col1)
                        colSpan.Add(ci, mc.Col2 - mc.Col1 + 1);
                    if (mc.Row2 > mc.Row1)
                        rowSpan.Add(ci, mc.Row2 - mc.Row1 + 1);
                }
            if (rep.Cells!=null)
                foreach (var cell in report.Cells)
                {
                    if (cell.Row != row)
                    {
                        CompleteRow();
                        StartRow(cell.Row, cell.Column);
                    }
                    ExtendRow(cell.Column);
                    var ind = CellIndex(cell.Row, cell.Column);
                    if (!Merged(ind))
                    {
                        html.td();
                        CheckMergeOrigin(ind);
						HtmlCellStyle style = null;
						
						if (String.IsNullOrEmpty(cell.CellStyleName) || stylesheet == null || !stylesheet.TryGetValue(cell.CellStyleName, out style))
							style = styleCache[(int)cell.CellStyleIndex];

						String css = null;
						String styles = null;
                        if (style != null) {
							if (style.CssClass!=null)
								css = style.CssClass;
							if (style.Style!=null)
								styles = style.Style;
						}

						if (cell.CustomStyle != null)
							styles += (styles != null ? " " : "") + HtmlCellStyle.BuildHtmlStyle(cell.CustomStyle);

						html.attCls(css);
						html.attStyle(styles);

                        switch (cell.Alignment)
                        {
                            case CellAlignment.Left:
                                html.att("align", "left");
                                break;
                            case CellAlignment.Right:
                                html.att("align", "right");
                                break;
                            case CellAlignment.Center:
                                html.att("align", "center");
                                break;
                        }
                        if (String.IsNullOrEmpty(cell.FormattedValue))
                            html.nbsp();
                        else
                            html.text(cell.FormattedValue);
                        html.c(); //td                    
                    }
                }

            CompleteRow();
            html.c(); //tbody
            html.nl();
            html.c(); //table
            html.nl();
        }

        private void StartRow(int row, int col)
        {
            for (int r = this.row + 1; r < row; r++)
            {
                StartRow(r, -1);
                CompleteRow();
            }
            html.tr();
            tro = true;
            this.col = -1;
            this.row = row;
            ExtendRow(col);  
        }

        bool Merged(int ind)
        {
            return merged.Contains(ind);
        }

        void CheckMergeOrigin(int ind)
        {
            int v;
            if (colSpan.TryGetValue(ind, out v))
                html.att("colspan", v);

            if (rowSpan.TryGetValue(ind, out v))
                html.att("rowspan", v);
        }

        private void AddEmptyCell(int row, int col)
        {
            var ind = CellIndex(row, col);
            if (Merged(ind))
                return;
            html.td();
            CheckMergeOrigin(ind);
            html.nbsp();
            html.c(); //td            
        }

        void ExtendRow(int col)
        {
            for (int c = this.col + 1; c < col; c++)
                AddEmptyCell(row, c);
            this.col = col;
        }

        void CompleteRow()
        {
            if (tro)
            {
                ExtendRow(report.Columns.Count);                    
                html.c();
                html.nl();
                tro = false;                
            }
        }

        public void RegisterCss(string virtalPath)
        {
            html.HtmlTextWriter.WriteLine("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", virtalPath);
        }
    }
}
