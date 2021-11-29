using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Module
{
    [PackageContentItemName(ContentTypeName)]
    public class AbpModuleModel : PackageContentItemModel
    {
        public const string ContentTypeName = "abpModule";
        
        public string Namespace { get; set; }
        
        public AbpModuleModel([NotNull] string name) : base(name)
        {
        }
    }
}