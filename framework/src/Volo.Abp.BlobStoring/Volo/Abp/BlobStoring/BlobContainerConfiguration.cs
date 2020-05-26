using System;
using System.Collections.Generic;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerConfiguration : Dictionary<string, object>
    {
        public Type ProviderType { get; set; }
    }
}