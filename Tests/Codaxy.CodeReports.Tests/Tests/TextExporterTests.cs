using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;
using Codaxy.CodeReports.CodeModel;
using Codaxy.CodeReports.Data;
using Codaxy.CodeReports.Controls;
using Codaxy.CodeReports.Exporters.Text;
using System.IO;

namespace Codaxy.CodeReports.Tests
{
    [TestFixture]
    class TextExporterTests
    {
        [GroupingLevel(0, ShowHeader=true, ShowFooter=true, ShowCaption=true, CaptionFormat="Caption")]
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
            flow.AddTable<Item>("data");

            var rep = Report.CreateReport(flow, dc);
            StringWriter sw = new StringWriter();
            TextReportWriter.WriteTo(rep, sw);

            var lines = sw.ToString().Split('\n').Select(a=>a.TrimEnd('\r')).ToArray();
            
            Assert.AreEqual(lines[1], "=========");
            Assert.AreEqual(lines[2], "Col1 Col2");
            Assert.AreEqual(lines[3], "---- ----");
            Assert.AreEqual(lines[4], "A       2");
            Assert.AreEqual(lines[5], "---------");
        }
    }
}
