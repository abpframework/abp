using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Http.Client
{
    public class ClientProxyBase : ITransientDependency
    {
        public const string ApiDescriptionCacheKey = "client-proxy";
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        protected IHttpProxyExecuter HttpProxyExecuter => LazyServiceProvider.LazyGetRequiredService<IHttpProxyExecuter>();
        protected IJsonSerializer JsonSerializer => LazyServiceProvider.LazyGetRequiredService<IJsonSerializer>();
        protected IApiDescriptionCache ApiDescriptionCache => LazyServiceProvider.LazyGetRequiredService<ApiDescriptionCache>();
        protected IVirtualFileProvider VirtualFileProvider => LazyServiceProvider.LazyGetRequiredService<VirtualFileProvider>();

        protected static readonly Dictionary<string, ActionApiDescriptionModel> ActionApiDescriptionModels = new Dictionary<string, ActionApiDescriptionModel>();

        private static object SyncLock = new object();

        protected virtual async Task MakeRequestAsync<TService>(string methodName, params object[] arguments)
        {
            await HttpProxyExecuter.MakeRequestAsync(await BuildHttpProxyExecuterContext<TService>(methodName, arguments));
        }

        protected virtual async Task<T> MakeRequestAsync<TService,T>(string methodName, params object[] arguments)
        {
            return await HttpProxyExecuter.MakeRequestAndGetResultAsync<T>(await BuildHttpProxyExecuterContext<TService>(methodName, arguments));
        }

        protected virtual async Task<HttpProxyExecuterContext> BuildHttpProxyExecuterContext<TService>(string methodName, params object[] arguments)
        {
            var actionDescriptionKey = $"{typeof(TService).Name}.{methodName}";

            if (!ActionApiDescriptionModels.ContainsKey(actionDescriptionKey))
            {
                var apiDescription = await ApiDescriptionCache.GetAsync(ApiDescriptionCacheKey, GetApplicationApiDescriptionModel);
                var controllers = apiDescription.Modules.Select(x=>x.Value).SelectMany(x => x.Controllers.Values).ToList();

                lock (SyncLock)
                {
                    foreach (var controller in controllers.Where(x => x.Interfaces.Any()))
                    {
                        var appServiceType = controller.Interfaces.Last().Type.Split('.').Last();

                        foreach (var actionItem in controller.Actions.Values)
                        {
                            if (!ActionApiDescriptionModels.ContainsKey($"{appServiceType}.{actionItem.Name}"))
                            {
                                ActionApiDescriptionModels.Add($"{appServiceType}.{actionItem.Name}", actionItem);
                            }
                        }
                    }
                }
            }

            var action = ActionApiDescriptionModels[actionDescriptionKey];

            return new HttpProxyExecuterContext(action, BuildArguments(action, arguments), typeof(TService));
        }

        protected virtual Dictionary<string, object> BuildArguments(ActionApiDescriptionModel action, object[] arguments)
        {
            var parameters = action.Parameters.GroupBy(x => x.NameOnMethod).Select(x => x.Key).ToList();
            var dict = new Dictionary<string, object>();

            for (var i = 0; i < parameters.Count; i++)
            {
                dict[parameters[i]] = arguments[i];
            }

            return dict;
        }

        protected virtual async Task<ApplicationApiDescriptionModel> GetApplicationApiDescriptionModel()
        {
            var applicationApiDescription = ApplicationApiDescriptionModel.Create();

            var fileInfoList = new List<IFileInfo>();
            GetGenerateProxyFileInfos(fileInfoList);

            foreach (var fileInfo in fileInfoList)
            {
                using (var streamReader = new StreamReader(fileInfo.CreateReadStream()))
                {
                    var content = await streamReader.ReadToEndAsync();

                    var subApplicationApiDescription = JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(content);

                    foreach (var module in subApplicationApiDescription.Modules)
                    {
                        if (!applicationApiDescription.Modules.ContainsKey(module.Key))
                        {
                            applicationApiDescription.AddModule(module.Value);
                        }
                    }
                }
            }

            return applicationApiDescription;
        }

        private void GetGenerateProxyFileInfos(List<IFileInfo> fileInfoList, string path = "")
        {
            foreach (var directoryContent in VirtualFileProvider.GetDirectoryContents(path))
            {
                if (directoryContent.IsDirectory)
                {
                    GetGenerateProxyFileInfos(fileInfoList, directoryContent.PhysicalPath);
                }
                else
                {
                    if (directoryContent.Name.EndsWith("generate-proxy.json"))
                    {
                        fileInfoList.Add(VirtualFileProvider.GetFileInfo(directoryContent.GetVirtualOrPhysicalPathOrNull()));
                    }
                }
            }
        }
    }
}
