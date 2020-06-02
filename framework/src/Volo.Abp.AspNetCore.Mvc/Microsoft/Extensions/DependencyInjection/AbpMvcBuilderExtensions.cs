using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpMvcBuilderExtensions
    {
        public static void AddApplicationPartIfNotExists(this IMvcBuilder mvcBuilder, Assembly assembly)
        {
            mvcBuilder.PartManager.ApplicationParts.AddIfNotContains(assembly);
        }
        
        public static void AddApplicationPartIfNotExists(this IMvcCoreBuilder mvcCoreBuilder, Assembly assembly)
        {
            mvcCoreBuilder.PartManager.ApplicationParts.AddIfNotContains(assembly);
        }
        
        public static void AddIfNotContains(this IList<ApplicationPart> applicationParts, Assembly assembly)
        {
            if (applicationParts.Any(
                p => p is AssemblyPart assemblyPart && assemblyPart.Assembly == assembly))
            {
                return;
            }

            applicationParts.Add(new AssemblyPart(assembly));
        }
    }
}
