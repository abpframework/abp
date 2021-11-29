using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models.Application
{
    [PackageContentItemName(ContentTypeName)]
    public class ApplicationServiceModel : PackageContentItemModel
    {
        public const string ContentTypeName = "applicationService";
        
        public string Namespace { get; set; }
        
        public string Summary { get; set; }
        
        public List<string> ImplementingInterfaces { get; set; }
        
        public ApplicationServiceModel([NotNull] string name) : base(name)
        {
        }
    }
}