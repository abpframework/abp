using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Build;

public class DefaultBuildProjectListSorter : IBuildProjectListSorter, ITransientDependency
{
    public List<DotNetProjectInfo> SortByDependencies(
        List<DotNetProjectInfo> source,
        IEqualityComparer<DotNetProjectInfo> comparer = null)
    {
        /* See: http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp
         *      http://en.wikipedia.org/wiki/Topological_sorting
         */

        var sorted = new List<DotNetProjectInfo>();
        var visited = new Dictionary<DotNetProjectInfo, bool>(comparer);

        foreach (var item in source)
        {
            SortByDependenciesVisit(source, item, sorted, visited);
        }

        return sorted;
    }

    private void SortByDependenciesVisit(
        List<DotNetProjectInfo> source,
        DotNetProjectInfo item,
        List<DotNetProjectInfo> sorted,
        Dictionary<DotNetProjectInfo, bool> visited)
    {
        bool inProcess;
        var alreadyVisited = visited.TryGetValue(item, out inProcess);

        if (alreadyVisited)
        {
            if (inProcess)
            {
                throw new ArgumentException("Cyclic dependency found! Item: " + item);
            }
        }
        else
        {
            visited[item] = true;

            var dependencies = item.Dependencies;
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    var dependencyItem = source.FirstOrDefault(e => e.CsProjPath == dependency.CsProjPath);
                    if (dependencyItem != null)
                    {
                        SortByDependenciesVisit(source, dependencyItem, sorted, visited);
                    }
                }
            }

            visited[item] = false;
            sorted.Add(item);
        }
    }
}
