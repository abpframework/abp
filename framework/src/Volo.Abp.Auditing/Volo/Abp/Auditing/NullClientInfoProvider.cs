using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing
{
    [Dependency(TryRegister = true)]
    public class NullClientInfoProvider : IClientInfoProvider, ISingletonDependency
    {
        public string BrowserInfo => null;

        public string ClientIpAddress => null;

        public string ComputerName => null;
    }
}