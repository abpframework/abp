using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.TenantManagement
{
    public class TenantDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}