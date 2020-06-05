using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TestApp.Application.Dto
{
    public class PersonDto : EntityDto<Guid>, IMultiTenant
    {
        [Required]
        public string Name { get; set; }

        public int Age { get; set; }

        public Guid? TenantId { get; set; }
    }
}
