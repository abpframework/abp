using System;

namespace Volo.Abp.AspNetCore.Mvc.MultiTenancy
{
    public class FindTenantResult
    {
        public bool Success { get; set; }

        public Guid? TenantId { get; set; }

        public string Name { get; set; }
    }
}