using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public class ClientProxyApiDescriptionFinder : IClientProxyApiDescriptionFinder, ISingletonDependency
    {
        protected IVirtualFileProvider VirtualFileProvider { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected Dictionary<string, ActionApiDescriptionModel> ActionApiDescriptionModels { get; }
        protected ApplicationApiDescriptionModel ApplicationApiDescriptionModel { get; set; }

        public ClientProxyApiDescriptionFinder(
            IVirtualFileProvider virtualFileProvider,
            IJsonSerializer jsonSerializer)
        {
            VirtualFileProvider = virtualFileProvider;
            JsonSerializer = jsonSerializer;
            ActionApiDescriptionModels = new Dictionary<string, ActionApiDescriptionModel>();

            Initial();
        }

        public Task<ActionApiDescriptionModel> FindActionAsync(string action)
        {
            return Task.FromResult(ActionApiDescriptionModels[action]);
        }

        public Task<ApplicationApiDescriptionModel> GetApiDescriptionAsync()
        {
            return Task.FromResult(ApplicationApiDescriptionModel);
        }

        private void Initial()
        {
            ApplicationApiDescriptionModel = GetApplicationApiDescriptionModel();
            var controllers = ApplicationApiDescriptionModel.Modules.Select(x=>x.Value).SelectMany(x => x.Controllers.Values).ToList();

            foreach (var controller in controllers.Where(x => x.Interfaces.Any()))
            {
                var appServiceType = controller.Interfaces.Last().Type;

                foreach (var actionItem in controller.Actions.Values)
                {
                    if (!ActionApiDescriptionModels.ContainsKey($"{appServiceType}.{actionItem.Name}"))
                    {
                        ActionApiDescriptionModels.Add($"{appServiceType}.{actionItem.Name}", actionItem);
                    }
                }
            }
        }

        private ApplicationApiDescriptionModel GetApplicationApiDescriptionModel()
        {
            var applicationApiDescription = ApplicationApiDescriptionModel.Create();
            var fileInfoList = new List<IFileInfo>();
            GetGenerateProxyFileInfos(fileInfoList);

            foreach (var fileInfo in fileInfoList)
            {
                using (var streamReader = new StreamReader(fileInfo.CreateReadStream()))
                {
                    var content = streamReader.ReadToEnd();

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
