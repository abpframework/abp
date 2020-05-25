using System;
using System.Collections.Generic;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerConfigurationDictionary : Dictionary<string, BlobContainerConfiguration>
    {
        public BlobContainerConfigurationDictionary Configure<TContainer>(Action<BlobContainerConfiguration> configureAction)
        {
            return Configure(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                configureAction
            );
        }
        
        public BlobContainerConfigurationDictionary Configure(string name, Action<BlobContainerConfiguration> configureAction)
        {
            configureAction(this.GetOrAdd(name, () => new BlobContainerConfiguration()));
            return this;
        }
    }
}