using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportLib.Xml;
using CodeNova.CodeReports.Data;

namespace CodeNova.CodeReports.Controls
{
    public partial class ReportTemplate
    {
        public Flow Flow { get; set; }
        public List<ReportColumn> Columns { get; set; }

        public Report Render(DataContext dataContext)
        {
            Report rep = new Report()
            {
                Columns = Columns ?? new List<ReportColumn>()                
            };
            Flow.Render(rep, null, dataContext);            
            return rep;
        }        
    }
}
