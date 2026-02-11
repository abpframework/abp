using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;

internal interface ISoftwareInfoProvider
{
    Task<List<SoftwareInfo>> GetSoftwareInfoAsync();
}