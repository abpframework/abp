using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class ApiDescriptionFinder : IApiDescriptionFinder, ITransientDependency
    {
        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        protected IApiDescriptionCache Cache { get; }

        public ApiDescriptionFinder(IApiDescriptionCache cache)
        {
            Cache = cache;
            CancellationTokenProvider = NullCancellationTokenProvider.Instance;
        }

        public async Task<ActionApiDescriptionModel> FindActionAsync(HttpClient client, string baseUrl, Type serviceType, MethodInfo method)
        {
            var apiDescription = await GetApiDescriptionAsync(client, baseUrl);

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
                                if (!TypeMatches(action.ParametersOnMethod[i], methodParameters[i]))
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

            throw new AbpException($"Could not found remote action for method: {method} on the URL: {baseUrl}");
        }

        public virtual async Task<ApplicationApiDescriptionModel> GetApiDescriptionAsync(HttpClient client, string baseUrl)
        {
            return await Cache.GetAsync(baseUrl, () => GetApiDescriptionFromServerAsync(client, baseUrl));
        }

        protected virtual async Task<ApplicationApiDescriptionModel> GetApiDescriptionFromServerAsync(HttpClient client, string baseUrl)
        {
            var response = await client.GetAsync(
                baseUrl.EnsureEndsWith('/') + "api/abp/api-definition",
                CancellationTokenProvider.Token
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new AbpException("Remote service returns error! StatusCode = " + response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return (ApplicationApiDescriptionModel)result;
        }

        protected virtual bool TypeMatches(MethodParameterApiDescriptionModel actionParameter, ParameterInfo methodParameter)
        {
            return NormalizeTypeName(actionParameter.TypeAsString) ==
                   NormalizeTypeName(methodParameter.ParameterType.GetFullNameWithAssemblyName());
        }

        protected virtual string NormalizeTypeName(string typeName)
        {
            const string placeholder = "%COREFX%";
            const string netCoreLib = "System.Private.CoreLib";
            const string netFxLib = "mscorlib";

            return typeName.Replace(netCoreLib, placeholder).Replace(netFxLib, placeholder);
        }
    }
}
