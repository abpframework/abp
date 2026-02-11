using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;

namespace Volo.Abp.Internal.Telemetry.Activity.Providers;

[ExposeServices(typeof(ITelemetryActivityEventEnricher))]
internal sealed class TelemetryDeviceInfoEnricher : TelemetryActivityEventEnricher
{
    private readonly ITelemetryActivityStorage _telemetryActivityStorage;
    private readonly ISoftwareInfoProvider _softwareInfoProvider;

    public TelemetryDeviceInfoEnricher(ITelemetryActivityStorage telemetryActivityStorage,
        ISoftwareInfoProvider softwareInfoProvider, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _telemetryActivityStorage = telemetryActivityStorage;
        _softwareInfoProvider = softwareInfoProvider;
    }
    protected async override Task ExecuteAsync(ActivityContext context)
    {
        try
        {
            var deviceId = DeviceManager.GetUniquePhysicalKey(true);
            context.Current[ActivityPropertyNames.DeviceId] = deviceId;
            
            if (!_telemetryActivityStorage.ShouldAddDeviceInfo())
            {
                return;
            }
            
            var softwareList = await _softwareInfoProvider.GetSoftwareInfoAsync();
            
            context.Current[ActivityPropertyNames.InstalledSoftwares] = softwareList;
            context.Current[ActivityPropertyNames.DeviceLanguage] = CultureInfo.CurrentUICulture.Name;
            context.Current[ActivityPropertyNames.OperatingSystem] = GetOperatingSystem();
            context.Current[ActivityPropertyNames.CountryIsoCode] = GetCountry();
            context.Current[ActivityPropertyNames.HasDeviceInfo] = true;
            context.Current[ActivityPropertyNames.OperatingSystemArchitecture] = RuntimeInformation.OSArchitecture.ToString();
        }
        catch
        {
            //ignored
        }
    }

    private static OperationSystem GetOperatingSystem()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OperationSystem.Windows;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return OperationSystem.Linux;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OperationSystem.MacOS;
        }

        return OperationSystem.Unknown;
    }

    
  
    private static string GetCountry()
    {
        var region = new RegionInfo(CultureInfo.InstalledUICulture.Name);
        return region.TwoLetterISORegionName;
    }
}