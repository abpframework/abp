using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp;

namespace System
{
    public static class AbpTypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// Generates a friendly name which includes generic type parameters.
        /// <br/>See original: https://stackoverflow.com/a/56756010/2634818
        /// </summary>
        /// <returns>A string representation of the Type including generic type parameters. </returns>
        public static string GetFriendlyName(this Type type)
        {
            if (type.IsGenericType)
            {
                return GetTypeString(type);
            }
            return type.FullName;
        }

        private static string GetTypeString(Type type)
        {
            var t = type.AssemblyQualifiedName;

            var output = new StringBuilder();
            List<string> typeStrings = new List<string>();

            int iAssyBackTick = t.IndexOf('`') + 1;
            output.Append(t.Substring(0, iAssyBackTick - 1).Replace("[", string.Empty));
            var genericTypes = type.GetGenericArguments();

            foreach (var genType in genericTypes)
            {
                typeStrings.Add(genType.IsGenericType ? GetTypeString(genType) : genType.ToString());
            }

            output.Append($"<{string.Join(",", typeStrings)}>");
            return output.ToString();
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <typeparamref name="TTarget"></typeparamref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/>.
        /// </summary>
        /// <typeparam name="TTarget">Target type</typeparam> (as reverse).
        public static bool IsAssignableTo<TTarget>([NotNull] this Type type)
        {
            Check.NotNull(type, nameof(type));

            return type.IsAssignableTo(typeof(TTarget));
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <paramref name="targetType"></paramref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/> (as reverse).
        /// </summary>
        /// <param name="type">this type</param>
        /// <param name="targetType">Target type</param>
        public static bool IsAssignableTo([NotNull] this Type type, [NotNull] Type targetType)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(targetType, nameof(targetType));

            return targetType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets all base classes of this type.
        /// </summary>
        /// <param name="type">The type to get its base classes.</param>
        /// <param name="includeObject">True, to include the standard <see cref="object"/> type in the returned array.</param>
        public static Type[] GetBaseClasses([NotNull] this Type type, bool includeObject = true)
        {
            Check.NotNull(type, nameof(type));

            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            return types.ToArray();
        }

        /// <summary>
        /// Gets all base classes of this type.
        /// </summary>
        /// <param name="type">The type to get its base classes.</param>
        /// <param name="stoppingType">A type to stop going to the deeper base classes. This type will be be included in the returned array</param>
        /// <param name="includeObject">True, to include the standard <see cref="object"/> type in the returned array.</param>
        public static Type[] GetBaseClasses([NotNull] this Type type, Type stoppingType, bool includeObject = true)
        {
            Check.NotNull(type, nameof(type));

            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            return types.ToArray();
        }

        private static void AddTypeAndBaseTypesRecursively(
            [NotNull] List<Type> types,
            [CanBeNull] Type type,
            bool includeObject,
            [CanBeNull] Type stoppingType = null)
        {
            if (type == null || type == stoppingType)
            {
                return;
            }

            if (!includeObject && type == typeof(object))
            {
                return;
            }

            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            types.Add(type);
        }
    }
}
