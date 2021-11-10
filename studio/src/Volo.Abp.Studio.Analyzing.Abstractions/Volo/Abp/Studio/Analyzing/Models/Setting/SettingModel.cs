using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Setting
{
    [PackageContentItemName(ContentTypeName)]
    public class SettingModel : PackageContentItemModel
    {
        public const string ContentTypeName = "setting";

        public string DefaultValue { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsVisibleToClient { get; set; }

        public bool IsInherited { get; set; }

        public bool IsEncrypted { get; set; }
        
        public SettingModel([NotNull] string name) : base(name)
        {
        }
    }
}