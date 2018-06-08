using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Configuration
{
    public class DefaultConfigurationAccessor : IConfigurationAccessor
    {
        public static DefaultConfigurationAccessor Empty { get; }

        public virtual IConfigurationRoot Configuration { get; }

        static DefaultConfigurationAccessor()
        {
            Empty = new DefaultConfigurationAccessor(
                new ConfigurationBuilder().Build()
            );
        }

        public DefaultConfigurationAccessor(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }
    }
}