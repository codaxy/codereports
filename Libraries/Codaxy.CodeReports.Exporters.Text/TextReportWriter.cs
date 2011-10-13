using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Codaxy.CodeReports.Exporters.Text
{
    public class TextReportWriter
    {
        class Cell
        {
            public String Text { get; set; }
            public Rect Merge { get; set; }
            public CellAlignment Alignment { get; set; }

            public CellStyleIndex Style { get; set; }
        }

        public static void WriteTo(Report report, TextWriter tw)
        {
            ExpandableList<int> colWidth = new ExpandableList<int>();
            ExpandableList<ExpandableList<Cell>> data = new ExpandableList<ExpandableList<Cell>>()
            {
                NewElement = (index) => { return new ExpandableList<Cell>() { NewElement = (i) => { return new Cell(); } }; }
            };

            if (report.MergedCells != null)
            {
                foreach (var mcr in report.MergedCells)
                    for (var c = mcr.Col1; c <= mcr.Col2; c++)
                        for (var r = mcr.Row1; r <= mcr.Row2; r++)
                            data[r][c].Merge = mcr;
            }

            foreach (var cell in report.Cells)
            {
                var c = data[cell.Row][cell.Column];                
                c.Text = cell.FormattedValue;
                c.Alignment = cell.Alignment;
                c.Style = cell.CellStyleIndex;
                if (cell.FormattedValue != null && c.Merge == null)
                    if (cell.FormattedValue.Length + 1 > colWidth[cell.Column])
                        colWidth[cell.Column] = cell.FormattedValue.Length + 1; 
            }

            var footerStyles = new[] {
                CellStyleIndex.Group1Footer,
                CellStyleIndex.Group2Footer,
                CellStyleIndex.Group3Footer,
                CellStyleIndex.Group1FooterHeader,
                CellStyleIndex.Group2FooterHeader,
                CellStyleIndex.Group3FooterHeader,
                CellStyleIndex.Group1FooterFooter,
                CellStyleIndex.Group2FooterFooter,
                CellStyleIndex.Group3FooterFooter,
            };

            var previousLineWasFooter = false;

            for (var r = 0; r < data.Count; r++)
            {
                bool header = data[r].Any(cell => cell.Style == CellStyleIndex.Group1Header || cell.Style == CellStyleIndex.Group2Header || cell.Style == CellStyleIndex.Group3Header);
                bool caption = data[r].Any(cell => cell.Style == CellStyleIndex.Group1Caption || cell.Style == CellStyleIndex.Group2Caption || cell.Style == CellStyleIndex.Group3Caption);
                bool footer = data[r].Any(cell => footerStyles.Contains(cell.Style));

                if (previousLineWasFooter && !footer) //skip one line after footer
                    tw.WriteLine();

                if (footer)
                {
                    for (var fc = 0; fc < colWidth.Count; fc++)
                        for (var i = 0; i < colWidth[fc]; i++)
                            tw.Write("-");
                }

                for (var c = 0; c < colWidth.Count; c++)
                {
                    var cell = data[r][c];
                    if (cell.Text != null)
                    {
                        int padLeft = 0;
                        int padRight = 0;
                        switch (cell.Alignment)
                        {
                            default:
                                padRight = colWidth[c] - cell.Text.Length;
                                break;
                            case CellAlignment.Right:
                                padLeft = colWidth[c] - cell.Text.Length;
                                break;
                            case CellAlignment.Center:
                                padLeft = (colWidth[c] - cell.Text.Length) / 2;
                                padRight = colWidth[c] - cell.Text.Length - padLeft;
                                break;
                        }

                        while (padLeft-- > 0)
                            tw.Write(" ");
                        tw.Write(cell.Text);
                        while (padRight-- > 0)
                            tw.Write(" ");
                    }
                    else
                    {
                        for (var i = 0; i < colWidth[c]; i++)
                            tw.Write(" ");
                    }
                }
                tw.WriteLine();
                if (caption)
                {
                    for (var c = 0; c < colWidth.Count; c++)
                        for (var i = 0; i < colWidth[c]; i++)
                            tw.Write("=");
                    tw.WriteLine();
                }
                else if (header)
                {
                    for (var c = 0; c < colWidth.Count; c++)
                        for (var i = 0; i < colWidth[c]; i++)
                            tw.Write("-");
                    tw.WriteLine();
                }
                
                previousLineWasFooter = footer;
            }
        }
    }

    class ExpandableList<T> : List<T>
    {
        public Func<int, T> NewElement { get; set; }

        public new T this[int index]
        {
            get
            {
                if (index >= Count)
                    Expand(index + 1);
                return base[index];
            }
            set
            {
                if (index >= Count)
                    Expand(index + 1);
                base[index] = value;
            }
        }

        private void Expand(int size)
        {
            while (Count < size)
                Add(NewElement != null ? NewElement(Count) : default(T));
        }
    }
}
