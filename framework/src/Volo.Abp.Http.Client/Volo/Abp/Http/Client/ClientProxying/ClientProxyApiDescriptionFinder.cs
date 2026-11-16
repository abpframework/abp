using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Http.Client.ClientProxying;

public class ClientProxyApiDescriptionFinder : IClientProxyApiDescriptionFinder, ISingletonDependency
{
    protected IVirtualFileProvider VirtualFileProvider { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected ConcurrentDictionary<string, ActionApiDescriptionModel> ActionApiDescriptionModels { get; }
    protected ApplicationApiDescriptionModel ApplicationApiDescriptionModel { get; set; } = default!;

    private readonly List<IFileInfo> _pendingFiles = new();
    private readonly Dictionary<IFileInfo, List<string>> _fileServiceTypes = new();
    private readonly object _syncObj = new();

    public ClientProxyApiDescriptionFinder(
        IVirtualFileProvider virtualFileProvider,
        IJsonSerializer jsonSerializer)
    {
        VirtualFileProvider = virtualFileProvider;
        JsonSerializer = jsonSerializer;
        ActionApiDescriptionModels = new ConcurrentDictionary<string, ActionApiDescriptionModel>();

        Initialize();
    }

    public virtual ActionApiDescriptionModel? FindAction(string methodName)
    {
        if (ActionApiDescriptionModels.TryGetValue(methodName, out var model))
        {
            return model;
        }

        lock (_syncObj)
        {
            if (ActionApiDescriptionModels.TryGetValue(methodName, out model))
            {
                return model;
            }

            var fileInfos = _pendingFiles
                .Where(x => GetServiceTypes(x).Any(serviceType => methodName.StartsWith(serviceType + ".", StringComparison.Ordinal)))
                .ToList();

            LoadFiles(fileInfos);
            if (ActionApiDescriptionModels.TryGetValue(methodName, out model))
            {
                return model;
            }

            LoadFiles(_pendingFiles.ToList());
            return ActionApiDescriptionModels.TryGetValue(methodName, out model) ? model : null;
        }
    }

    public virtual ApplicationApiDescriptionModel GetApiDescription()
    {
        lock (_syncObj)
        {
            LoadFiles(_pendingFiles.ToList());
            return ApplicationApiDescriptionModel;
        }
    }

    protected virtual void Initialize()
    {
        ApplicationApiDescriptionModel = ApplicationApiDescriptionModel.Create();
        GetGenerateProxyFileInfos(_pendingFiles);
    }

    protected virtual void GetGenerateProxyFileInfos(List<IFileInfo> fileInfoList, string path = "")
    {
        foreach (var fileInfo in VirtualFileProvider.GetDirectoryContents(path))
        {
            if (fileInfo.IsDirectory)
            {
                GetGenerateProxyFileInfos(fileInfoList, path + fileInfo.Name.EnsureStartsWith('/'));
            }
            else
            {
                if (fileInfo.Name.EndsWith("generate-proxy.json", StringComparison.OrdinalIgnoreCase))
                {
                    fileInfoList.Add(fileInfo);
                }
            }
        }
    }

    protected virtual void LoadFiles(List<IFileInfo> fileInfos)
    {
        foreach (var fileInfo in fileInfos)
        {
            var subApplicationApiDescription = JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(ReadFile(fileInfo));
            var modules = subApplicationApiDescription.Modules
                .Where(x => !ApplicationApiDescriptionModel.Modules.ContainsKey(x.Key))
                .Select(x => x.Value)
                .ToList();

            var actions = new List<KeyValuePair<string, ActionApiDescriptionModel>>();
            foreach (var controller in modules.SelectMany(x => x.Controllers.Values).Where(x => x.Interfaces.Any()))
            {
                var appServiceType = controller.Interfaces.Last().Type;

                foreach (var actionItem in controller.Actions.Values)
                {
                    var actionKey = $"{appServiceType}.{actionItem.Name}.{string.Join("-", actionItem.ParametersOnMethod.Select(x => x.Type))}";
                    actions.Add(new KeyValuePair<string, ActionApiDescriptionModel>(actionKey, actionItem));
                }
            }

            foreach (var module in modules)
            {
                ApplicationApiDescriptionModel.AddModule(module);
            }

            foreach (var action in actions)
            {
                ActionApiDescriptionModels.TryAdd(action.Key, action.Value);
            }

            _pendingFiles.Remove(fileInfo);
            _fileServiceTypes.Remove(fileInfo);
        }
    }

    protected virtual string ReadFile(IFileInfo fileInfo)
    {
        using (var streamReader = new StreamReader(fileInfo.CreateReadStream()))
        {
            return streamReader.ReadToEnd();
        }
    }

    private List<string> GetServiceTypes(IFileInfo fileInfo)
    {
        if (!_fileServiceTypes.TryGetValue(fileInfo, out var serviceTypes))
        {
            try
            {
                // Deserializing only the interfaces of the controllers costs much less than deserializing the whole file.
                var file = JsonSerializer.Deserialize<GenerateProxyFile>(ReadFile(fileInfo));
                serviceTypes = (file.Modules?.Values ?? Enumerable.Empty<GenerateProxyModule>())
                    .SelectMany(x => x.Controllers?.Values ?? Enumerable.Empty<GenerateProxyController>())
                    .Select(x => x.Interfaces?.LastOrDefault()?.Type)
                    .OfType<string>()
                    .ToList();
            }
            catch (Exception)
            {
                // For example, the JsonSerializer may not allow the members left out here. FindAction still loads
                // this file when it doesn't find the action in the other files.
                serviceTypes = new List<string>();
            }

            _fileServiceTypes[fileInfo] = serviceTypes;
        }

        return serviceTypes;
    }

    private class GenerateProxyFile
    {
        public Dictionary<string, GenerateProxyModule>? Modules { get; set; }
    }

    private class GenerateProxyModule
    {
        public Dictionary<string, GenerateProxyController>? Controllers { get; set; }
    }

    private class GenerateProxyController
    {
        public List<GenerateProxyInterface>? Interfaces { get; set; }
    }

    private class GenerateProxyInterface
    {
        public string? Type { get; set; }
    }
}
