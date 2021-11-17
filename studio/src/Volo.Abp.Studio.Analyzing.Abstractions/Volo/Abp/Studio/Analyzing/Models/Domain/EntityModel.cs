using System.Collections.Generic;

namespace Volo.Abp.Studio.Analyzing.Models.Domain
{
    [PackageContentItemName(ContentTypeName)]
    public class EntityModel : PackageContentItemModel
    {
        public const string ContentTypeName = "entity";
        
        public string Namespace { get; set; }
        
        public string PrimaryKeyType { get; set; }
        
        public string Summary { get; set; }
        
        public List<string> CollectionProperties { get; set; }
        public List<string> NavigationProperties { get; set; }

        public EntityModel(string name) : base(name)
        {
            CollectionProperties = new List<string>();
            NavigationProperties = new List<string>();
        }
    }
}