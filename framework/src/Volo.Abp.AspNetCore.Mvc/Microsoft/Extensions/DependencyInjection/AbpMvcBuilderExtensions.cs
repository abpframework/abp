using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpMvcBuilderExtensions
    {
        public static void AddApplicationPartIfNotExists(this IMvcBuilder mvcBuilder, Assembly assembly)
        {
            if (mvcBuilder.PartManager.ApplicationParts.Any(
                p => p is AssemblyPart assemblyPart && assemblyPart.Assembly == assembly))
            {
                return;
            }

            mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
        }
    }
}
