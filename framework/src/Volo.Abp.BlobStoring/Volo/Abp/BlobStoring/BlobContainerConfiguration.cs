using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerConfiguration : Dictionary<string, object>
    {
        [NotNull]
        public string Name { get; }
        
        public Type ProviderType { get; set; }
        
        public BlobContainerConfiguration([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }
    }
}