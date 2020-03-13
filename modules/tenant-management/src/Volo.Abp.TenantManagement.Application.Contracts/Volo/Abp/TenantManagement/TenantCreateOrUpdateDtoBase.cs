namespace Volo.Abp.TenantManagement
{
    public abstract class TenantCreateOrUpdateDtoBase
    {
        public string Name { get; set; }

        public string AdminEmailAddress { get; set; }

        public string AdminPassword { get; set; }
    }
}