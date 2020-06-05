namespace Volo.Abp.Data
{
    public class AbpDbConnectionOptions
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public AbpDbConnectionOptions()
        {
            ConnectionStrings = new ConnectionStrings();
        }
    }
}
