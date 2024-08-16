using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class DotnetEfToolManager : ISingletonDependency
{
    public ICmdHelper CmdHelper { get; }
    public ILogger<DotnetEfToolManager> Logger { get; set; }

    public DotnetEfToolManager(ICmdHelper cmdHelper)
    {
        CmdHelper = cmdHelper;
        
        Logger = NullLogger<DotnetEfToolManager>.Instance;
    }

    public async Task BeSureInstalledAsync()
    {
        if (!IsDotNetEfToolInstalled())
        {
            InstallDotnetEfTool();
        }        
    }
    
    private bool IsDotNetEfToolInstalled()
    {
        var output = CmdHelper.RunCmdAndGetOutput("dotnet tool list -g");
        return output.Contains("dotnet-ef");
    }

    private void InstallDotnetEfTool()
    {
        Logger.LogInformation("Installing dotnet-ef tool...");
        CmdHelper.RunCmd("dotnet tool install --global dotnet-ef");
        Logger.LogInformation("dotnet-ef tool is installed.");
    }
}