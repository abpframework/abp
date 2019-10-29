using System;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nito.AsyncEx;

namespace Volo.Abp.Threading
{
    /// <summary>
    /// Provides some helper methods to work with async methods.
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// Checks if given method is an async method.
        /// </summary>
        /// <param name="method">A method to check</param>
        public static bool IsAsync([NotNull] this MethodInfo method)
        {
            Check.NotNull(method, nameof(method));

            return method.ReturnType.IsTaskOrTaskOfT();
        }

        public static bool IsTaskOrTaskOfT([NotNull] this Type type)
        {
            return type == typeof(Task) || (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>));
        }

        /// <summary>
        /// Returns void if given type is Task.
        /// Return T, if given type is Task{T}.
        /// Returns given type otherwise.
        /// </summary>
        public static Type UnwrapTask([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));

            if (type == typeof(Task))
            {
                return typeof(void);
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return type.GenericTypeArguments[0];
            }

            return type;
        }

        /// <summary>
        /// Runs a async method synchronously.
        /// </summary>
        /// <param name="func">A function that returns a result</param>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <returns>Result of the async operation</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }

        /// <summary>
        /// Runs a async method synchronously.
        /// </summary>
        /// <param name="action">An async action</param>
        public static void RunSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}
