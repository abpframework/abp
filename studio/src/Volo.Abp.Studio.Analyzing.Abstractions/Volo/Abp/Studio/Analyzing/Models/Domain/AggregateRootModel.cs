namespace Volo.Abp.Studio.Analyzing.Models.Domain
{
    [PackageContentItemName(ContentTypeName)]
    public class AggregateRootModel : EntityModel
    {
        public new const string ContentTypeName = "aggregateRoot";

        public AggregateRootModel(string name) : base(name)
        {
        }
    }
}