using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Codaxy.CodeReports.Controls;
using Codaxy.CodeReports.Data;

namespace Codaxy.CodeReports
{
    public class Rect
    {
        public int Col1 { get; set; }
        public int Col2 { get; set; }
        public int Row1 { get; set; }
        public int Row2 { get; set; }

        public int Width { get { return Col2 - Col1 + 1; } }
        public int Height { get { return Row2 - Row1 + 1; } }
    }

    public class Report
    {
        public List<ReportColumn> Columns { get; set; }
        public List<ReportRow> Rows { get; set; }
        public List<Rect> MergedCells { get; set; }
        public List<Cell> Cells { get; set; }        

        public Report()
        {
            Columns = new List<ReportColumn>();
            Rows = new List<ReportRow>();
            MergedCells = new List<Rect>();
            Cells = new List<Cell>();
        }

        public static Report CreateReport(Control root, DataContext data)
        {
            var report = new Report();
            root.Render(report, null, data);
            return report;
        }
    }

    public class ReportColumn
    {        
        public float? Width { get; set; }
    }

    public class ReportRow
    {
        public float? Height { get; set; }
    }

    public enum CellStyleIndex
    {
        Body,
        Note,
        Title,
        H1, H2, H3, 
        Highlight,
        TableRow,
        TableRowHeader,
        TableRowFooter,
        Group1HeaderHeader,
        Group1HeaderFooter,
        Group1Header,        
        Group1FooterHeader,
        Group1FooterFooter,
        Group1Footer,
        Group1Caption,
        Group2HeaderHeader,
        Group2HeaderFooter,
        Group2Header,
        Group2FooterHeader,
        Group2FooterFooter,
        Group2Footer,
        Group2Caption,
        Group3HeaderHeader,
        Group3HeaderFooter,
        Group3Header,
        Group3FooterHeader,
        Group3FooterFooter,
        Group3Footer,
        Group3Caption,
    }

    public enum CellAlignment { Inherit = 0, Auto, Left, Right, Center };

    public class Cell
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public object Value { get; set; }
        public String FormattedValue { get; set; }
        public String Format { get; set; }
        public CellStyleIndex CellStyleIndex { get; set; }
        public String CellStyleName { get; set; }
        public String Url { get; set; }
        public CellAlignment Alignment { get; set; }
    }
}
