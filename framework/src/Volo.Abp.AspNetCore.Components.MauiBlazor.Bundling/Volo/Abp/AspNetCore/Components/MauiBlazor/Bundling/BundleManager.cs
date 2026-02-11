using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Maui.Storage;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Scripts;
using Volo.Abp.AspNetCore.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Bundling;

public class BundleManager : BundleManagerBase, ITransientDependency
{
    protected IMauiBlazorContentFileProvider MauiBlazorContentFileProvider { get; }

    public BundleManager(
        IOptions<AbpBundlingOptions> options,
        IOptions<AbpBundleContributorOptions> contributorOptions,
        IScriptBundler scriptBundler,
        IStyleBundler styleBundler,
        IServiceProvider serviceProvider,
        IDynamicFileProvider dynamicFileProvider,
        IBundleCache bundleCache,
        IMauiBlazorContentFileProvider mauiBlazorContentFileProvider) : base(
        options,
        contributorOptions,
        scriptBundler,
        styleBundler,
        serviceProvider,
        dynamicFileProvider,
        bundleCache)
    {
        MauiBlazorContentFileProvider = mauiBlazorContentFileProvider;
    }

    public override bool IsBundlingEnabled()
    {
        switch (Options.Mode)
        {
            case BundlingMode.None:
                return false;
            case BundlingMode.Bundle:
            case BundlingMode.BundleAndMinify:
                return true;
            case BundlingMode.Auto:
                return !IsDebug();
            default:
                throw new AbpException($"Unhandled {nameof(BundlingMode)}: {Options.Mode}");
        }
    }

    protected async override Task<List<BundleFile>> GetBundleFilesAsync(List<IBundleContributor> contributors)
    {
        var files = await base.GetBundleFilesAsync(contributors);

        foreach (var file in files)
        {
            await CopyFileToAppDataDirectoryAsync(file);
        }

        return files;
    }

    protected virtual async Task CopyFileToAppDataDirectoryAsync(BundleFile file)
    {
        if (file.IsExternalFile)
        {
            return;
        }

        var fileName = Path.Combine("wwwroot", file.FileName);
        if(MauiBlazorContentFileProvider.GetFileInfo(fileName).Exists)
        {
            return;
        }

        try
        {
            await using var inputStream = await FileSystem.Current.OpenAppPackageFileAsync(fileName);
            var targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);
            var fileDirectory = Path.GetDirectoryName(targetFile)!;
            if (!Path.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            await using var outputStream = File.Create(targetFile);
            await inputStream.CopyToAsync(outputStream);
        }
        catch (Exception e)
        {
            Logger.LogError($"Could not copy the file to the app data directory: {fileName}", e);
        }
    }

    protected override bool IsMinficationEnabled()
    {
        switch (Options.Mode)
        {
            case BundlingMode.None:
            case BundlingMode.Bundle:
                return false;
            case BundlingMode.BundleAndMinify:
                return true;
            case BundlingMode.Auto:
                return !IsDebug();
            default:
                throw new AbpException($"Unhandled {nameof(BundlingMode)}: {Options.Mode}");
        }
    }

    protected virtual bool IsDebug()
    {
        #if DEBUG
                return true;
        #else
                return false;
        #endif
    }

    protected override IFileProvider GetFileProvider()
    {
        return MauiBlazorContentFileProvider;
    }
}
