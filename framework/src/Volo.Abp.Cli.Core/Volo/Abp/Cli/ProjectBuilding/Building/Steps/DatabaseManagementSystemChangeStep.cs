using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Text;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class DatabaseManagementSystemChangeStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            switch (context.BuildArgs.DatabaseManagementSystem)
            {
                    case DatabaseManagementSystem.NotSpecified:
                    return;

                    case DatabaseManagementSystem.SQLServer:
                    return;

                    case DatabaseManagementSystem.MySQL:
                    return;

                    case DatabaseManagementSystem.PostgreSQL:
                    return;

                    case DatabaseManagementSystem.Oracle:
                    return;

                    case DatabaseManagementSystem.SQLite:
                    return;
            }
        }
    }
}
