using System;

namespace Pages.Abp.MultiTenancy
{
    public class FindTenantResult
    {
        public bool Success { get; set; }

        public Guid? TenantId { get; set; }
    }
}