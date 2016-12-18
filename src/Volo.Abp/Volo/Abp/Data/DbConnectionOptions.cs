namespace Volo.Abp.Data
{
    public class DbConnectionOptions
    {
        public ConnectionStringsDictionary ConnectionStrings { get; set; }

        public DbConnectionOptions()
        {
            ConnectionStrings = new ConnectionStringsDictionary();
        }
    }
}
