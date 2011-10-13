using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codaxy.CodeReports.Tests.Helpers
{
    class ReportUtil
    {
        public static ExpandableList<ExpandableList<Cell>> GetCellMatrix(Report rep)
        {
            var result = new ExpandableList<ExpandableList<Cell>>
            {
                NewElement = (i) =>
                {
                    return new ExpandableList<Cell>
                    {
                        NewElement = (j) => { return new Cell(); }
                    };
                }
            };
            
            foreach (var cell in rep.Cells)
                result[cell.Row][cell.Column] = cell;
            return result;
        }
    }
}
