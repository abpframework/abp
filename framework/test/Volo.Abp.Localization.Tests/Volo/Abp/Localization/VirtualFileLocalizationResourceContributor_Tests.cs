using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Localization.VirtualFiles.Json;
using Volo.Abp.Testing;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.Localization;

public class VirtualFileLocalizationResourceContributor_Tests : AbpIntegratedTest<AbpLocalizationTestModule>
{
    [Fact]
    public void Initialize_Should_Not_Resolve_Services()
    {
        var serviceProvider = new RecordingServiceProvider(ServiceProvider);
        var resource = GetRequiredService<IOptions<AbpLocalizationOptions>>().Value.Resources.Get("LocalizationTestCountryNames");
        var contributor = new JsonVirtualFileLocalizationResourceContributor("/Volo/Abp/Localization/TestResources/Base/CountryNames");

        contributor.Initialize(new LocalizationResourceInitializationContext(resource, serviceProvider));

        serviceProvider.RequestedTypes.ShouldBeEmpty();

        contributor.GetOrNull("en", "USA").Value.ShouldBe("United States of America");
        serviceProvider.RequestedTypes.ShouldBe(new[] { typeof(IVirtualFileProvider) });
    }

    [Fact]
    public void The_First_Read_Should_Resolve_Services_Outside_Of_Its_Lock()
    {
        var resource = GetRequiredService<IOptions<AbpLocalizationOptions>>().Value.Resources.Get("LocalizationTestCountryNames");
        var contributor = new JsonVirtualFileLocalizationResourceContributor("/Volo/Abp/Localization/TestResources/Base/CountryNames");
        var serviceProvider = new ReadingWhileResolvingServiceProvider(ServiceProvider, contributor);
        contributor.Initialize(new LocalizationResourceInitializationContext(resource, serviceProvider));

        contributor.GetOrNull("en", "USA").Value.ShouldBe("United States of America");

        serviceProvider.OtherReadCompletedWhileResolving.ShouldBeTrue();
        serviceProvider.OtherRead.IsCompletedSuccessfully.ShouldBeTrue();
    }

    /* Reads the same resource from another thread while the first read resolves the virtual file provider, as a
     * container does when it creates a singleton that reads localized texts while it holds its own lock. */
    private class ReadingWhileResolvingServiceProvider : IServiceProvider
    {
        public Task OtherRead { get; private set; }

        public bool OtherReadCompletedWhileResolving { get; private set; }

        private readonly IServiceProvider _serviceProvider;
        private readonly JsonVirtualFileLocalizationResourceContributor _contributor;
        private int _otherReadStarted;

        public ReadingWhileResolvingServiceProvider(IServiceProvider serviceProvider, JsonVirtualFileLocalizationResourceContributor contributor)
        {
            _serviceProvider = serviceProvider;
            _contributor = contributor;
        }

        public object GetService(Type serviceType)
        {
            // The other read resolves the provider too, and may do so before OtherRead is assigned.
            if (serviceType == typeof(IVirtualFileProvider) && Interlocked.CompareExchange(ref _otherReadStarted, 1, 0) == 0)
            {
                var otherRead = Task.Run(() => _contributor.GetOrNull("en", "Brazil"));
                OtherRead = otherRead;
                OtherReadCompletedWhileResolving = otherRead.Wait(TimeSpan.FromSeconds(5));
            }

            return _serviceProvider.GetService(serviceType);
        }
    }

    private class RecordingServiceProvider : IServiceProvider
    {
        public List<Type> RequestedTypes { get; } = new();

        private readonly IServiceProvider _serviceProvider;

        public RecordingServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            RequestedTypes.Add(serviceType);
            return _serviceProvider.GetService(serviceType);
        }
    }
}
