using System.Collections.Generic;

namespace Volo.Abp.Data;

public class AbpDatabaseInfo
{
    public string DatabaseName { get; }

    /// <summary>
    /// List of connection names mapped to this database.
    /// </summary>
    public HashSet<string> MappedConnections { get; }

    /// <summary>
    /// Is this database used by tenants. Set this to false if this database
    /// can not owned by tenants.
    /// 
    /// Default: true.
    /// </summary>
    public bool IsUsedByTenants { get; set; } = true;

    internal AbpDatabaseInfo(string databaseName)
    {
        DatabaseName = databaseName;
        MappedConnections = new HashSet<string>();
    }
}
