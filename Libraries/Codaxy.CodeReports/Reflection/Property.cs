using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Codaxy.CodeReports.Reflection
{
    public delegate object PropertyGetter(object target);
    public delegate void PropertySetter(object target, object value);

    public class Member
    {
        public MemberInfo MemberInfo { get; private set; }
        IMemberValueProvider valueProvider;
        bool HasDynamicValueProvider;

        public bool CanWrite { get; private set; }   

        internal Member(MemberInfo memberInfo, bool emitDynamicValueProvider)
        {
            MemberInfo = memberInfo;
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    Type = ((FieldInfo)memberInfo).FieldType;
                    CanWrite = true;
                    break;
                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberInfo;
                    Type = propertyInfo.PropertyType;
                    CanWrite = propertyInfo.CanWrite;
                    break;
                default:
                    throw new NotSupportedException();
            }
            
            if (emitDynamicValueProvider)
                valueProvider = new DynamicMemberValueProvider(memberInfo);
            else
                valueProvider = new ReflectionMemberValueProvider(memberInfo);
            HasDynamicValueProvider = emitDynamicValueProvider;
        }       

        public object GetValue(object d)
        {
            return valueProvider.GetValue(d);
        }
        public void SetValue(object d, object value)
        {
            valueProvider.SetValue(d, value);
        }

        public String Name { get { return MemberInfo.Name; } }
        public Type Type { get; private set; }
    }
}
