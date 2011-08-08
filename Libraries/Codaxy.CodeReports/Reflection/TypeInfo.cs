using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codaxy.CodeReports;

namespace Codaxy.CodeReports.Reflection
{
    public class TypeInfo
    {
        public static bool IsNullableType(Type type)
        {
            return Codaxy.Common.Nullable.IsNullableType(type);
        }

        static Type[] numericTypes = new[] { typeof(int), typeof(decimal), typeof(float), typeof(uint), typeof(long), typeof(ulong), typeof(double), typeof(short), typeof(ushort) };

        public static bool IsNumericType(Type type)
        {
            if (IsNullableType(type))
                return IsNumericType(Codaxy.Common.Nullable.GetUnderlyingType(type));
            return numericTypes.Contains(type);
        }
    }
}
