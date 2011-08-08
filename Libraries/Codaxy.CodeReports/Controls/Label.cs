using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codaxy.CodeReports.Data;
using Codaxy.Common.Text;

namespace Codaxy.CodeReports.Controls
{    

    public class Label : Control
    {
        public String Text { get; set; }
        public String Format { get; set; }
        public CellAlignment Alignment { get; set; }
        public CellStyleIndex CellStyle { get; set; }

        public override void Render(Report report, Flow fm, DataContext dataContext)
        {
            Rect rect = fm.GetRect(Position, Width ?? 1, Height ?? 1);
            String value = Format == null ? Text : GetFormattedValue(dataContext);
            report.Cells.Add(new Cell
            {
                Alignment = Alignment,
                Column = rect.Col1,
                Row = rect.Row1,
                Value = value,
                FormattedValue = value, 
                CellStyleIndex = CellStyle
            });

            if (rect.Width > 1 || rect.Height > 1)
                report.MergedCells.Add(rect);
        }

        private string GetFormattedValue(DataContext dataContext)
        {
            if (Format == null)
                return null;
            String f;
            String[] pn;
            StringFormatHelper.PrepareFormatWithNames(Format, out f, out pn);
            if (pn == null)
                return f;
            object[] pv = pn.Select(a => dataContext.GetParameterValue(a)).ToArray();
            return String.Format(f, pv);
        }
    }
}
