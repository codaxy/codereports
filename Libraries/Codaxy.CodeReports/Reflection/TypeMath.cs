using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;

namespace Codaxy.CodeReports.Reflection
{
    public class TypeMath
    {
        public delegate object BinOp(object a, object b);

        public BinOp Min { get; set; }
        public BinOp Max { get; set; }
        public BinOp Multiply { get; set; }
        public BinOp Sum { get; set; }
        public BinOp MultiplyNullAsOne { get; set; }
        public BinOp SumNullAsZero { get; set; }
        public object Zero { get; set; }
        public object One { get; set; }

        private TypeMath(Type valueType)
        {
            if (!TypeInfo.IsNumericType(valueType))
                throw new Exception("Numeric type is required for math!");

            Min = TypeMathExpressionHelper.GetMinDelegate<BinOp>(valueType, typeof(object));
            Max = TypeMathExpressionHelper.GetMaxDelegate<BinOp>(valueType, typeof(object));

            Sum = TypeMathExpressionHelper.GetSumDelegate<BinOp>(valueType, typeof(object));
            Multiply = TypeMathExpressionHelper.GetMultiplyDelegate<BinOp>(valueType, typeof(object));            

            if (TypeInfo.IsNullableType(valueType))
            {
                var nc = new NullableConverter(valueType);
                Zero = nc.ConvertFrom(Convert.ChangeType(0, Nullable.GetUnderlyingType(valueType)));
                One = nc.ConvertFrom(Convert.ChangeType(1, Nullable.GetUnderlyingType(valueType)));
                SumNullAsZero = TypeMathExpressionHelper.GetSumIsNullDelegate<BinOp>(valueType, typeof(object), Zero);
                MultiplyNullAsOne = TypeMathExpressionHelper.GetMultiplyIsNullDelegate<BinOp>(valueType, typeof(object), One);
            }
            else
            {
                Zero = Convert.ChangeType(0, valueType);
                One = Convert.ChangeType(1, valueType);
                SumNullAsZero = Sum;
                MultiplyNullAsOne = Multiply;
            }            
        }

        static Dictionary<Type, TypeMath> cache = new Dictionary<Type, TypeMath>();

        public static TypeMath Create(Type valueType)
        {
            TypeMath m;
            if (cache.TryGetValue(valueType, out m))
                return m;
            return cache[valueType] = new TypeMath(valueType);
        }
    }
}
