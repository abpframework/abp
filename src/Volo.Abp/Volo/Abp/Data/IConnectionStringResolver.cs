namespace Volo.Abp.Data
{
    public interface IConnectionStringResolver
    {
        string Resolve(string connectionStringName = null);
    }
}
