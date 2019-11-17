namespace Volo.Abp.MultiTenancy
{
    public class AbpMultiTenancyOptions
    {
        /// <summary>
        /// A central point to enable/disable multi-tenancy.
        /// Default: false. 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Database style for tenants.
        /// Default: <see cref="MultiTenancyDatabaseStyle.Hybrid"/>.
        /// </summary>
        public MultiTenancyDatabaseStyle DatabaseStyle { get; set; } = MultiTenancyDatabaseStyle.Hybrid;
    }
}
