using Volo.Abp.Application.Dtos;

namespace Volo.Abp.TestApp.Application.Dto
{
    public class PersonDto : EntityDto
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
