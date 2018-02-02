using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.MultiTenancy
{
    public class TenantDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}