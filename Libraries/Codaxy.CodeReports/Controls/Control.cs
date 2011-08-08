using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codaxy.CodeReports.Data;

namespace Codaxy.CodeReports.Controls
{
    public class RowCol
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public static RowCol Zero { get { return new RowCol { Row = 0, Col = 0 }; } }
    }

    public class Control
    {
        public RowCol Position { get; set; }

        public int? Width { get; set; }
        public int? Height { get; set; }

        public virtual void Render(Report report, Flow flow, DataContext dataContext) { }
    }
}
