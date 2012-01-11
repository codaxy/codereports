using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codaxy.CodeReports.Controls;
using Codaxy.Common.Localization;
using Codaxy.Common.Reflection;

namespace Codaxy.CodeReports.CodeModel
{
    public class TableGenerator
    {
        class ColLoc
        {
            public String Property { get; set; }
            public String LocalizedText { get; set; }
        }

        public static Table GetTable(Type rowType, String tableName)
        {
            return GetTable(rowType, tableName, null);
        }

        static Dictionary<String, List<ColLoc>> GetTableLocalization(Type rowType, ILocalizationStore localizationStore)
        {
            var fields = localizationStore == null ? null : localizationStore.GetTypeLocalizationData(rowType);
            Dictionary<String, List<ColLoc>> loc = null;
            if (fields != null)
            {
                loc = new Dictionary<string, List<ColLoc>>();
                foreach (var field in fields)
                {
                    var parts = field.FieldName.Split(':');
                    if (parts.Length == 2)
                    {
                        List<ColLoc> props;
                        if (!loc.TryGetValue(parts[0], out props))
                            loc[parts[0]] = props = new List<ColLoc>();
                        props.Add(new ColLoc
                        {
                            Property = parts[1],
                            LocalizedText = field.LocalizedText
                        });
                    }
                }
            }
            return loc;
        }

        public static Table GetTable(Type rowType, String tableName, ILocalizationStore localizationStore)
        {
            List<TableColumn> columns = new List<TableColumn>();
            List<GroupingLevel> groups = new List<GroupingLevel>();
            List<List<GroupByAttribute>> groupByAttributes = new List<List<GroupByAttribute>>();

            var loc = GetTableLocalization(rowType, localizationStore);

            var gas = AttributeHelper.GetCustomAttributes<GroupingLevelAttribute>(rowType, false);
            foreach (var ga in gas.OrderBy(a => a.Level))
            {
                if (ga.Level != groups.Count)
                    throw new InvalidOperationException(String.Format("Invalid group level: {0}. Expected: {1}", ga.Level, groups.Count));
                var g = new GroupingLevel
                {
                    CaptionFormat = ga.CaptionFormat,
                    FooterFormat = ga.FooterFormat,
                    ShowCaption = ga.ShowCaption,
                    ShowFooter = ga.ShowFooter,
                    ShowHeader = ga.ShowHeader
                };

                List<ColLoc> gloc;
                if (loc != null && loc.TryGetValue("GroupingLevel-" + ga.Level, out gloc))
                {
                    foreach (var gl in gloc)
                    {
                        switch (gl.Property)
                        {
                            case "CaptionFormat": g.CaptionFormat = gl.LocalizedText; break;
                            case "FooterFormat": g.FooterFormat = gl.LocalizedText; break;                                
                        }
                    }
                }

                groups.Add(g);
                groupByAttributes.Add(new List<GroupByAttribute>());
            }

            foreach (var p in rowType.GetProperties())
            {
                var tca = AttributeHelper.GetCustomAttribute<TableColumnAttribute>(p, false);
                if (tca != null)
                {
                    var cal = tca.CellAlignment == CellAlignment.Inherit ? CellAlignment.Auto : tca.CellAlignment;
                    var c = new TableColumn
                    {
                        AggregateFunction = tca.AggregateFunction,
                        AggregateWeightDataField = tca.AggregateWeightDataField,
                        CellAlignment = cal,
                        DataField = tca.DataField ?? p.Name,
                        FooterAlignment = tca.FooterAlignment == CellAlignment.Inherit ? cal : tca.FooterAlignment,
                        FooterColSpan = tca.FooterColSpan,
                        FooterFormat = tca.FooterFormat ?? tca.Format,
                        FooterText = tca.FooterText,
                        FooterType = tca.FooterType,
                        Format = tca.Format,
                        HeaderAlignment = tca.HeaderAlignment == CellAlignment.Inherit ? cal : tca.HeaderAlignment,
                        HeaderText = tca.HeaderText ?? p.Name,
                        ColumnType = tca.IsRowFooter ? TableColumnType.FooterColumn : tca.IsRowHeader ? TableColumnType.HeaderColumn : TableColumnType.Normal,                        
                        LookupDisplayField = tca.LookupDisplayField,
                        LookupField = tca.LookupField,
                        LookupTable = tca.LookupTable,
                        SortDirection = tca.SortDirection,
                        SortIndex = tca.SortIndex, 
                        CellDisplayMode = tca.CellDisplayMode
                    };

                    List<ColLoc> colLoc;
                    if (loc != null && loc.TryGetValue(c.DataField, out colLoc))
                        foreach (var cl in colLoc)
                        {
                            switch (cl.Property)
                            {
                                case "HeaderText": c.HeaderText = cl.LocalizedText; break;
                                case "Format": c.Format = cl.LocalizedText; break;
                                case "FooterText": c.FooterText = cl.LocalizedText; break;
                                case "FooterFormat": c.FooterFormat = cl.LocalizedText; break;
                            }
                        }
                    columns.Add(c);
                }

                var gcas = AttributeHelper.GetCustomAttributes<GroupByAttribute>(p, false);
                foreach (var gca in gcas)
                {
                    if (gca.Level < 0 || gca.Level >= groups.Count)
                        throw new InvalidOperationException(String.Format("Invalid GroupBy level on column '{0}'.", p.Name));
                    gca.DataField = gca.DataField ?? p.Name;
                    groupByAttributes[gca.Level].Add(gca);
                }
            }

            for (var level = 0; level < groups.Count; level++)
                groups[level].GroupByColumns.AddRange(groupByAttributes[level].OrderBy(a => a.FieldOrder).Select(gca => new GroupByColumn
                {
                    DataField = gca.DataField,
                    SortDirection = gca.SortDirection
                }));

            if (groups.Count == 0)
                groups.Add(new GroupingLevel
                {
                    ShowHeader = true
                });

            return new Table
            {
                DataTable = tableName,
                Columns = columns.ToArray(),
                Groups = groups.ToArray()
            };
        }
    }
}
