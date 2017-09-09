using Volo.Abp.Application.Dtos;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.Application
{
    public class PhoneDto : EntityDto<long>
    {
        public string Number { get; set; }

        public PhoneType Type { get; set; }
    }
}