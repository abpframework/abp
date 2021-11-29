using System;
using System.Linq;
using System.Reflection;

namespace Volo.Abp.Validation.StringValues
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValueValidatorAttribute : Attribute
    {
        public string Name { get; set; }

        public ValueValidatorAttribute(string name)
        {
            Name = name;
        }

        public static string GetName(Type type)
        {
            if (type.IsDefined(typeof(ValueValidatorAttribute)))
            {
                return type.GetCustomAttributes(typeof(ValueValidatorAttribute)).Cast<ValueValidatorAttribute>().First().Name;
            }

            return type.Name;
        }
    }
}