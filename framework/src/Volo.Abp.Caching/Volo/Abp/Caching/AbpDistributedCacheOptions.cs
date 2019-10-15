namespace Volo.Abp.Caching
{
    public class AbpDistributedCacheOptions
    {
        /// <summary>
        /// Throw or hide exceptions for the distributed cache.
        /// </summary>
        public bool HideErrors { get; set; } = true;
    }
}
