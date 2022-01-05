using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Domain;

[PackageContentItemName(ContentTypeName)]
public class RepositoryInterfaceModel : PackageContentItemModel
{
    public const string ContentTypeName = "repositoryInterface";

    public string Namespace { get; set; }

    public string Summary { get; set; }

    public EntityModel EntityModel { get; set; }

    public RepositoryInterfaceModel([NotNull] string name) : base(name)
    {
    }
}
