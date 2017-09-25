using System.Linq;
using System.Reflection;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Uow
{
    internal static class UnitOfWorkHelper
    {
        public static bool IsUnitOfWorkType(TypeInfo implementationType)
        {
            //Explicitly defined UnitOfWorkAttribute
            if (HasUnitOfWorkAttribute(implementationType) || AnyMethodHasUnitOfWorkAttribute(implementationType))
            {
                return true;
            }

            //Conventional classes
            if (typeof(IApplicationService).GetTypeInfo().IsAssignableFrom(implementationType) ||
                typeof(IRepository).GetTypeInfo().IsAssignableFrom(implementationType))
            {
                return true;
            }

            return false;
        }

        public static bool IsUnitOfWorkMethod(MethodInfo methodInfo)
        {
            //Method declaration
            var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Any())
            {
                return true;
            }

            if (methodInfo.DeclaringType != null)
            {
                //Class declaration
                attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
                if (attrs.Any())
                {
                    return true;
                }

                //Conventional classes
                if (typeof(IApplicationService).GetTypeInfo().IsAssignableFrom(methodInfo.DeclaringType) ||
                    typeof(IRepository).GetTypeInfo().IsAssignableFrom(methodInfo.DeclaringType))
                {
                    return true;
                }
            }

            return false;
        }

        public static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }
            
            return null;
        }


        private static bool AnyMethodHasUnitOfWorkAttribute(TypeInfo implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(HasUnitOfWorkAttribute);
        }

        private static bool HasUnitOfWorkAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }
    }
}
