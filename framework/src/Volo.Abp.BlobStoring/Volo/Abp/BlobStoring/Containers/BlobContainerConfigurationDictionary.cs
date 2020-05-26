using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring.Containers
{
    public class BlobContainerConfigurationDictionary
    {
        public BlobContainerConfiguration Default { get; }

        private readonly Dictionary<string, BlobContainerConfiguration> _containers;

        public BlobContainerConfigurationDictionary()
        {
            Default = new BlobContainerConfiguration();
            _containers = new Dictionary<string, BlobContainerConfiguration>();
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
            
            configureAction(_containers.GetOrAdd(name, () => new BlobContainerConfiguration(Default)));
            
            return this;
        }

        public BlobContainerConfigurationDictionary ConfigureDefault(Action<BlobContainerConfiguration> configureAction)
        {
            configureAction(Default);
            return this;
        }
        
        [NotNull]
        public BlobContainerConfiguration GetConfiguration<TContainer>([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _containers.GetOrDefault(name) ?? 
                   Default;
        }

        [NotNull]
        public BlobContainerConfiguration GetConfiguration([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _containers.GetOrDefault(name) ?? 
                   Default;
        }
    }
}