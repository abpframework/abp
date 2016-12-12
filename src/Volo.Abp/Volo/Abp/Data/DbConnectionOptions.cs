namespace Volo.Abp.Data
{
    public class DbConnectionOptions
    {
        public ConnectionStringsOption ConnectionStrings { get; set; }

        public DbConnectionOptions()
        {
            ConnectionStrings = new ConnectionStringsOption();
        }
    }
}
