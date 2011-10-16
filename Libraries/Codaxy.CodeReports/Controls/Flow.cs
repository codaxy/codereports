using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codaxy.CodeReports.CodeModel;
using Codaxy.Common.Localization;

namespace Codaxy.CodeReports.Controls
{
    public enum FlowOrientation { Horizontal, Vertical }

    public class Flow : Control
    {
        List<Control> children;
        protected Rect bounds;
        public List<Control> Children { get { return children; } }
        
        public FlowOrientation Orientation { get; set; }

        public Flow()
        {
            Position = RowCol.Zero;
            children = new List<Control>();
        }

        public Flow Horizontal(int height)
        {
            var flow = new Flow { Orientation = FlowOrientation.Horizontal, Height = height };
            return Add(flow);
        }
        
        public Flow Vertical(int width)
        {
            var flow = new Flow { Orientation = FlowOrientation.Horizontal, Width = width };
            return Add(flow);
        }

        public T Add<T>(T child) where T : Control
        {
            if (child.Position == null)
                child.Position = RowCol.Zero;
            children.Add(child);
            return child;
        }

        public void AddMany(params Control[] controls)
        {
            foreach (var c in controls)
                Add(c);
        }

        public Table AddTable<T>(string tableName)
        {
            var table = TableGenerator.GetTable(typeof(T), tableName);
            Add(table);
			return table;
        }

		public Table AddTable<T>(string tableName, ILocalizationStore localizationStore)
        {
            var table = TableGenerator.GetTable(typeof(T), tableName, localizationStore);
            Add(table);
			return table;
        }

        public Rect GetRect(RowCol pos, int w, int h)
        {
            if (pos == null)
                throw new ArgumentNullException("Flow.GetRect: Argument 'pos' is null!");

            Rect res = new Rect();
            if (pos.Col < 0)
            {
                res.Col1 = bounds.Col2 + pos.Col - w + 1;
                res.Col2 = res.Col1 + w -1;
                if (Orientation == FlowOrientation.Horizontal)
                    bounds.Col2 = res.Col1 - 1;
                else
                    if (res.Col1 > bounds.Col1)
                        bounds.Col1 = res.Col1;
            }
            else
            {
                res.Col1 = bounds.Col1 + pos.Col;
                res.Col2 = res.Col1 + w - 1;
                if (Orientation == FlowOrientation.Horizontal)
                    bounds.Col1 = res.Col2 + 1;
                else
                    if (res.Col2 > bounds.Col2)
                        bounds.Col2 = res.Col2;
            }

            if (pos.Row < 0)
            {
                res.Row1 = bounds.Row2 + pos.Row - h + 1;
                res.Row2 = res.Row1 + h - 1;
                if (Orientation == FlowOrientation.Vertical)
                    bounds.Row2 = res.Row1 - 1;
                else
                    if (res.Row1 > bounds.Row1)
                        bounds.Row1 = res.Row1;
            }
            else
            {
                res.Row1 = bounds.Row1 + pos.Row;
                res.Row2 = res.Row1 + h - 1;
                if (Orientation == FlowOrientation.Vertical)
                    bounds.Row1 = res.Row2 + 1;
                else
                    if (res.Row2 > bounds.Row2)
                        bounds.Row2 = res.Row2;
            }
            return res;
        }

        public override void Render(Report report, Flow flow, Data.DataContext dataContext)
        {
            if (flow != null)
                bounds = flow.GetRect(Position, Width ?? 0, Height ?? 0);
            else
                bounds = new Rect { Col1 = 0, Row1 = 0, Col2 = Width ?? 0, Row2 = Height ?? 0 };

            foreach (var c in Children)
            {
                c.Render(report, this, dataContext);
            }

            if (flow != null)
            {
                if (!Width.HasValue)
                    flow.GetRect(RowCol.Zero, bounds.Width, 0);

                if (!Height.HasValue)
                    flow.GetRect(RowCol.Zero, 0, bounds.Height);
            }
        }
    }
}
