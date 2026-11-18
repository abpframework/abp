using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Internal.Telemetry;
using Xunit;

namespace Volo.Abp.Cli.Commands;

public class AddPackageCommand_Tests : AbpCliTestBase
{
    private static readonly string WorkingDirectory = Path.Combine(Path.GetTempPath(), "react-app");
    private static readonly string AngularDirectory = Path.Combine(Path.GetTempPath(), "angular-app");

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddTransient<TestAddPackageCommand>();
    }

    [Theory]
    [InlineData(AddPackageCommand.Options.WorkingDirectory.Short)]
    [InlineData(AddPackageCommand.Options.WorkingDirectory.Long)]
    public void Should_Use_Working_Directory_Option_For_Npm_Packages(string optionName)
    {
        var args = CreateArgs();
        args.Options.Add(optionName, WorkingDirectory);

        GetNpmProjectDirectory(args).ShouldBe(WorkingDirectory);
    }

    [Theory]
    [InlineData(AddPackageCommand.Options.AngularDirectory.Short)]
    [InlineData(AddPackageCommand.Options.AngularDirectory.Long)]
    public void Should_Still_Accept_Angular_Directory_Option(string optionName)
    {
        var args = CreateArgs();
        args.Options.Add(optionName, AngularDirectory);

        GetNpmProjectDirectory(args).ShouldBe(AngularDirectory);
    }

    [Fact]
    public void Should_Prefer_Working_Directory_Option_Over_Angular_Directory_Option()
    {
        var args = CreateArgs();
        args.Options.Add(AddPackageCommand.Options.AngularDirectory.Long, AngularDirectory);
        args.Options.Add(AddPackageCommand.Options.WorkingDirectory.Long, WorkingDirectory);

        GetNpmProjectDirectory(args).ShouldBe(WorkingDirectory);
    }

    [Fact]
    public void Should_Use_Current_Directory_When_No_Directory_Option_Is_Given()
    {
        GetNpmProjectDirectory(CreateArgs()).ShouldBe(Directory.GetCurrentDirectory());
    }

    private static CommandLineArgs CreateArgs()
    {
        return new CommandLineArgs(AddPackageCommand.Name, "@volo/abp-react-app-config");
    }

    private string GetNpmProjectDirectory(CommandLineArgs args)
    {
        return GetRequiredService<TestAddPackageCommand>().ResolveNpmProjectDirectory(args);
    }

    private class TestAddPackageCommand : AddPackageCommand
    {
        public TestAddPackageCommand(
            ProjectNugetPackageAdder projectNugetPackageAdder,
            ProjectNpmPackageAdder projectNpmPackageAdder,
            ITelemetryService telemetryService)
            : base(projectNugetPackageAdder, projectNpmPackageAdder, telemetryService)
        {
        }

        public string ResolveNpmProjectDirectory(CommandLineArgs args)
        {
            return GetNpmProjectDirectory(args);
        }
    }
}
