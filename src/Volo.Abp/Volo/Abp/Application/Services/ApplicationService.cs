using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services
{
    public abstract class ApplicationService : IApplicationService
    {
        public IObjectMapper ObjectMapper { get; set; }
    }
}