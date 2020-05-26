using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerConfigurationDictionary : Dictionary<string, BlobContainerConfiguration>
    {
        public BlobContainerConfiguration Default { get; }

        public BlobContainerConfigurationDictionary()
        {
            Default = new BlobContainerConfiguration("_default");
        }

        public BlobContainerConfigurationDictionary Configure<TContainer>(
            Action<BlobContainerConfiguration> configureAction)
        {
            return Configure(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                configureAction
            );
        }

        public BlobContainerConfigurationDictionary Configure(
            [NotNull] string name,
            [NotNull] Action<BlobContainerConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));
            
            configureAction(this.GetOrAdd(name, () => new BlobContainerConfiguration(name)));
            
            return this;
        }

        public BlobContainerConfigurationDictionary ConfigureDefault(Action<BlobContainerConfiguration> configureAction)
        {
            configureAction(Default);
            return this;
        }

        public BlobContainerConfiguration GetOrDefaultConfiguration(string name)
        {
            return AbpDictionaryExtensions.GetOrDefault(this, name) ??
                   Default;
        }
    }
}