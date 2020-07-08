using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc
{
    /// <summary>
    /// This class is used to align order of the MVC Application Parts with the order of
    /// ABP module dependencies.
    /// </summary>
    public static class ApplicationPartSorter
    {
        public static void Sort(ApplicationPartManager partManager, IModuleContainer moduleContainer)
        {
            /* Performing a double Reverse() to preserve the original order for non-sorted parts
             */

            var dependencyDictionary = CreateDependencyDictionary(partManager, moduleContainer);

            var sortedParts = partManager
                .ApplicationParts
                .Reverse() //First Revers
                .SortByDependencies(p => dependencyDictionary[p]);

            sortedParts.Reverse(); //Reverse again

            //Replace the original parts with the sorted parts
            partManager.ApplicationParts.Clear();
            foreach (var applicationPart in sortedParts)
            {
                partManager.ApplicationParts.Add(applicationPart);
            }
        }

        private static Dictionary<ApplicationPart, List<ApplicationPart>> CreateDependencyDictionary(
            ApplicationPartManager partManager, IModuleContainer moduleContainer)
        {
            var dependencyDictionary = new Dictionary<ApplicationPart, List<ApplicationPart>>();

            foreach (var applicationPart in partManager.ApplicationParts)
            {
                dependencyDictionary[applicationPart] =
                    CreateDependencyList(applicationPart, partManager, moduleContainer);
            }

            return dependencyDictionary;
        }

        private static List<ApplicationPart> CreateDependencyList(
            ApplicationPart applicationPart,
            ApplicationPartManager partManager,
            IModuleContainer moduleContainer)
        {
            var list = new List<ApplicationPart>();

            if (applicationPart is AssemblyPart assemblyPart)
            {
                AddDependencies(list, assemblyPart, partManager, moduleContainer);
            }
            else if (applicationPart is CompiledRazorAssemblyPart compiledRazorAssemblyPart)
            {
                AddDependencies(list, compiledRazorAssemblyPart, partManager, moduleContainer);
            }

            return list;
        }

        private static void AddDependencies(
            List<ApplicationPart> list,
            AssemblyPart assemblyPart,
            ApplicationPartManager partManager,
            IModuleContainer moduleContainer)
        {
            var dependedAssemblyParts = GetDependedAssemblyParts(
                partManager,
                moduleContainer,
                assemblyPart
            );
            
            list.AddRange(dependedAssemblyParts);

            foreach (var dependedAssemblyPart in dependedAssemblyParts)
            {
                var viewsPart = GetViewsPartOrNull(partManager, dependedAssemblyPart);
                if (viewsPart != null)
                {
                    list.Add(viewsPart);
                }
            }
        }
        
        private static void AddDependencies(
            List<ApplicationPart> list,
            CompiledRazorAssemblyPart compiledRazorAssemblyPart,
            ApplicationPartManager partManager,
            IModuleContainer moduleContainer)
        {
            if (!compiledRazorAssemblyPart.Name.EndsWith(".Views"))
            {
                return;
            }

            var originalAssemblyPart = GetOriginalAssemblyPartOrNull(compiledRazorAssemblyPart, partManager);
            if (originalAssemblyPart == null)
            {
                return;
            }

            list.Add(originalAssemblyPart);
        }
        
        private static AssemblyPart[] GetDependedAssemblyParts(
            ApplicationPartManager partManager,
            IModuleContainer moduleContainer,
            AssemblyPart assemblyPart)
        {
            var moduleDescriptor = GetModuleDescriptorForAssemblyOrNull(moduleContainer, assemblyPart.Assembly);
            if (moduleDescriptor == null)
            {
                return Array.Empty<AssemblyPart>();
            }

            var moduleDependedAssemblies = moduleDescriptor
                .Dependencies
                .Select(d => d.Assembly)
                .ToArray();
            
            return partManager.ApplicationParts
                .OfType<AssemblyPart>()
                .Where(a => a.Assembly.IsIn(moduleDependedAssemblies))
                .Distinct()
                .ToArray();
        }

        private static CompiledRazorAssemblyPart GetViewsPartOrNull(ApplicationPartManager partManager,
            ApplicationPart assemblyPart)
        {
            var viewsAssemblyName = assemblyPart.Name + ".Views";
            return partManager
                .ApplicationParts
                .OfType<CompiledRazorAssemblyPart>()
                .FirstOrDefault(p => p.Name == viewsAssemblyName);
        }

        private static AssemblyPart GetOriginalAssemblyPartOrNull(
            CompiledRazorAssemblyPart compiledRazorAssemblyPart,
            ApplicationPartManager partManager)
        {
            var originalAssemblyName = compiledRazorAssemblyPart.Name.RemovePostFix(".Views");
            return partManager.ApplicationParts
                .OfType<AssemblyPart>()
                .FirstOrDefault(p => p.Name == originalAssemblyName);
        }

        private static IAbpModuleDescriptor GetModuleDescriptorForAssemblyOrNull(
            IModuleContainer moduleContainer,
            Assembly assembly)
        {
            return moduleContainer
                .Modules
                .FirstOrDefault(m => m.Assembly == assembly);
        }
    }
}