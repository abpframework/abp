using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Database
{
    [PackageContentItemName(ContentTypeName)]
    public class DatabaseCollectionModel : PackageContentItemModel
    {
        public const string ContentTypeName = "databaseCollection";
        
        public string EntityFullName { get; private set; }

        public DatabaseCollectionModel([NotNull] string name, string entityFullName) : base(name)
        {
            EntityFullName = Check.NotNullOrWhiteSpace(entityFullName, nameof(entityFullName));
        }
    }
}