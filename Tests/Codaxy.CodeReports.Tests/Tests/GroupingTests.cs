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

namespace Codaxy.CodeReports.Tests.Tests
{
    [TestFixture]
    class GroupingTests
    {
        [GroupingLevel(0, ShowCaption=true, ShowFooter=true, CaptionFormat="{Revoked}")]
        [GroupingLevel(1, ShowCaption = true, ShowHeader = true, ShowFooter = true, CaptionFormat = "{BrokerageHouseCode}")]
        class Item
        {
            [GroupBy(1, 0)]
            [TableColumn]
            public String BrokerageHouseCode { get; set; }

            [GroupBy(0, 0)]
            [TableColumn]
            public bool Revoked { get;set;}

            [TableColumn(AggregateFunction=AggregateFunction.Sum, FooterType= ColumnFooterType.AggregateValue)]
            public decimal Amount { get; set; }
        }

        [Test(Active=false)]
        public void TwoLevelGrouping()
        {
            var dc = new DataContext();
            dc.AddTable("data", new[] { 
                new Item {  Amount = 10, BrokerageHouseCode = "ADBR", Revoked = false },
                new Item {  Amount = 20, BrokerageHouseCode = "ADBR", Revoked = true},
                new Item {  Amount = 30, BrokerageHouseCode = "MOBR", Revoked = true },
            });

            var flow = new Flow { Orientation = FlowOrientation.Vertical };
            flow.AddTable<Item>("data");

            var rep = Report.CreateReport(flow, dc);
            var cells = ReportUtil.GetCellMatrix(rep);

//False                            
//=================================
//ADBR                             
//=================================
//BrokerageHouseCode Revoked Amount
//------------------ ------- ------
//ADBR               False       10
//---------------------------------
//                               10
//---------------------------------
//                               10

//True                             
//=================================
//ADBR                             
//=================================
//BrokerageHouseCode Revoked Amount
//------------------ ------- ------
//ADBR               True        20
//---------------------------------
//                               20

//MOBR                             
//=================================
//BrokerageHouseCode Revoked Amount
//------------------ ------- ------
//MOBR               True        30
//---------------------------------
//                               30
//---------------------------------
//                               50

            //Debug.WriteLine(ReportUtil.RenderTextReport(rep));
            Assert.AreEqual(16, cells.Count);

            Assert.AreEqual("False", cells[0][0].Value);
            Assert.AreEqual("ADBR", cells[1][0].Value);
            Assert.AreEqual("BrokerageHouseCode", cells[2][0].Value);
            Assert.AreEqual(10m, cells[3][2].Value);
            Assert.AreEqual(10m, cells[4][2].Value);
            Assert.AreEqual(10m, cells[5][2].Value);
            Assert.AreEqual("True", cells[6][0].Value);
            Assert.AreEqual("ADBR", cells[7][0].Value);
            Assert.AreEqual("BrokerageHouseCode", cells[8][0].Value);
            Assert.AreEqual(20m, cells[9][2].Value);
            Assert.AreEqual(20m, cells[10][2].Value);
            Assert.AreEqual("MOBR", cells[11][0].Value);
            Assert.AreEqual("BrokerageHouseCode", cells[12][0].Value);
            Assert.AreEqual(30m, cells[13][2].Value);
            Assert.AreEqual(30m, cells[14][2].Value);
            Assert.AreEqual(50m, cells[15][2].Value);
        }
    }
}
