namespace Volo.Abp.Application.Services
{
    public abstract class ApplicationService : AbpServiceBase, IApplicationService
    {
        public static string[] CommonPostfixes = { "AppService", "ApplicationService", "Service" };

        /* Will be added when implemented
          - AbpSession
          - ...
         */
    }
}