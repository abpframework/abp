using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Database;

[PackageContentItemName(ContentTypeName)]
public class EfCoreDbContextModel : PackageContentItemModel, IDbContextModel
{
    public const string ContentTypeName = "efCoreDbContext";

    public string Namespace { get; private set; }

    public string ConnectionStringName { get; set; }

    public List<DatabaseTableModel> DatabaseTables { get; set; }

    public EfCoreDbContextModel(
        [NotNull] string name,
        [NotNull] string @namespace
        ) : base(name)
    {
        Namespace = Check.NotNullOrWhiteSpace(@namespace, nameof(@namespace));
    }
}
