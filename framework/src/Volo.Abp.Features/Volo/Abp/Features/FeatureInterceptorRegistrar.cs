using System;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    public static class FeatureInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<FeatureInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            return type.IsDefined(typeof(RequiresFeatureAttribute), true) ||
                   AnyMethodRequiresFeatureAttribute(type);
        }

        private static bool AnyMethodRequiresFeatureAttribute(Type implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(HasRequiresFeatureAttribute);
        }

        private static bool HasRequiresFeatureAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(RequiresFeatureAttribute), true);
        }
    }
}