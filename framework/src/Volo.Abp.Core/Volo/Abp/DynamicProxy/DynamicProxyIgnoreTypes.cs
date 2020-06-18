using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.DynamicProxy
{
    /// <summary>
    /// Castle's dynamic proxy class feature will have performance issues for some components, such as the controller of Asp net core MVC.
    /// For related discussions, see: https://github.com/castleproject/Core/issues/486 https://github.com/abpframework/abp/issues/3180
    /// The Abp framework may enable interceptors for certain components (UOW, Auditing, Authorization, etc.), which requires dynamic proxy classes, but will cause application performance to decline.
    /// We need to use other methods for the controller to implement interception, such as middleware or MVC / Page filters.
    /// So we provide some ignored types to avoid enabling dynamic proxy classes.
    /// By default it is empty. When you use middleware or filters for these components in your application, you can add these types to the list.
    /// </summary>
    public static class DynamicProxyIgnoreTypes
    {
        private static HashSet<Type> IgnoredTypes { get; } = new HashSet<Type>();
        
        public static void Add<T>()
        {
            lock (IgnoredTypes)
            {
                IgnoredTypes.AddIfNotContains(typeof(T));
            }
        }
        
        public static bool Contains(Type type, bool includeDerivedTypes = true)
        {
            lock (IgnoredTypes)
            {
                return includeDerivedTypes
                    ? IgnoredTypes.Any(t => t.IsAssignableFrom(type))
                    : IgnoredTypes.Contains(type);
            }
        }
    }
}