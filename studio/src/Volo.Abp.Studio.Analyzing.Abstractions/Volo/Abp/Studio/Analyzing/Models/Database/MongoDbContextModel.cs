using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Database;

[PackageContentItemName(ContentTypeName)]
public class MongoDbContextModel : PackageContentItemModel, IDbContextModel
{
    public const string ContentTypeName = "mongoDbContext";

    public string Namespace { get; private set; }

    public string ConnectionStringName { get; set; }

    public List<DatabaseCollectionModel> DatabaseCollections { get; set; }

    public MongoDbContextModel(
        [NotNull] string name,
        [NotNull] string @namespace
        ) : base(name)
    {
        Namespace = Check.NotNullOrWhiteSpace(@namespace, nameof(@namespace));
    }
}
