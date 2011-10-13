using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codaxy.CodeReports.Data;
using Codaxy.CodeReports.Controls;

namespace Codaxy.CodeReports.CodeModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableAttribute : System.Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TableColumnAttribute : System.Attribute
    {
        public String DataField { get; set; }
        public String HeaderText { get; set; }
        public String Format { get; set; }
        public String LookupField { get; set; }
        public String LookupDisplayField { get; set; }
        public String LookupTable { get; set; }
        public SortDirection SortDirection { get; set; }
        public int SortIndex { get; set; }
        public String FooterText { get; set; }
        public ColumnFooterType FooterType { get; set; }
        public AggregateFunction AggregateFunction { get; set; }
        public CellAlignment CellAlignment { get; set; }
        public CellDisplayMode CellDisplayMode { get; set; }
        public CellAlignment FooterAlignment { get; set; }
        public CellAlignment HeaderAlignment { get; set; }
        public int FooterColSpan { get; set; }
        public String FooterFormat { get; set; }
        public bool IsRowHeader { get; set; }
        public bool IsRowFooter { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GroupByAttribute : System.Attribute
    {
        public GroupByAttribute(int groupingLevel, int fieldOrder) { Level = groupingLevel; FieldOrder = fieldOrder; }
        public int Level { get; private set; }
        public int FieldOrder { get; private set; }
        public String DataField { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class GroupingLevelAttribute : System.Attribute
    {
        public GroupingLevelAttribute(int level) { Level = level; }
        public int Level { get; private set; }
        public bool ShowHeader { get; set; }
        public bool ShowFooter { get; set; }
        public bool ShowCaption { get; set; }
        public String CaptionFormat { get; set; }
        public String FooterFormat { get; set; }
    }

    
}
