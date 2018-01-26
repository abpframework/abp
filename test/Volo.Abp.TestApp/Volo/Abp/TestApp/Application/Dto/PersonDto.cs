using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.TestApp.Application.Dto
{
    public class PersonDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
