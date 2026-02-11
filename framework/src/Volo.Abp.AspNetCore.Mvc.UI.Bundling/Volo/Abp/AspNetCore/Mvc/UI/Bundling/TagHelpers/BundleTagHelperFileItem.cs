using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers;

public class BundleTagHelperFileItem : BundleTagHelperItem
{
    [NotNull]
    public BundleFile File { get; }

    public BundleTagHelperFileItem([NotNull] BundleFile file)
    {
        File = Check.NotNull(file, nameof(file));
    }

    public override string ToString()
    {
        return File.FileName;
    }

    public override void AddToConfiguration(BundleConfiguration configuration)
    {
        configuration.AddFiles(File);
    }
}
