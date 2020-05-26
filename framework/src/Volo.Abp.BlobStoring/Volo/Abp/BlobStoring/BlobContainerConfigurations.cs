using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerConfigurations
    {
        public BlobContainerConfiguration Default { get; }

        private readonly Dictionary<string, BlobContainerConfiguration> _containers;

        public BlobContainerConfigurations()
        {
            Default = new BlobContainerConfiguration();
            _containers = new Dictionary<string, BlobContainerConfiguration>();
        }

        public BlobContainerConfigurations Configure<TContainer>(
            Action<BlobContainerConfiguration> configureAction)
        {
            return Configure(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                configureAction
            );
        }

        public BlobContainerConfigurations Configure(
            [NotNull] string name,
            [NotNull] Action<BlobContainerConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));
            
            configureAction(_containers.GetOrAdd(name, () => new BlobContainerConfiguration(Default)));
            
            return this;
        }

        public BlobContainerConfigurations ConfigureDefault(Action<BlobContainerConfiguration> configureAction)
        {
            configureAction(Default);
            return this;
        }
        
        [NotNull]
        public BlobContainerConfiguration GetConfiguration<TContainer>()
        {
            return GetConfiguration(BlobContainerNameAttribute.GetContainerName<TContainer>());
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