using JetBrains.Annotations;

namespace Volo.Abp.Data
{
    public interface IConnectionStringResolver
    {
        [NotNull]
        string Resolve(string connectionStringName = null);
    }
}
