using Microsoft.Extensions.Logging;

namespace Volo.Abp.Cli.Commands
{
    public static class AddCommandHelper
    {
        public static void WriteUsage(ILogger logger)
        {
            logger.LogWarning("");
            logger.LogWarning("'add' command is used to add a module to a solution or to a project.");
            logger.LogWarning("Use in a solution folder to add a multi-package module to a solution.");
            logger.LogWarning("Use in a project folder to add a single-package module to a project.");
            logger.LogWarning("");
            logger.LogWarning("Usage:");
            logger.LogWarning("  abp add <module-name> [-s|--solution] [-p|--project]");
            logger.LogWarning("");
            logger.LogWarning("Options:");
            logger.LogWarning("  -s|--solution <solution-file>  Specify solution file explicitly.");
            logger.LogWarning("  -p|--project <project-file>    Specify project file explicitly.");
            logger.LogWarning("");
            logger.LogWarning("Examples:");
            logger.LogWarning("  abp add Volo.Abp.TenantManagement   Adds tenant management module packages to the solution.");
            logger.LogWarning("  abp add Volo.Abp.FluentValidation   Adds a single module package to the project.");
            logger.LogWarning("");
        }
    }
}