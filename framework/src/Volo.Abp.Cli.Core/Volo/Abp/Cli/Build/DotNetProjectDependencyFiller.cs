using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Build
{
    public class DotNetProjectDependencyFiller : IDotNetProjectDependencyFiller, ITransientDependency
    {
        public void Fill(List<DotNetProjectInfo> projects)
        {
            foreach (var project in projects)
            {
                FillProjectDependencies(project);
            }
        }

        private void FillProjectDependencies(DotNetProjectInfo project)
        {
            var projectNode = XElement.Load(project.CsProjPath);
            var referenceNodes = projectNode.Descendants("ItemGroup").Descendants("ProjectReference");

            foreach (var referenceNode in referenceNodes)
            {
                if (referenceNode.Attribute("Include") == null)
                {
                    continue;
                }

                var relativePath = referenceNode.Attribute("Include").Value;
                var file = new FileInfo(project.CsProjPath);
                var referenceProjectInfo = new FileInfo(Path.Combine(file.Directory.FullName, relativePath));

                var referenceProject = new DotNetProjectInfo(project.RepositoryName, referenceProjectInfo.FullName, false);
                project.Dependencies.Add(referenceProject);
            }
        }
    }
}
