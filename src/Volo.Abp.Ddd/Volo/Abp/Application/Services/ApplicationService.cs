using System.Collections.Generic;
using Volo.Abp.Aspects;

namespace Volo.Abp.Application.Services
{
    public abstract class ApplicationService : AbpServiceBase, IApplicationService, IAvoidDuplicateCrossCuttingConcerns
    {
        public static string[] CommonPostfixes { get; set; } = { "AppService", "ApplicationService", "Service" };

        /// <summary>
        /// Gets the applied cross cutting concerns.
        /// </summary>
        public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

        /* Will be added when implemented
          - AbpSession
          - ...
         */
    }
}