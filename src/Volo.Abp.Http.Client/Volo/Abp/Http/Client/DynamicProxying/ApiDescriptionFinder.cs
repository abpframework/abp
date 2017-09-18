using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class ApiDescriptionFinder : IApiDescriptionFinder, ISingletonDependency
    {
        private readonly IApiDescriptionCache _descriptionCache;

        public ApiDescriptionFinder(IApiDescriptionCache descriptionCache)
        {
            _descriptionCache = descriptionCache;
        }

        public async Task<ActionApiDescriptionModel> FindActionAsync(DynamicHttpClientProxyConfig proxyConfig, MethodInfo method)
        {
            var apiDescription = await _descriptionCache.GetAsync(proxyConfig.BaseUrl);

            //TODO: Cache finding?

            var methodParameters = method.GetParameters().ToArray();

            foreach (var module in apiDescription.Modules.Values)
            {
                if (module.Name != proxyConfig.ModuleName)
                {
                    continue;
                }

                foreach (var controller in module.Controllers.Values)
                {
                    if (controller.Interfaces.All(i => i.TypeAsString != proxyConfig.Type.FullName))
                    {
                        continue;
                    }

                    foreach (var action in controller.Actions.Values)
                    {
                        if (action.NameOnClass == method.Name && action.ParametersOnMethod.Count == methodParameters.Length)
                        {
                            var found = true;

                            for (int i = 0; i < methodParameters.Length; i++)
                            {
                                if (action.ParametersOnMethod[i].TypeAsString != methodParameters[i].ParameterType.FullName)
                                {
                                    found = false;
                                    break;
                                }
                            }

                            if (found)
                            {
                                return action;
                            }
                        }
                    }
                }
            }

            throw new AbpException("Could not found remote action for method: " + method);
        }
    }
}
