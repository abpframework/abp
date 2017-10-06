using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity.HttpApi.Host.VersioningTests.V1
{
    public class CallDto : EntityDto<int>
    {
        public string Number { get; set; }
    }
}