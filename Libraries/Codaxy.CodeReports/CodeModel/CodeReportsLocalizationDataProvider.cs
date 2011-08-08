using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Codaxy.Common.Localization;
using Codaxy.CodeReports.Reflection;
using Codaxy.CodeReports.CodeModel;
using Codaxy.Common.Reflection;

namespace Codaxy.CodeReports.Localization
{
    public class CodeModelReportLocalizationDataProvider : ILocalizationDataProvider
    {
        public Dictionary<string, Field[]> ReadDefaultData(Assembly assembly)
        {
            var types = AssemblyHelper.GetAttributedTypesForAssembly(assembly, typeof(TableAttribute), false);

            Dictionary<String, Field[]> res = new Dictionary<string, Field[]>();

            foreach (var type in types)
            {
                List<Field> fields = new List<Field>();
                var rowGroupAttributes = AttributeHelper.GetCustomAttributes<GroupingLevelAttribute>(type, false);
                foreach (var rga in rowGroupAttributes)
                {
                    fields.Add(new Field { FieldName = "GroupingLevel-" + rga.Level + ":CaptionFormat", LocalizedText = rga.CaptionFormat });
                    fields.Add(new Field { FieldName = "GroupingLevel-" + rga.Level + ":FooterFormat", LocalizedText = rga.FooterFormat });
                }

                foreach (var p in type.GetProperties())
                {
                    var column = AttributeHelper.GetCustomAttribute<TableColumnAttribute>(p, false);
                    if (column != null)
                    {
                        fields.Add(new Field { FieldName = p.Name + ":HeaderText", LocalizedText = column.HeaderText });
                        fields.Add(new Field { FieldName = p.Name + ":Format", LocalizedText = column.Format });
                        fields.Add(new Field { FieldName = p.Name + ":FooterText", LocalizedText = column.FooterText });
                        fields.Add(new Field { FieldName = p.Name + ":FooterFormat", LocalizedText = column.FooterFormat });
                    }
                }

                if (fields.Count > 0)
                    res.Add(type.FullName, fields.ToArray());
            }
            return res;
        }
    }
}
