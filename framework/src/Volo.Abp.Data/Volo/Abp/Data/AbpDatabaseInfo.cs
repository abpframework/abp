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
    
    /// <summary>
    /// Shortcut method to add one or multiple connections to the <see cref="MappedConnections"/> collection.
    /// </summary>
    /// <param name="connectionNames"></param>
    public void MapConnection(params string[] connectionNames)
    {
        foreach (var connectionName in connectionNames)
        {
            MappedConnections.AddIfNotContains(connectionName);
        }
    }
}
