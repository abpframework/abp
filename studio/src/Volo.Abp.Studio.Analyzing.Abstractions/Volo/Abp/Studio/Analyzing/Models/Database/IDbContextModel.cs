namespace Volo.Abp.Studio.Analyzing.Models.Database
{
    public interface IDbContextModel
    {
        string ConnectionStringName { get; set; }
    }
}