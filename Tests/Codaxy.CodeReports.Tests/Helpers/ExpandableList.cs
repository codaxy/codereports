using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codaxy.CodeReports.Tests.Helpers
{
    class ExpandableList<T> : List<T>
    {
        public Func<int, T> NewElement { get; set; }

        public new T this[int index]
        {
            get
            {
                if (index >= Count)
                    Expand(index + 1);
                return base[index];
            }
            set
            {
                if (index >= Count)
                    Expand(index + 1);
                base[index] = value;
            }
        }

        private void Expand(int size)
        {
            while (Count < size)
                Add(NewElement != null ? NewElement(Count) : default(T));
        }
    }
}
