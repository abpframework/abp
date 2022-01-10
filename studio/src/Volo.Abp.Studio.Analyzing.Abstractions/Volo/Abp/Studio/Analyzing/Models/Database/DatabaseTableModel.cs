using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Database;

[PackageContentItemName(ContentTypeName)]
public class DatabaseTableModel : PackageContentItemModel
{
    public const string ContentTypeName = "databaseTable";

    public string EntityFullName { get; private set; }

    public DatabaseTableModel([NotNull] string name, string entityFullName) : base(name)
    {
        EntityFullName = Check.NotNullOrWhiteSpace(entityFullName, nameof(entityFullName));
    }
}
