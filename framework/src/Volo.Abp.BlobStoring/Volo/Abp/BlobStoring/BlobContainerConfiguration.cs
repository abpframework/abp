using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerConfiguration
    {
        public Type ProviderType { get; set; }

        [NotNull]
        private readonly Dictionary<string, object> _properties;
        
        [CanBeNull]
        private readonly BlobContainerConfiguration _fallbackConfiguration;
        
        public BlobContainerConfiguration(BlobContainerConfiguration fallbackConfiguration = null)
        {
            _fallbackConfiguration = fallbackConfiguration;
            _properties = new Dictionary<string, object>();
        }

        [CanBeNull]
        public T GetConfigurationOrDefault<T>(string name, T defaultValue = default)
        {
            return (T) GetConfigurationOrNull(name, defaultValue);
        }
        
        [CanBeNull]
        public object GetConfigurationOrNull(string name, object defaultValue = null)
        {
            return _properties.GetOrDefault(name) ??
                   _fallbackConfiguration?.GetConfigurationOrNull(name, defaultValue) ??
                   defaultValue;
        }
        
        [NotNull]
        public BlobContainerConfiguration SetConfiguration([NotNull] string name, [CanBeNull] object value)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(value, nameof(value));
            
            _properties[name] = value;
            
            return this;
        }
        
        [NotNull]
        public BlobContainerConfiguration ClearConfiguration([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            _properties.Remove(name);
            
            return this;
        }
    }
}