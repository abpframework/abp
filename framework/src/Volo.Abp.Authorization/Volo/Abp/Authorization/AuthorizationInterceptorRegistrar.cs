using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization
{
    public static class AuthorizationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            return type.IsDefined(typeof(AuthorizeAttribute), true) ||
                   AnyMethodHasAuthorizeAttribute(type);
        }

        private static bool AnyMethodHasAuthorizeAttribute(Type implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(HasAuthorizeAttribute);
        }

        private static bool HasAuthorizeAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(AuthorizeAttribute), true);
        }
    }
}