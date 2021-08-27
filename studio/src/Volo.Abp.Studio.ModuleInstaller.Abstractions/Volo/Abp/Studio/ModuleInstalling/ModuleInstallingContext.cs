using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Studio.Packages;

namespace Volo.Abp.Studio.ModuleInstalling
{
    public class ModuleInstallingContext
    {
        public bool WithSourceCode { get; set; }

        public bool AddToSolutionFile { get; set; }

        public string Version { get; set; }

        public List<PackageInfo> TargetModulePackages { get; protected set; }

        public List<PackageInfoWithAnalyze> ReferenceModulePackages { get; protected set; }

        public Dictionary<string,string> Options { get; }

        public IServiceProvider ServiceProvider { get; }

        public ModuleInstallingContext(
            bool withSourceCode,
            bool addToSolutionFile,
            string version,
            Dictionary<string,string> options,
            IServiceProvider serviceProvider)
        {
            WithSourceCode = withSourceCode;
            AddToSolutionFile = addToSolutionFile;
            Version = version;
            Options = options;

            TargetModulePackages = new List<PackageInfo>();
            ReferenceModulePackages = new List<PackageInfoWithAnalyze>();

            ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
        }

        public void SetReferenceModulePackages([NotNull] List<PackageInfoWithAnalyze> referenceModulePackages)
        {
            Check.NotNull(referenceModulePackages, nameof(referenceModulePackages));

            ReferenceModulePackages = referenceModulePackages;
        }
        
        public void SetTargetModulePackages([NotNull] List<PackageInfo> targetModulePackages)
        {
            Check.NotNull(targetModulePackages, nameof(targetModulePackages));

            TargetModulePackages = targetModulePackages;
        }
    }
}
