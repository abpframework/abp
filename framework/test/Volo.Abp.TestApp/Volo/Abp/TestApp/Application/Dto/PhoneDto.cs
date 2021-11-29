using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.Application.Dto
{
    public class PhoneDto
    {
        public string Number { get; set; }

        public PhoneType Type { get; set; }
    }
}