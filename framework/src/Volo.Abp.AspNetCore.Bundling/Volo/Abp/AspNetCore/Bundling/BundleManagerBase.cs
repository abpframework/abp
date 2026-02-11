using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling.Scripts;
using Volo.Abp.AspNetCore.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Bundling;

public abstract class BundleManagerBase : IBundleManager
{
    public ILogger<BundleManagerBase> Logger { get; set; }

    protected readonly AbpBundlingOptions Options;
    protected readonly AbpBundleContributorOptions ContributorOptions;
    protected readonly IScriptBundler ScriptBundler;
    protected readonly IStyleBundler StyleBundler;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IDynamicFileProvider DynamicFileProvider;
    protected readonly IBundleCache BundleCache;

    public BundleManagerBase(
        IOptions<AbpBundlingOptions> options,
        IOptions<AbpBundleContributorOptions> contributorOptions,
        IScriptBundler scriptBundler,
        IStyleBundler styleBundler,
        IServiceProvider serviceProvider,
        IDynamicFileProvider dynamicFileProvider,
        IBundleCache bundleCache)
    {
        Options = options.Value;
        ContributorOptions = contributorOptions.Value;
        ScriptBundler = scriptBundler;
        ServiceProvider = serviceProvider;
        DynamicFileProvider = dynamicFileProvider;
        BundleCache = bundleCache;
        StyleBundler = styleBundler;

        Logger = NullLogger<BundleManagerBase>.Instance;
    }

    public virtual async Task<IReadOnlyList<BundleFile>> GetStyleBundleFilesAsync(string bundleName)
    {
        return await GetBundleFilesAsync(Options.StyleBundles, bundleName, StyleBundler);
    }

    public virtual async Task<IReadOnlyList<BundleFile>> GetScriptBundleFilesAsync(string bundleName)
    {
        return await GetBundleFilesAsync(Options.ScriptBundles, bundleName, ScriptBundler);
    }

    protected virtual async Task<IReadOnlyList<BundleFile>> GetBundleFilesAsync(BundleConfigurationCollection bundles,
        string bundleName, IBundler bundler)
    {
        var files = new List<BundleFile>();

        var contributors = GetContributors(bundles, bundleName);
        var bundleFiles = await GetBundleFilesAsync(contributors);
        var dynamicResources = await GetDynamicResourcesAsync(contributors);

        if (!IsBundlingEnabled())
        {
            return bundleFiles.Union(dynamicResources).ToImmutableList();
        }

        var localBundleFiles = new List<string>();
        foreach (var bundleFile in bundleFiles)
        {
            if (!bundleFile.IsExternalFile)
            {
                localBundleFiles.Add(bundleFile.FileName);
            }
            else
            {
                if (localBundleFiles.Count != 0)
                {
                    files.AddRange(AddToBundleCache(bundleName, bundler, localBundleFiles).Files);
                    localBundleFiles.Clear();
                }

                files.Add(bundleFile);
            }
        }

        if (localBundleFiles.Count != 0)
        {
            files.AddRange(AddToBundleCache(bundleName, bundler, localBundleFiles).Files);
        }

        return files.Union(dynamicResources).ToImmutableList();
    }

    private BundleCacheItem AddToBundleCache(string bundleName, IBundler bundler, List<string> bundleFiles)
    {
        var bundleRelativePath =
            Options.BundleFolderName.EnsureEndsWith('/') +
            bundleName + "." + bundleFiles.JoinAsString("|").ToMd5() + "." + bundler.FileExtension;

        return BundleCache.GetOrAdd(bundleRelativePath, () =>
        {
            var cacheValue = new BundleCacheItem(
                new List<BundleFile> { new BundleFile("/" + bundleRelativePath) }
            );

            WatchChanges(cacheValue, bundleFiles, bundleRelativePath);

            var bundleResult = bundler.Bundle(
                new BundlerContext(
                    bundleRelativePath,
                    bundleFiles,
                    IsMinficationEnabled()
                )
            );

            SaveBundleResult(bundleRelativePath, bundleResult);

            return cacheValue;
        });
    }

    private void WatchChanges(BundleCacheItem cacheValue, List<string> files, string bundleRelativePath)
    {
        lock (cacheValue.WatchDisposeHandles)
        {
            foreach (var file in files)
            {
                var watchDisposeHandle = GetFileProvider().Watch(file).RegisterChangeCallback(_ =>
                {
                    lock (cacheValue.WatchDisposeHandles)
                    {
                        cacheValue.WatchDisposeHandles.ForEach(h => h.Dispose());
                        cacheValue.WatchDisposeHandles.Clear();
                    }

                    BundleCache.Remove(bundleRelativePath);
                    DynamicFileProvider.Delete("/wwwroot/" + bundleRelativePath); //TODO: get rid of wwwroot!
                }, null);

                cacheValue.WatchDisposeHandles.Add(watchDisposeHandle);
            }
        }
    }

    protected virtual void SaveBundleResult(string bundleRelativePath, BundleResult bundleResult)
    {
        var fileName = bundleRelativePath.Substring(bundleRelativePath.IndexOf('/') + 1);

        DynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(
                "/wwwroot/" + bundleRelativePath, //TODO: get rid of wwwroot!
                Encoding.UTF8.GetBytes(bundleResult.Content),
                fileName
            )
        );
    }

    public abstract bool IsBundlingEnabled();


    protected abstract bool IsMinficationEnabled();

    protected virtual async Task<List<BundleFile>> GetBundleFilesAsync(List<IBundleContributor> contributors)
    {
        var context = CreateBundleConfigurationContext();

        foreach (var contributor in contributors)
        {
            await contributor.PreConfigureBundleAsync(context);
        }

        foreach (var contributor in contributors)
        {
            await contributor.ConfigureBundleAsync(context);
        }

        foreach (var contributor in contributors)
        {
            await contributor.PostConfigureBundleAsync(context);
        }

        return context.Files;
    }

    protected virtual async Task<List<BundleFile>> GetDynamicResourcesAsync(List<IBundleContributor> contributors)
    {
        var context = CreateBundleConfigurationContext();

        foreach (var contributor in contributors)
        {
            await contributor.ConfigureDynamicResourcesAsync(context);
        }

        return context.Files;
    }

    protected virtual BundleConfigurationContext CreateBundleConfigurationContext()
    {
        return new BundleConfigurationContext(ServiceProvider, GetFileProvider(),
            Options.Parameters);
    }

    protected abstract IFileProvider GetFileProvider();

    protected virtual List<IBundleContributor> GetContributors(BundleConfigurationCollection bundles, string bundleName)
    {
        var contributors = new List<IBundleContributor>();

        AddContributorsWithBaseBundles(contributors, bundles, bundleName);

        for (var i = 0; i < contributors.Count; ++i)
        {
            var extensions = ContributorOptions.Extensions(contributors[i].GetType()).GetAll();
            if (extensions.Count > 0)
            {
                contributors.InsertRange(i + 1, extensions);
                i += extensions.Count;
            }
        }

        return contributors;
    }

    protected virtual void AddContributorsWithBaseBundles(List<IBundleContributor> contributors,
        BundleConfigurationCollection bundles, string bundleName)
    {
        var bundleConfiguration = bundles.Get(bundleName);

        foreach (var baseBundleName in bundleConfiguration.BaseBundles)
        {
            AddContributorsWithBaseBundles(contributors, bundles, baseBundleName); //Recursive call
        }

        var selfContributors = bundleConfiguration.Contributors.GetAll();

        if (selfContributors.Any())
        {
            contributors.AddRange(selfContributors);
        }
    }
}