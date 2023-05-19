namespace Volo.Abp.Cli.Build;

public interface IDotNetProjectBuildConfigReader
{
    DotNetProjectBuildConfig Read(string directoryPath);
}
