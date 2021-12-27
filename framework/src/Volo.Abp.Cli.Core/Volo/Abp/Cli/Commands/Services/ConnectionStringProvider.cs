using System;
using System.IO;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class ConnectionStringProvider : ITransientDependency
{
    public string GetByDbms(DatabaseManagementSystem databaseManagementSystem, string outputFolder = "")
    {
        switch (databaseManagementSystem)
        {
            case DatabaseManagementSystem.NotSpecified:
            case DatabaseManagementSystem.SQLServer:
                return "Server=localhost;Database=MyProjectName;Trusted_Connection=True";
            case DatabaseManagementSystem.MySQL:
                return "Server=localhost;Port=3306;Database=MyProjectName;Uid=root;Pwd=myPassword;";
            case DatabaseManagementSystem.PostgreSQL:
                return "Host=localhost;Port=5432;Database=MyProjectName;User ID=root;Password=myPassword;";
            case DatabaseManagementSystem.Oracle:
            case DatabaseManagementSystem.OracleDevart:
                return "Data Source=MyProjectName;Integrated Security=yes;";
            case DatabaseManagementSystem.SQLite:
                var comment = outputFolder.IsNullOrWhiteSpace() ? "//You need to change to an absolute filename" : string.Empty;
                return $"Data Source={Path.Combine(outputFolder, "MyProjectName.db")};".Replace("\\", "\\\\") + comment;
            default:
                return null;
        }
    }
}
