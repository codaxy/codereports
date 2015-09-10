using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Codaxy.CodeReports.Reflection;

namespace Codaxy.CodeReports.Data
{
    public class TypeHelper
    {
        Type t;
        TypeMath m;
        public TypeHelper(Type type) { t = type; }
        public bool IsNumericType { get { return TypeInfo.IsNumericType(t); } }
        public TypeMath Math { get { return m ?? (m = TypeMath.Create(t)); } }
    }

    public interface IRow
    {
        object GetField(String col);        
    }

    public interface ITable
    {
        IEnumerable<IRow> GetRows();
        Type GetFieldType(String col);        
        String[] GetColumnNames();
    }

    public interface ILookupTable : ITable
    {
        IRow Lookup(object key);
    }

    public enum SortDirection { None = 0, Ascending = 1 , Descending = -1 }

    public class SortColumn
    {
        public int ColumnIndex { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    public class RowComparer : IComparer<Row>
    {
        Table t;
        SortColumn[] sort;        

        public RowComparer(Table table, params SortColumn[] sortColumns)
        {
            t = table;
            sort = sortColumns.ToArray();
        }

        public int Compare(Row x, Row y)
        {
            return Compare(x, y, sort, compareRowIndices: true);
        }

        public static int Compare(Row x, Row y, SortColumn[] sort, bool compareRowIndices = false)
        {
            for (int i = 0; i < sort.Length; i++)
            {
                var ci = sort[i].ColumnIndex;
                var xv = x[ci];
                var yv = y[ci];
                if (xv == null && yv == null)
                    continue;
                if (xv == null)
                    return -1 * (int)sort[i].SortDirection;
                if (yv == null)
                    return 1 * (int)sort[i].SortDirection;
                var xc = xv as IComparable;
                if (xc == null)
                    return 0;
                var res = xc.CompareTo(yv);
                if (res != 0)
                    return res * (int)sort[i].SortDirection;
            }

            if (compareRowIndices)
                return x.RowIndex - y.RowIndex;
            
            return 0;
        }
    }

    public class DataContext
    {
        Dictionary<String, ITable> table = new Dictionary<string,ITable>();
        public Dictionary<String, object> parameter = new Dictionary<string, object>();

        public void AddParameter(String name, object value) { parameter[name] = value; }
        public object GetParameterValue(String name) { object res; return parameter.TryGetValue(name, out res) ? res : null; }

        public ILookupTable GetLookup(String lookupName) { return table[lookupName] as ILookupTable; }
        public ITable GetTable(String tableName)
        {
            ITable res;
            if (table.TryGetValue(tableName, out res)) 
                return res;
            throw new Exception(String.Format("Table '{0}' was not found in DataContext!", tableName));
        }
        

        public void AddTable(String tableName, ITable t) { table[tableName] = t; }
        public void AddTable<T>(String tableName, IEnumerable<T> rows) where T:class
        {
            AddTable(tableName, TableHelper.CreateTable<T>(rows));
        }

        public Table CreateTable(String tableName, params String[] columns)
        {
            var dt = GetTable(tableName);
            Table rt = new Table();
            var rows = dt.GetRows();
            List<Row> res = new List<Row>();
            int rowIndex = 0;

            foreach (var r in rows)
            {
                Row rr = new Row(columns.Length)
                {
                    RowIndex = rowIndex++
                };
                int ci = 0;
                foreach (var c in columns)
                    rr[ci++] = r.GetField(c);
                res.Add(rr);       
            }
            rt.Rows = res.ToArray();
            rt.FieldName = columns.ToArray();
            rt.ColumnType = new Type[columns.Length];
            rt.TypeHelper = new TypeHelper[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                rt.ColumnType[i] = dt.GetFieldType(columns[i]);
                rt.TypeHelper[i] = new TypeHelper(rt.ColumnType[i]);
            }

            return rt;
        }
    }

    public class Row
    {
        object[] data;

        public int RowIndex { get; set; }

        public Row(int n) { data = new object[n]; }
        public object this[int i] { get { return i < 0 ? null : data[i]; } set { data[i] = value; } }

        public object Get(int i) { return this[i]; }
        public object[] GetMany(params int[] i) { return i.Select(a => this[a]).ToArray(); }
    }

    public class Table
    {
        public Row[] Rows { get; internal set; }
        public Type[] ColumnType { get; internal set; }
        public String[] FieldName { get; internal set; }
        public TypeHelper[] TypeHelper { get; internal set; }

        public void Sort(params SortColumn[] columns)
        {
            RowComparer rc = new RowComparer(this, columns);
            Array.Sort<Row>(Rows, rc);
        }

        /// <summary>
        /// -1 if not found
        /// </summary>        
        public int GetColumnIndex(string fieldName)
        {
            return Array.IndexOf(FieldName, fieldName);            
        }
    }


    public class RowAdapter<T> : IRow where T : class
    {
        Class rc;

        public T Row { get; set; }
        public RowAdapter(T r) { Row = r; rc = Class.Create(typeof(T)); }
        public object GetField(string col)
        {
            return rc.GetPropertyValue(Row, col);
        }        
    }

    public class TableAdapter<T> : ITable where T : class
    {
        IList<T> rows;
        public IList<T> Rows { get { return rows; } set { rows = value; OnRowsChanged(); } }

        Class helper;
        public TableAdapter() { helper = Class.Create(typeof(T)); }

        protected virtual void OnRowsChanged() {}

        #region ITable Members

        public IEnumerable<IRow> GetRows()
        {
            return Rows.Select(a => (IRow)new RowAdapter<T>(a));
        }

        public Type GetFieldType(string col)
        {
            return helper.GetPropertyType(col);
        }

        public TypeHelper GetFieldTypeHelper(string col)
        {
            return new TypeHelper(GetFieldType(col));
        }

        public String[] GetColumnNames() { return helper.GetPropertyNames(); }

        #endregion        
    }

    public class LookupTableAdapter<K, T> : TableAdapter<T>, ILookupTable where T : class
    {
        Dictionary<K, T> index;
        public delegate K Selector(T t);

        Selector keySelector;

        public LookupTableAdapter(Selector sel)
        {
            keySelector = sel;
        }

        void RebuildIndex()
        {
            index = new Dictionary<K, T>();
            foreach (var i in Rows)
                index[keySelector(i)] = i;
        }

        #region ILookup Members

        public IRow Lookup(object key)
        {
            K k = (K)key;
            T t;
            if (index.TryGetValue(k, out t))
                return new RowAdapter<T>(t);
            return null;
        }

        protected override void OnRowsChanged()
        {
            RebuildIndex();
        }

        #endregion
    }

    public class TableHelper
    {
        public static ILookupTable CreateLookupTable<K, T>(IEnumerable<T> rows, LookupTableAdapter<K, T>.Selector selector) where T : class
        {
            return new LookupTableAdapter<K, T>(selector) { Rows = rows.ToList() };
        }

        public static ITable CreateTable<T>(IEnumerable<T> rows) where T : class
        {
            return new TableAdapter<T>() { Rows = rows.ToList() };
        }
    }   
}
