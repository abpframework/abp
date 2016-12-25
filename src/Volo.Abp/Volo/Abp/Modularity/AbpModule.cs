using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public abstract class AbpModule : IAbpModule, IOnApplicationInitialization, IOnApplicationShutdown, IPreConfigureServices, IPostConfigureServices
    {
        public virtual void PreConfigureServices(IServiceCollection services)
        {
            
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            
        }

        public virtual void PostConfigureServices(IServiceCollection services)
        {
            
        }

        public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            
        }

        public virtual void OnApplicationShutdown(ApplicationShutdownContext context)
        {

        }

        public static bool IsAbpModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IAbpModule).GetTypeInfo().IsAssignableFrom(type);
        }

        internal static void CheckAbpModuleType(Type moduleType)
        {
            if (!IsAbpModule(moduleType))
            {
                throw new ArgumentException("Given type is not an ABP module: " + moduleType.AssemblyQualifiedName);
            }
        }
    }
}