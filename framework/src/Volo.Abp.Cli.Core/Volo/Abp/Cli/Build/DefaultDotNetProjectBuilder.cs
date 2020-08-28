using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Build
{
    public class DefaultDotNetProjectBuilder : IDotNetProjectBuilder, ITransientDependency
    {
        public List<string> Build(List<DotNetProjectInfo> projects, int maxParallelBuildCount, string arguments)
        {
            var builtProjects = new ConcurrentBag<string>();

            try
            {
                foreach (var project in projects)
                {
                    if (builtProjects.Contains(project.CsProjPath))
                    {
                        continue;
                    }

                    BuildInternal(project, arguments, builtProjects);

                    if (!project.Dependencies.Any())
                    {
                        continue;
                    }

                    // TODO: use parallel foreach here.
                    foreach (var projectDependency in project.Dependencies)
                    {
                        if (builtProjects.Contains(project.CsProjPath))
                        {
                            continue;
                        }

                        BuildInternal(projectDependency, arguments, builtProjects);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return builtProjects.ToList();
        }

        private void BuildInternal(DotNetProjectInfo project, string arguments, ConcurrentBag<string> builtProjects)
        {
            Console.WriteLine("Building...: dotnet build " + project.CsProjPath + " " + arguments);

            CmdHelper.RunCmdAndGetOutput(
                "dotnet build " + project.CsProjPath + " " + arguments,
                out int buildStatus
            );

            if (buildStatus == 0)
            {
                builtProjects.Add(project.CsProjPath);
            }
            else
            {
                // TODO: throw exception and stop build process.
                Console.WriteLine("Build failed for :" + project.CsProjPath);
            }
        }
    }
}
