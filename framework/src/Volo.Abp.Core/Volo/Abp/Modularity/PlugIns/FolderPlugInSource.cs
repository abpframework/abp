using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using JetBrains.Annotations;
using Volo.Abp.Reflection;

namespace Volo.Abp.Modularity.PlugIns
{
    public class FolderPlugInSource : IPlugInSource
    {
        public string Folder { get; }

        public SearchOption SearchOption { get; set; }

        public Func<string, bool> Filter { get; set; }

        public FolderPlugInSource(
            [NotNull] string folder,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            Check.NotNull(folder, nameof(folder));

            Folder = folder;
            SearchOption = searchOption;
        }

        public Type[] GetModules()
        {
            var modules = new List<Type>();

            foreach (var assembly in GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (AbpModule.IsAbpModule(type))
                        {
                            modules.AddIfNotContains(type);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new AbpException("Could not get module types from assembly: " + assembly.FullName, ex);
                }
            }

            return modules.ToArray();
        }

        private List<Assembly> GetAssemblies()
        {
            var assemblyFiles = AssemblyHelper.GetAssemblyFiles(Folder, SearchOption);

            if (Filter != null)
            {
                assemblyFiles = assemblyFiles.Where(Filter);
            }

            return assemblyFiles.Select(AssemblyLoadContext.Default.LoadFromAssemblyPath).ToList();
        }
    }
}
