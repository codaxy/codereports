using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Codaxy.CodeReports.Data;
using Codaxy.Common.Text;

namespace Codaxy.CodeReports.Controls
{
    public enum AggregateFunction { None, Sum, Count, Avg, Product, Min, Max }
    public enum ColumnFooterType { FooterText, AggregateValue, GroupFooter }
    public enum TableColumnType { Normal, HeaderColumn, FooterColumn };

    public class TableColumn
    {
        public String DataField { get; set; }
        public String HeaderText { get; set; }
        public String Format { get; set; }
        public String LookupField { get; set; }
        public String LookupDisplayField { get; set; }
        public String LookupTable { get; set; }
        public SortDirection SortDirection { get; set; }
        public int SortIndex { get; set; }
        public String FooterText { get; set; }
        public ColumnFooterType FooterType { get; set; }
        public AggregateFunction AggregateFunction { get; set; }
        public CellAlignment CellAlignment { get; set; }
        public CellAlignment FooterAlignment { get; set;}
        public CellAlignment HeaderAlignment { get; set; }
        public int FooterColSpan { get; set; }
        public String FooterFormat { get; set; }
        public TableColumnType ColumnType { get; set; }
        
        internal int _Index { get; set; }
        internal int _DataFieldIndex { get; set; }
    }

    public class GroupByColumn
    {
        public String DataField { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    public class GroupingLevel
    {
        public GroupingLevel()
        {
            GroupByColumns = new List<GroupByColumn>();
        }
        public List<GroupByColumn> GroupByColumns { get; set; }
        public bool ShowHeader { get; set; }
        public bool ShowFooter { get; set; }
        public bool ShowCaption { get; set; }
        public String CaptionFormat { get; set; }
        public String FooterFormat { get; set; }
    }    
    
    public class Table : Control
    {
        public String DataTable { get; set; }
        public TableColumn[] Columns { get; set; }
        public GroupingLevel[] Groups { get; set; }
        
        class GroupData
        {
            public GroupingLevel Group { get; set; }
            public SortColumn[] Columns { get; set; }

            public object[] GroupAccumulator { get; set; }
            public int[] GroupCounter { get; set; }

            public String PreparedCaptionFormat { get; set; }
            public int[] PreparedCaptionColumns { get; set; }

            public String PreparedFooterFormat { get; set; }
            public int[] PreparedFooterColumns { get; set; }
            
            public Boolean GroupOpen { get; set; }

            public int GroupIndex { get; set; }
            

            public CellStyleIndex GetHeaderCellStyle(TableColumnType cellType) {
                switch (cellType)
                {
                    case TableColumnType.HeaderColumn:
                        switch (GroupLevel)
                        {
                            case 0: return CellStyleIndex.Group1HeaderHeader;
                            case 1: return CellStyleIndex.Group2HeaderHeader;
                            default:
                            case 2: return CellStyleIndex.Group3HeaderHeader;
                        }
                    case TableColumnType.FooterColumn:
                        switch (GroupLevel)
                        {
                            case 0: return CellStyleIndex.Group1HeaderFooter;
                            case 1: return CellStyleIndex.Group2HeaderFooter;
                            default:
                            case 2: return CellStyleIndex.Group3HeaderFooter;
                        }
                    case TableColumnType.Normal:
                        switch (GroupLevel)
                        {
                            case 0: return CellStyleIndex.Group1Header;
                            case 1: return CellStyleIndex.Group2Header;
                            default:
                            case 2: return CellStyleIndex.Group3Header;
                        }
                    default:
                        throw new InvalidOperationException();
                }
            }

            public CellStyleIndex GetFooterCellStyle(TableColumnType cellType)
            {
                switch (cellType)
                {
                    case TableColumnType.HeaderColumn:
                        switch (GroupLevel)
                        {
                            case 0: return CellStyleIndex.Group1FooterHeader;
                            case 1: return CellStyleIndex.Group2FooterHeader;
                            default:
                            case 2: return CellStyleIndex.Group3FooterHeader;
                        }
                    case TableColumnType.FooterColumn:
                        switch (GroupLevel)
                        {
                            case 0: return CellStyleIndex.Group1FooterFooter;
                            case 1: return CellStyleIndex.Group2FooterFooter;
                            default:
                            case 2: return CellStyleIndex.Group3FooterFooter;
                        }
                    case TableColumnType.Normal:
                        switch (GroupLevel)
                        {
                            case 0: return CellStyleIndex.Group1Footer;
                            case 1: return CellStyleIndex.Group2Footer;
                            default:
                            case 2: return CellStyleIndex.Group3Footer;
                        }
                    default:
                        throw new InvalidOperationException();
                }
            }



            public int GroupLevel { get; set; }

            public CellStyleIndex GetCaptionStyle()
            {
                switch (GroupLevel)
                {
                    case 0: return CellStyleIndex.Group1Caption;
                    case 1: return CellStyleIndex.Group2Caption;
                    default:
                    case 2: return CellStyleIndex.Group3Caption;
                }
            }
        }
        
        public override void Render(Report report, Flow fm, DataContext dataContext)
        {
            HashSet<String> fieldNames = new HashSet<string>();
            
            foreach (var c in Columns)
                if (c.DataField != "#")
                    fieldNames.Add(c.DataField);
            
            foreach (var g in Groups)
                foreach (var sc in g.GroupByColumns)
                    fieldNames.Add(sc.DataField);

            var data = dataContext.CreateTable(DataTable, fieldNames.ToArray());

            for (int i = 0; i < Columns.Length; i++)
            {
                Columns[i]._Index = i;
                Columns[i]._DataFieldIndex = data.GetColumnIndex(Columns[i].DataField);
            }            

            List<SortColumn> sort = new List<SortColumn>();
            List<GroupData> groupData = new List<GroupData>();

            int gi = 0;
            if (Groups != null)
            { 
                foreach (var g in Groups)
                {
                    var gd = new GroupData();
                    var gc = new List<SortColumn>();     
                    if (g.GroupByColumns!=null)
                        foreach (var c in g.GroupByColumns)
                        {
                            var ci = data.GetColumnIndex(c.DataField);
                            if (ci != -1)
                                gc.Add(new SortColumn
                                {
                                    ColumnIndex = ci,
                                    SortDirection = c.SortDirection == SortDirection.None ? SortDirection.Ascending : c.SortDirection
                                });
                        }
                    gd.Columns = gc.ToArray();
                    sort.AddRange(gd.Columns);
                    gd.GroupIndex = gi++;
                    gd.Group = g;

                    if (g.CaptionFormat != null)
                    {
                        String[] names;
                        String format;
                        StringFormatHelper.PrepareFormatWithNames(g.CaptionFormat, out format, out names);

                        gd.PreparedCaptionFormat = format;
                        gd.PreparedCaptionColumns = names != null ? names.Select(a => data.GetColumnIndex(a)).ToArray() : null;
                    }

                    if (g.FooterFormat != null)
                    {
                        String[] names;
                        String format;
                        StringFormatHelper.PrepareFormatWithNames(g.FooterFormat, out format, out names);

                        gd.PreparedFooterFormat = format;
                        gd.PreparedFooterColumns = names!=null ? names.Select(a => data.GetColumnIndex(a)).ToArray() : null;
                    }

                    gd.GroupLevel = Groups.Length - gd.GroupIndex - 1;
                    gd.GroupOpen = false;
                    groupData.Add(gd);
                }
            }

            sort.AddRange(from c in Columns
                          where c.SortDirection != SortDirection.None
                          orderby c.SortIndex
                          select new SortColumn
                          {
                              ColumnIndex = c._DataFieldIndex,
                              SortDirection = c.SortDirection
                          });
            
            if (sort.Count != 0)
                data.Sort(sort.ToArray());
            
            var rows = data.Rows;
            var rect = fm.GetRect(Position, Columns.Length, 0);
            var pos = new RowCol { Col = rect.Col1, Row = rect.Row1 };
            var startRow = pos.Row;
            List<Cell> cells = new List<Cell>();            
            

            object[] accumulator = new object[Columns.Length];
            int[] count = new int[Columns.Length];
            CellAlignment[] align = new CellAlignment[Columns.Length];

            for (int c = 0; c < Columns.Length; c++)
            {
                var th = Columns[c]._DataFieldIndex >= 0 ? data.TypeHelper[Columns[c]._DataFieldIndex] : null;
                if (th != null)
                    switch (Columns[c].AggregateFunction)
                    {
                        case AggregateFunction.Sum:
                            accumulator[c] = th.Math.Zero;
                            break;
                        case AggregateFunction.Count:
                            accumulator[c] = 0;
                            break;
                        case AggregateFunction.Avg:
                            accumulator[c] = th.Math.Zero;
                            count[c] = 0;
                            break;
                        case AggregateFunction.Product:
                            accumulator[c] = th.Math.One;
                            break;
                    }
                align[c] = th != null ? CalcAlignment(Columns[c].CellAlignment, th) : CellAlignment.Right;
            }
            

            Data.Row prevRow = null;
            for (int r = 0; r <= rows.Length; r++)
            {
                var row = r < rows.Length ? rows[r] : null;

                for (int g = 0; g < groupData.Count; g++)
                {
                    var gd = groupData[groupData.Count - 1 - g];
                    if (gd.GroupOpen)
                    {
                        //prevRow is not null because group is open
                        if (row == null || RowComparer.Compare(row, prevRow, gd.Columns) != 0)
                        {
                            gd.GroupOpen = false;
                            //close group
                            if (gd.Group.ShowFooter)
                            {
                                for (int c = 0; c < Columns.Length; c++)
                                {
                                    if (Columns[c].AggregateFunction == AggregateFunction.Avg && gd.GroupCounter[c] > 0 && gd.GroupAccumulator[c] != null)
                                    {
                                        gd.GroupAccumulator[c] = Convert.ToDecimal(gd.GroupAccumulator[c]) / gd.GroupCounter[c];
                                    }
                                    var style = gd.GetFooterCellStyle(Columns[c].ColumnType);
                                    switch (Columns[c].FooterType)
                                    {
                                        case ColumnFooterType.FooterText:                                            
                                            cells.Add(new Cell { Column = pos.Col + c, Row = pos.Row, FormattedValue = Columns[c].FooterText, Value = Columns[c].FooterText, CellStyleIndex = style, Alignment = Columns[c].FooterAlignment });
                                            break;
                                        case ColumnFooterType.AggregateValue:
                                            String fv = (Columns[c].FooterFormat != null) ? String.Format(Columns[c].FooterFormat, gd.GroupAccumulator[c]) : (gd.GroupAccumulator[c] != null ? gd.GroupAccumulator[c].ToString() : null);
                                            var al = Columns[c]._DataFieldIndex >= 0 ? CalcAlignment(Columns[c].FooterAlignment, data.TypeHelper[Columns[c]._DataFieldIndex]) : CellAlignment.Auto;
                                            cells.Add(new Cell { Column = pos.Col + c, Row = pos.Row, FormattedValue = fv, Value = gd.GroupAccumulator[c], CellStyleIndex = style, Alignment = al, Format = Columns[c].FooterFormat });
                                            break;
                                        case ColumnFooterType.GroupFooter:
                                            String gfv = gd.PreparedFooterColumns == null ? gd.PreparedFooterFormat : String.Format(gd.PreparedFooterFormat, prevRow.GetMany(gd.PreparedFooterColumns));
                                            var gal = Columns[c]._DataFieldIndex >= 0 ? CalcAlignment(Columns[c].FooterAlignment, data.TypeHelper[Columns[c]._DataFieldIndex]) : CellAlignment.Auto;
                                            cells.Add(new Cell { Column = pos.Col + c, Row = pos.Row, FormattedValue = gfv, Value = gfv, CellStyleIndex = style, Alignment = gal });
                                            break;
                                    }
                                    if (Columns[c].FooterColSpan > 1)
                                    {
                                        report.MergedCells.Add(new Rect { Col1 = pos.Col + c, Col2 = pos.Col + c + Columns[c].FooterColSpan - 1, Row1 = pos.Row, Row2 = pos.Row });
                                        c += Columns[c].FooterColSpan - 1;
                                    }
                                }
                                pos.Row++;
                            }
                        }
                    }
                }

                for (int g = 0; g < groupData.Count; g++) {
                    var gd = groupData[g];
                    //add row
                    if (row != null)
                    {
                        if (!gd.GroupOpen)
                        {
                            gd.GroupOpen = true;

                            if (gd.Group.ShowCaption)
                            {
                                String caption = gd.PreparedCaptionColumns == null ? gd.PreparedCaptionFormat : String.Format(gd.PreparedCaptionFormat, row.GetMany(gd.PreparedCaptionColumns));

                                cells.Add(new Cell { Column = pos.Col, Row = pos.Row, FormattedValue = caption, Value = caption, CellStyleIndex = gd.GetCaptionStyle()});
                                report.MergedCells.Add(new Rect { Col1 = pos.Col, Col2 = pos.Col + Columns.Length - 1, Row1 = pos.Row, Row2 = pos.Row });
                                pos.Row++;
                            }

                            if (gd.Group.ShowHeader)
                            {
                                for (int c = 0; c < Columns.Length; c++)
                                {
                                    var ht = Columns[c].HeaderText;
                                    var style = gd.GetHeaderCellStyle(Columns[c].ColumnType);
                                    var a = Columns[c]._DataFieldIndex >= 0 ? CalcAlignment(Columns[c].HeaderAlignment, data.TypeHelper[Columns[c]._DataFieldIndex]) : CellAlignment.Right;

                                    cells.Add(new Cell
                                    {
                                        Column = pos.Col + c,
                                        Row = pos.Row,
                                        FormattedValue = ht,
                                        Value = ht,
                                        CellStyleIndex = style,
                                        Alignment = a
                                    });
                                }
                                pos.Row++;
                            }

                            gd.GroupAccumulator = new object[Columns.Length];
                            gd.GroupCounter = new int[Columns.Length];

                            //reset group accumulator
                            for (int c = 0; c < Columns.Length; c++)
                            {
                                var th = Columns[c]._DataFieldIndex >= 0 ? data.TypeHelper[Columns[c]._DataFieldIndex] : null;
                                if (th != null)
                                    switch (Columns[c].AggregateFunction)
                                    {
                                        case AggregateFunction.Sum:
                                            gd.GroupAccumulator[c] = th.Math.Zero;
                                            break;
                                        case AggregateFunction.Count:
                                            gd.GroupAccumulator[c] = 0;
                                            break;
                                        case AggregateFunction.Avg:
                                            gd.GroupAccumulator[c] = th.Math.Zero;
                                            gd.GroupCounter[c] = 0;
                                            break;
                                        case AggregateFunction.Product:
                                            gd.GroupAccumulator[c] = th.Math.One;
                                            break;
                                    }
                            }
                        }

                        for (int c = 0; c < Columns.Length; c++)
                        {
                            var dfi = Columns[c]._DataFieldIndex;
                            var v = Columns[c].DataField == "#" ? r + 1 : row[dfi];
                            String fv = (Columns[c].Format != null) ? String.Format(Columns[c].Format, v) : (v != null ? v.ToString() : null);

                            if (g + 1 == groupData.Count)
                                cells.Add(new Cell
                                {
                                    Column = pos.Col + c,
                                    Row = pos.Row,
                                    Value = v,
                                    FormattedValue = fv,
                                    CellStyleIndex = Columns[c].ColumnType == TableColumnType.HeaderColumn ? CellStyleIndex.TableRowHeader : Columns[c].ColumnType == TableColumnType.FooterColumn ? CellStyleIndex.TableRowFooter : CellStyleIndex.TableRow,
                                    Alignment = align[c],
                                    Format = Columns[c].Format
                                });

                            var th = dfi >= 0 ? data.TypeHelper[dfi] : null;

                            if (th != null)
                            {
                                switch (Columns[c].AggregateFunction)
                                {
                                    case AggregateFunction.Sum:
                                        gd.GroupAccumulator[c] = th.Math.SumNullAsZero(gd.GroupAccumulator[c], v);
                                        break;
                                    case AggregateFunction.Count:
                                        if (v != null)
                                            gd.GroupAccumulator[c] = (int)gd.GroupAccumulator[c] + 1;
                                        break;
                                    case AggregateFunction.Product:
                                        gd.GroupAccumulator[c] = th.Math.Multiply(gd.GroupAccumulator[c], v);
                                        break;
                                    case AggregateFunction.Avg:
                                        if (v != null)
                                        {
                                            gd.GroupAccumulator[c] = th.Math.Sum(gd.GroupAccumulator[c], v);
                                            ++gd.GroupCounter[c];
                                        }
                                        break;
                                    case AggregateFunction.Min:
                                        if (gd.GroupAccumulator[c] == null)
                                            gd.GroupAccumulator[c] = v;
                                        else if (v != null)
                                            gd.GroupAccumulator[c] = th.Math.Min(gd.GroupAccumulator[c], v);
                                        break;
                                    case AggregateFunction.Max:
                                        if (gd.GroupAccumulator[c] == null)
                                            gd.GroupAccumulator[c] = v;
                                        else if (v != null)
                                            gd.GroupAccumulator[c] = th.Math.Max(gd.GroupAccumulator[c], v);
                                        break;
                                }
                            }
                        }

                        if (g + 1 == groupData.Count)
                            pos.Row++;
                    }
                }

                prevRow = row;
            }

            fm.GetRect(RowCol.Zero, 0, pos.Row - startRow);

            report.Cells.AddRange(cells);
        }

        CellAlignment CalcAlignment(CellAlignment a, TypeHelper th)
        {
            if (th!=null && a == CellAlignment.Auto)
                if (th.IsNumericType)
                    return CellAlignment.Right;
            return a;
        }
    }
}
