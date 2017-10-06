using System;
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

        public async Task<ActionApiDescriptionModel> FindActionAsync(string baseUrl, Type serviceType, MethodInfo method)
        {
            var apiDescription = await _descriptionCache.GetAsync(baseUrl);

            //TODO: Cache finding?

            var methodParameters = method.GetParameters().ToArray();

            foreach (var module in apiDescription.Modules.Values)
            {
                foreach (var controller in module.Controllers.Values)
                {
                    if (!controller.Implements(serviceType))
                    {
                        continue;
                    }

                    foreach (var action in controller.Actions.Values)
                    {
                        if (action.Name == method.Name && action.ParametersOnMethod.Count == methodParameters.Length)
                        {
                            var found = true;

                            for (int i = 0; i < methodParameters.Length; i++)
                            {
                                if (action.ParametersOnMethod[i].TypeAsString != methodParameters[i].ParameterType.GetFullNameWithAssemblyName())
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
