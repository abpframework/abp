using System;
using System.Linq;
using System.Reflection;

namespace Volo.Abp.Validation.StringValues
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StringValueTypeAttribute : Attribute
    {
        public string Name { get; set; }

        public StringValueTypeAttribute(string name)
        {
            Name = name;
        }

        public static string GetName(Type type)
        {
            if (type.IsDefined(typeof(StringValueTypeAttribute)))
            {
                return type.GetCustomAttributes(typeof(StringValueTypeAttribute)).Cast<StringValueTypeAttribute>().First().Name;
            }

            return type.Name;
        }
    }
}