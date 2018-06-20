using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Configuration
{
    public interface IConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
