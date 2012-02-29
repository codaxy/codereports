using System;
using System.Linq;
using System.Text;
using System.IO;
using Codaxy.Xlio;
using Codaxy.CodeReports;
using Codaxy.CodeReports.Styling;
using Codaxy.Xlio.IO;

namespace Codaxy.CodeReports.Exporters.Xlio
{
    public class XlsxReportWriter
    {
        public static void WriteToStream(Report report, Theme theme, Stream outputStream)
        {
            var ef = new Workbook();
            var ws = new Sheet("Report");
            ef.Sheets.AddSheet(ws);

            if (report.MergedCells != null)
            {
                foreach (var m in report.MergedCells)
                    ws[m.Row1, m.Col1, m.Row2, m.Col2].Merge();
            }

            if (report.Cells != null)
            {
                foreach (var cell in report.Cells)
                {
                    var eStyle = theme.GetCellStyle(cell.CellStyleIndex);
                    CellData c = null;
                    object v = cell.Value;
                    if (eStyle != null)
                    {
                        if (eStyle.BorderStyle != null)
                        {
                            c = ws.Cells[cell.Row, cell.Column];
                            if (eStyle.BorderStyle.Left != null)
                                c.Style.Border.Left = eStyle.BorderStyle.Left.ToBorderEdge();
                            if (eStyle.BorderStyle.Right != null)
                                c.Style.Border.Right = eStyle.BorderStyle.Right.ToBorderEdge();
                            if (eStyle.BorderStyle.Top != null)
                                c.Style.Border.Top = eStyle.BorderStyle.Top.ToBorderEdge();
                            if (eStyle.BorderStyle.Bottom != null)
                                c.Style.Border.Bottom = eStyle.BorderStyle.Bottom.ToBorderEdge();

                            if (c.IsMerged && c.MergedRange.Cell1.Col == cell.Column && c.MergedRange.Cell1.Row == cell.Row)
                            {
                                for (var col = c.MergedRange.Cell1.Col + 1; col <= c.MergedRange.Cell2.Col; col++)
                                {
                                    if (eStyle.BorderStyle.Top != null)
                                        ws.Cells[cell.Row, col].Style.Border.Top = c.Style.Border.Top;
                                    if (eStyle.BorderStyle.Bottom != null)
                                        ws.Cells[c.MergedRange.Cell2.Row, col].Style.Border.Bottom = c.Style.Border.Bottom;
                                }

                                for (var row = c.MergedRange.Cell1.Row + 1; row <= c.MergedRange.Cell2.Row; row++)
                                {
                                    if (eStyle.BorderStyle.Left != null)
                                        ws.Cells[row, cell.Column].Style.Border.Left = c.Style.Border.Left;
                                    if (eStyle.BorderStyle.Right != null)
                                        ws.Cells[row, c.MergedRange.Cell2.Col].Style.Border.Right = c.Style.Border.Right;
                                }
                            }
                        }

                        if (eStyle.FontStyle != null)
                        {
                            c = c ?? ws.Cells[cell.Row, cell.Column];
                            if (!String.IsNullOrEmpty(eStyle.FontStyle.FontName))
                                c.Style.Font.Name = eStyle.FontStyle.FontName;
                            if (eStyle.FontStyle.FontSize != 0)
                                c.Style.Font.Size = eStyle.FontStyle.FontSize;
                            if (eStyle.FontStyle.FontColor != null)
                                c.Style.Font.Color = eStyle.FontStyle.FontColor.ToColor();
                            c.Style.Font.Bold = eStyle.FontStyle.Bold;
                            c.Style.Font.Italic = eStyle.FontStyle.Italic;
                            if (eStyle.FontStyle.Underline)
                                c.Style.Font.Underline = FontUnderline.Single;
                        }

                        String numberFormat;
                        if (cell.Format != null)
                            if (GetNumberFormat(cell.Format, out numberFormat))
                                c.Style.Format = numberFormat;
                            else
                                v = cell.FormattedValue;

                        if (eStyle.BackgroundColor != null)
                            c.Style.Fill = new CellFill { Foreground = eStyle.BackgroundColor.ToColor(), Pattern = FillPattern.Solid };
                    }

                    ws.Cells[cell.Row, cell.Column].Style.Alignment.HAlign = GetAlignment(cell.Alignment);

                    ws.Cells[cell.Row, cell.Column].Value = v;
                }
            }

            //if (report.Columns.Count > 0)
            //{
            //    int colIndex = 0;
            //    foreach (var col in report.Columns)
            //    {
            //        if (col.Width.HasValue)
            //            ws.Columns[colIndex].Width = (int)(col.Width.Value * 256);
            //        colIndex++;
            //    }
            //}

            //if (report.Rows.Count > 0)
            //{
            //    int rowIndex = 0;
            //    foreach (var row in report.Rows)
            //    {
            //        if (row.Height.HasValue)
            //            ws.Rows[rowIndex].Height = (int)(row.Height.Value * 20);
            //        rowIndex++;
            //    }
            //}            

            ef.SaveToStream(outputStream, XlsxFileWriterOptions.AutoFit);
        }

        private static HorizontalAlignment GetAlignment(Codaxy.CodeReports.CellAlignment cellAlignment)
        {
            switch (cellAlignment)
            {
                case Codaxy.CodeReports.CellAlignment.Left:
                    return HorizontalAlignment.Left;
                case Codaxy.CodeReports.CellAlignment.Right:
                    return HorizontalAlignment.Right;
                case Codaxy.CodeReports.CellAlignment.Center:
                    return HorizontalAlignment.Center;
                default:
                    return HorizontalAlignment.Left;
            }
        }

        private static bool GetNumberFormat(string format, out string excelFormat)
        {
            if (format == null || format.Length <= 4 || !format.StartsWith("{0:") || !format.EndsWith("}"))
            {
                excelFormat = null;
                return false;
            }
            string f = format.Substring(3, format.Length - 4);
            if (f == "d")
            {
                excelFormat = "mm/dd/yyyy";
                return true;
            }
            if (f == "n")
            {
                excelFormat = "#,#0.00";
                return true;
            }
            excelFormat = null;
            return false;
        }
    }

    internal static class Extensions
    {
        public static BorderEdge ToBorderEdge(this BorderEdgeStyle style)
        {
            var res = new BorderEdge { Style = GetBorderStyle(style) };
            if (style.Color != null)
                res.Color = style.Color.ToColor();
            return res;
        }

        public static Codaxy.Xlio.Color ToColor(this Codaxy.CodeReports.Styling.Color c) {
			return new Codaxy.Xlio.Color(c.a, c.r, c.g, c.b);
        }

        private static Codaxy.Xlio.BorderStyle GetBorderStyle(BorderEdgeStyle style)
        {
            if (style.LineStyle == LineStyle.None)
                return Codaxy.Xlio.BorderStyle.None;

            if (style.LineWidth > 2)
                return Codaxy.Xlio.BorderStyle.Thick;
            if (style.LineWidth > 1)
                switch (style.LineStyle)
                {
                    case LineStyle.DashDot:
                        return Codaxy.Xlio.BorderStyle.MediumDashDot;
                    case LineStyle.Dashed:
                        return Codaxy.Xlio.BorderStyle.MediumDashed;
                    case LineStyle.Dotted:
                        return Codaxy.Xlio.BorderStyle.MediumDashDotDot;
                    default:
                        return Codaxy.Xlio.BorderStyle.Medium;
                }

            switch (style.LineStyle)
            {
                case LineStyle.DashDot: return Codaxy.Xlio.BorderStyle.DashDot;
                case LineStyle.Dashed: return Codaxy.Xlio.BorderStyle.Dashed;
                case LineStyle.Dotted: return Codaxy.Xlio.BorderStyle.Dotted;
                default: return Codaxy.Xlio.BorderStyle.Hair;
            }
        }
    }
}
