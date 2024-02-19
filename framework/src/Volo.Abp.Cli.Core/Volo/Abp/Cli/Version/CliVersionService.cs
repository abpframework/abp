using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NuGet.Versioning;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Version;

public class CliVersionService : ITransientDependency
{
    private CmdHelper CmdHelper { get; }

    public CliVersionService(CmdHelper cmdHelper)
    {
        CmdHelper = cmdHelper;
    }

    public async Task<SemanticVersion> GetCurrentCliVersionAsync()
    {
        SemanticVersion currentCliVersion = default;

        var consoleOutput = new StringReader(CmdHelper.RunCmdAndGetOutput($"dotnet tool list -g", out int exitCode));
        string line;
        while ((line = await consoleOutput.ReadLineAsync()) != null)
        {
            if (line.StartsWith("volo.abp.cli", StringComparison.InvariantCultureIgnoreCase))
            {
                var version = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[1];

                SemanticVersion.TryParse(version, out currentCliVersion);

                break;
            }
        }

        if (currentCliVersion == null)
        {
            // If not a tool executable, fallback to assembly version and treat as dev without updates
            // Assembly revisions are not supported by SemVer scheme required for NuGet, trim to {major}.{minor}.{patch}
            var assemblyVersion = string.Join(".", Assembly.GetExecutingAssembly().GetFileVersion().Split('.').Take(3));
            return SemanticVersion.Parse(assemblyVersion + "-dev");
        }

        return currentCliVersion;
    }
}
