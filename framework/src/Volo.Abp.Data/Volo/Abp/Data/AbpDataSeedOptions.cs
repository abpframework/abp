namespace Volo.Abp.Data
{
    public class AbpDataSeedOptions
    {
        public DataSeedContributorList Contributors { get; }

        public AbpDataSeedOptions()
        {
            Contributors = new DataSeedContributorList();
        }
    }
}
