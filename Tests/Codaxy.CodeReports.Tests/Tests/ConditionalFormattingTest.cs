using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;
using Codaxy.CodeReports.CodeModel;
using Codaxy.CodeReports.Data;
using Codaxy.CodeReports.Controls;
using Codaxy.CodeReports.Tests.Helpers;
using Codaxy.CodeReports.Exporters.Html;

namespace Codaxy.CodeReports.Tests.Tests
{
	[TestFixture]
	public class ConditionalFormattingTests
	{
		[GroupingLevel(0, ShowHeader=false, ShowFooter=false, ShowCaption=false)]
        class Item
        {
            [TableColumn()]
            public String Col1 { get; set; }

            [TableColumn()]
            public int Col2 { get; set; }
        }

		[Test]
		public void Test1()
		{
			var dc = new DataContext();
			dc.AddTable("data", new[] { 
                new Item { Col1="A", Col2 = 2 }
            });

			var flow = new Flow { Orientation = FlowOrientation.Vertical };
			var table = flow.AddTable<Item>("data");
			table.Columns.Single(a => a.DataField == "Col2").ConditionalFormatting = (value) => { return new Styling.CellStyle(); };

			var rep = Report.CreateReport(flow, dc);
			var cells = ReportUtil.GetCellMatrix(rep);

			Assert.IsNull(cells[0][0].CustomStyle);
			Assert.IsNotNull(cells[0][1].CustomStyle);
		}

		[Test]
		public void Test2()
		{
			var dc = new DataContext();
			dc.AddTable("data", new[] { 
                new Item { Col1="A", Col2 = 2 }
            });

			var flow = new Flow { Orientation = FlowOrientation.Vertical };
			var table = flow.AddTable<Item>("data");
			table.Columns.Single(a => a.DataField == "Col2").ConditionalFormatting = (value) => {
				if (!(value is int))
					return null;
				var v = (int)value;
				if (v > 0)
					return new Styling.CellStyle
					{
						FontStyle = new Styling.FontStyle
						{
							FontColor = Styling.Color.FromHtml("#00FF00")
						}
					};
				return null;
			};

			var rep = Report.CreateReport(flow, dc);
			var cells = ReportUtil.GetCellMatrix(rep);

			Assert.IsNull(cells[0][0].CustomStyle);
			Assert.IsNotNull(cells[0][1].CustomStyle);

			var html = HtmlReportWriter.RenderReport(rep, new DefaultHtmlReportTheme());
			Assert.IsTrue(html.Contains("style=\"color:"));
			Assert.IsTrue(html.Contains("#00FF00"));
		}
	}
}
