using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Domain;

[PackageContentItemName(ContentTypeName)]
public class DomainServiceModel : PackageContentItemModel
{
    public const string ContentTypeName = "domainService";

    public string Namespace { get; set; }

    public string Summary { get; set; }

    public DomainServiceModel([NotNull] string name) : base(name)
    {
    }
}
