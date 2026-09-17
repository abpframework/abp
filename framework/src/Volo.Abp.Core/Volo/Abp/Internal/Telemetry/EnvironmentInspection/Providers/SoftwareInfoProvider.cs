using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Providers;

internal class SoftwareInfoProvider : ISoftwareInfoProvider , ISingletonDependency
{
    private readonly IEnumerable<ISoftwareDetector> _softwareDetectors;

    public SoftwareInfoProvider(IEnumerable<ISoftwareDetector> softwareDetectors)
    {
        _softwareDetectors = softwareDetectors;
    }

    public async Task<List<SoftwareInfo>> GetSoftwareInfoAsync()
    {
        var result = new List<SoftwareInfo>();

        foreach (var softwareDetector in _softwareDetectors)
        {
            try
            {
                var softwareInfo = await softwareDetector.DetectAsync();
                if (softwareInfo is not null)
                {
                    result.Add(softwareInfo);
                }
            }
            catch  
            {
                //ignored
            }
        }
        return result;
    }
  

}