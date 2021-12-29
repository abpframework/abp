using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.ProxyScripting.Generators;

public interface IProxyScriptGenerator
{
    string CreateScript(ApplicationApiDescriptionModel model);
}
