using System.Collections.Generic;

namespace Volo.Abp.Cli.ProjectModification
{
    public class ModuleWithMastersInfo : ModuleInfo
    {
        public List<ModuleWithMastersInfo> MasterModuleInfos { get; set; }
    }
}