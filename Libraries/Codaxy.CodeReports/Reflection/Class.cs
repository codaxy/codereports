using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Codaxy.CodeReports.Reflection
{
    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(String propertyName) : base(String.Format("Property '{0}' not found!", propertyName)) { }
        public MemberNotFoundException(int propertyIndex) : base(String.Format("Property at position {0} not found!", propertyIndex)) { }
    }
   
    public class Class
    {
        Dictionary<String, Member> members;
        Member[] memberList;
        public Type Type { get; private set; }

        private Class(Type type)
        {
            Type = type;
            members = new Dictionary<string, Member>();
        }

        public string[] GetPropertyNames() { return GetMembers().Select(a=>a.Name).ToArray(); }
        
        public object GetPropertyValue(object target, String propertyName) { return GetMember(propertyName).GetValue(target); }        
        public object GetPropertyValue(object target, int propertyIndex) { return GetMember(propertyIndex).GetValue(target); }        
        
        public Type GetPropertyType(string propertyName) { return GetMember(propertyName).Type; }
        public Type GetPropertyType(int propertyIndex) { return GetMember(propertyIndex).Type; }

        public void SetPropertyValue(object target, String propertyName, object value) { GetMember(propertyName).SetValue(target, value); }
        public void SetPropertyValue(object target, int propertyIndex, object value) { GetMember(propertyIndex).SetValue(target, value); }
        
        public Member GetMember(string propertyName) {
            Member p;
            if (members.TryGetValue(propertyName, out p))
                return p;
            lock (members)
            {
                var mi = Type.GetMember(propertyName);
                if (mi.Length != 1)
                    throw new MemberNotFoundException(propertyName);
                return members[propertyName] = new Member(mi[0], true);
            }            
        }

        public Member GetMember(int propertyIndex)
        {
            EnsureAllMembersLoaded();

            if (propertyIndex < 0 || propertyIndex >= memberList.Length)
                throw new MemberNotFoundException(propertyIndex);
            return memberList[propertyIndex];
        }

        public Member this[String propertyName] { get { return GetMember(propertyName); } }
        public Member this[int propertyIndex] { get { return GetMember(propertyIndex); } }

        public Member[] Members { get { return GetMembers(); } }

        private Member[] GetMembers()
        {
            EnsureAllMembersLoaded();
            return memberList;
        }

        public bool TrySetMemberValues(object o, Dictionary<string, object> values)
        {
            int errors = 0;
            if (values != null)
                foreach (var a in values)
                {
                    Member p;
                    if (TryGetMember(a.Key, out p))
                    {
                        try
                        {
                            var value = a.Value;
                            if (value != null && value.GetType() != p.Type)
                                value = Codaxy.Common.Convert.ChangeType(value, p.Type);
                            p.SetValue(o, value);
                        }
                        catch
                        {
                            errors++;
                        }
                    }
                }
            return errors == 0;
        }

        public void SetMemberValues(object o, Dictionary<string, object> values)
        {
            if (values != null)
                foreach (var a in values)
                {
                    Member p;
                    if (TryGetMember(a.Key, out p))
                        p.SetValue(o, a.Value);
                }
        }


        public bool TryGetMember(string propertyName, out Member prop)
        {
            EnsureAllMembersLoaded();
            return members.TryGetValue(propertyName, out prop);
        }

        public Dictionary<String, object> GetMemberValues(object o)
        {
            Dictionary<String, object> data = new Dictionary<string, object>();
            foreach (var m in GetMembers())
                data[m.Name] = m.GetValue(o);
            return data;
        }

        private void EnsureAllMembersLoaded()
        {
            if (memberList == null)
            {
                List<String> members = new List<string>();
                members.AddRange(Type.GetProperties().Select(a => a.Name));
                members.AddRange(Type.GetFields().Select(a => a.Name));
                memberList = new Member[members.Count];
                for (int i = 0; i < members.Count; i++)
                    memberList[i] = GetMember(members[i]);
            }
        }

        static Dictionary<Type, Class> typeCache = new Dictionary<Type, Class>();

        public static Class Create(Type classType)
        {
            Class c;
            if (typeCache.TryGetValue(classType, out c))
                return c;
            return typeCache[classType] = new Class(classType);
        }
    }
}
