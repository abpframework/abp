using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Data
{
    public class DatabaseNameAttribute : Attribute
    {
        [NotNull]
        public string Name { get; }

        public DatabaseNameAttribute([NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
        }

        public static string GetDatabaseName<T>()
        {
            return GetDatabaseName(typeof(T));
        }

        public static string GetDatabaseName(Type type)
        {
            var databaseNameAttribute = type.GetTypeInfo().GetCustomAttribute<DatabaseNameAttribute>();

            if (databaseNameAttribute == null)
            {
                return type.FullName;
            }

            return databaseNameAttribute.Name;
        }
    }
}