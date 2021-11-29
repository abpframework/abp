using System;
using System.IO;

namespace Volo.Abp.Studio.Package
{
    public class PackageDependency
    {
        public ReferenceType Type { get; }

        public string Name { get; }

        public string Version { get; }

        public string Path { get; }

        public PackageDependency(string name, string version)
        {
            Type = ReferenceType.Package;
            Name = name;
            Version = version;
        }

        public PackageDependency(string path)
        {
            Type = ReferenceType.Project;
            Path = path;
            Name = System.IO.Path.GetFileName(path).RemovePostFix(".csproj");
        }
    }
}
