using System;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class AngularThemeConfigurer : ITransientDependency
{
    private readonly ICmdHelper _cmdHelper;

    public AngularThemeConfigurer(ICmdHelper cmdHelper)
    {
        _cmdHelper = cmdHelper;
    }

    public void Configure(AngularThemeConfigurationArgs args)
    {
        if (args.ProjectName.IsNullOrEmpty() || args.AngularFolderPath.IsNullOrEmpty())
        {
            return;
        }
        
        var command = "npx ng g @abp/ng.schematics:change-theme " +
                      $"--name {(int)args.Theme} " +
                      $"--target-project {args.ProjectName}";
        
        _cmdHelper.RunCmd(command, workingDirectory: args.AngularFolderPath);
    }
}