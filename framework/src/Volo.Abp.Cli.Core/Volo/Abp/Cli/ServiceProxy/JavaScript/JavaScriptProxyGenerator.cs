using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ServiceProxy.JavaScript
{
    public class JavaScriptServiceProxyGenerator : IServiceProxyGenerator, ITransientDependency
    {
        public const string Name = "JS";

        public Task GenerateProxyAsync(GenerateProxyArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
