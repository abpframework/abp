using System;
using System.Linq;
using System.Reflection;

namespace Volo.Abp.Data
{
    public class DatabaseNameAttribute : Attribute
    {
        public string Name { get; }

        public DatabaseNameAttribute(string name)
        {
            Name = name;
        }

        public static string GetDatabaseName<T>()
        {
            return GetDatabaseName(typeof(T));
        }

        public static string GetDatabaseName(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var databaseNameAttribute = typeInfo.GetCustomAttributes<DatabaseNameAttribute>().FirstOrDefault();
            if (databaseNameAttribute != null)
            {
                return databaseNameAttribute.Name;
            }

            return type.FullName;
        }
    }
}