using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Internal;

namespace Volo.DependencyInjection
{
    public abstract class ConventionalRegistrarBase : IConventionalRegistrar
    {
        public virtual void AddAssembly(IServiceCollection services, Assembly assembly)
        {
            var types = AssemblyHelper
                .GetAllTypes(assembly)
                .Where(t =>
                {
                    var typeInfo = t.GetTypeInfo();
                    return typeInfo.IsClass &&
                           !typeInfo.IsAbstract &&
                           !typeInfo.IsGenericType;
                });

            AddTypes(services, types.ToArray());
        }

        public virtual void AddTypes(IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                AddType(services, type);
            }
        }

        public abstract void AddType(IServiceCollection services, Type type);
    }
}