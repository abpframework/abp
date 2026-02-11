using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Scripts;
using Volo.Abp.AspNetCore.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Scripts;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleManager : BundleManagerBase, ITransientDependency
{
    public BundleManager(
        IOptions<AbpBundlingOptions> options,
        IOptions<AbpBundleContributorOptions> contributorOptions,
        IScriptBundler scriptBundler, 
        IStyleBundler styleBundler,
        IServiceProvider serviceProvider,
        IDynamicFileProvider dynamicFileProvider,
        IBundleCache bundleCache,
        IWebHostEnvironment hostingEnvironment, 
        IWebRequestResources requestResources) : base(
        options, 
        contributorOptions,
        scriptBundler, 
        styleBundler, 
        serviceProvider,
        dynamicFileProvider, 
        bundleCache)
    {
        HostingEnvironment = hostingEnvironment;
        RequestResources = requestResources;
    }

    protected IWebHostEnvironment HostingEnvironment { get; }

    protected IWebRequestResources RequestResources { get; }



    protected async override Task<List<BundleFile>> GetBundleFilesAsync(List<IBundleContributor> contributors)
    {
        return RequestResources.TryAdd(await base.GetBundleFilesAsync(contributors));
    }

    protected async override Task<List<BundleFile>> GetDynamicResourcesAsync(List<IBundleContributor> contributors)
    {
        return RequestResources.TryAdd(await base.GetDynamicResourcesAsync(contributors));
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
                return !HostingEnvironment.IsDevelopment();
            default:
                throw new AbpException($"Unhandled {nameof(BundlingMode)}: {Options.Mode}");
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
                return !HostingEnvironment.IsDevelopment();
            default:
                throw new AbpException($"Unhandled {nameof(BundlingMode)}: {Options.Mode}");
        }
    }

    protected override IFileProvider GetFileProvider()
    {
        return HostingEnvironment.WebRootFileProvider;
    }
}