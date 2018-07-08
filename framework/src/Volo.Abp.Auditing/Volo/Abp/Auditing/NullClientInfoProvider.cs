using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing
{
    //TODO: Implement on aspnet core layer
    public class NullClientInfoProvider : IClientInfoProvider, ISingletonDependency
    {
        public static NullClientInfoProvider Instance { get; } = new NullClientInfoProvider();

        public string BrowserInfo => null;
        public string ClientIpAddress => null;
        public string ComputerName => null;
    }
}