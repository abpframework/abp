using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.ViewFeatures
{
    /// <summary>
    /// The code for this class comes from
    /// https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.Razor/src/ApplicationParts/RazorCompiledItemFeatureProvider.cs
    /// </summary>
    public class AbpRazorCompiledItemFeatureProvider : IApplicationFeatureProvider<ViewsFeature>
    {
        private readonly IAbpApplication _application;

        public AbpRazorCompiledItemFeatureProvider(IAbpApplication application)
        {
            _application = application;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ViewsFeature feature)
        {
            var virtualFileProvider = _application.ServiceProvider
                .GetRequiredService<IVirtualFileProvider>();

            foreach (var provider in parts.OfType<IRazorCompiledItemProvider>())
            {
                // Ensure parts do not specify views with differing cases. This is not supported
                // at runtime and we should flag at as such for precompiled views.
                var duplicates = provider.CompiledItems
                    .GroupBy(i => i.Identifier, StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault(g => g.Count() > 1);

                if (duplicates != null)
                {
                    var viewsDifferingInCase = string.Join(Environment.NewLine, duplicates.Select(d => d.Identifier));

                    var message = string.Join(
                        Environment.NewLine,
                        "The following precompiled view paths differ only in case, which is not supported:",
                        viewsDifferingInCase);
                    throw new InvalidOperationException(message);
                }

                foreach (var item in provider.CompiledItems)
                {
                    // Skip pages existing in the virtual file system. This allows us to replace pre-compiled pages.
                    if (virtualFileProvider.GetFileInfo(item.Identifier).Exists)
                    {
                        continue;
                    }

                    var descriptor = new CompiledViewDescriptor(item);
                    feature.ViewDescriptors.Add(descriptor);
                }
            }
        }
    }
}
