using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;
using Codaxy.CodeReports.CodeModel;
using Codaxy.CodeReports.Data;
using Codaxy.CodeReports.Controls;
using Codaxy.CodeReports.Tests.Helpers;
using System.Diagnostics;

namespace Codaxy.CodeReports.Tests
{
    [TestFixture]
    class WeightedAverageTests
    {
        [GroupingLevel(0, ShowHeader = false, ShowFooter = true, ShowCaption = false)]
        [GroupingLevel(1, ShowHeader = false, ShowFooter = true, ShowCaption = false)]
        class Item2
        {
            [TableColumn(CellDisplayMode = CellDisplayMode.RowNumber)]
            public string No { get; set; }

            [TableColumn(SortIndex = 0, SortDirection = SortDirection.Ascending)]
            public String Name { get; set; }

            public decimal Weight { get; set; }

            [TableColumn(AggregateFunction = AggregateFunction.Avg, FooterType=ColumnFooterType.AggregateValue, AggregateWeightDataField="Weight")]
            public decimal Value { get; set; }

            [GroupBy(1, 0)]
            public String G { get; set; }
        }

        [Test(Active=true)]
        public void TestFooters()
        {
            var dc = new DataContext();
            dc.AddTable("data", new[] { 
                new Item2 { Name="1", Value = 2, G="1", Weight = 3 },
                new Item2 { Name="2", Value = 3, G="1", Weight = 1 },
                new Item2 { Name="3", Value = 4, G="2", Weight = 4 }
            });

            var flow = new Flow { Orientation = FlowOrientation.Vertical };
            flow.AddTable<Item2>("data");

            var rep = Report.CreateReport(flow, dc);
            var cells = ReportUtil.GetCellMatrix(rep);

            Debug.WriteLine(ReportUtil.RenderTextReport(rep));

            Assert.AreEqual(6, cells.Count);                        

            //Footers
            Assert.AreEqual(9m/4, cells[2][2].Value);
            Assert.AreEqual(16m/4, cells[4][2].Value);
            Assert.AreEqual(25m/8, cells[5][2].Value);
        }
    }
}
