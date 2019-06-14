using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class PackageJsonFileFinder : ITransientDependency
    {
        public List<string> Find(string rootDirectory)
        {
            var slash = Path.DirectorySeparatorChar;

            return
                Directory.GetFiles(rootDirectory.EnsureEndsWith(slash), "*package.json", SearchOption.AllDirectories)
                    .Where(f =>
                        !f.Contains(slash + "node_modules" + slash) &&
                        !f.Contains(slash + "Release" + slash) &&
                        !f.Contains(slash + "Debug" + slash) &&
                        IsWithProjectFile(f)
                    ).ToList();
        }

        protected virtual bool IsWithProjectFile(string path)
        {
            var directory = Path.GetDirectoryName(path);

            return Directory.GetFiles(directory, "*.csproj", searchOption: SearchOption.TopDirectoryOnly).Length > 0;
        }
    }
}
