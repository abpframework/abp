using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Internal.Telemetry.Activity;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Activity.Providers;
using Volo.Abp.Internal.Telemetry.Constants;
using Xunit;

namespace Volo.Abp.Telemetry;

public class TelemetrySolutionInfoEnricher_Tests : IDisposable
{
    private readonly string _solutionDirectory;

    public TelemetrySolutionInfoEnricher_Tests()
    {
        _solutionDirectory = Path.Combine(Path.GetTempPath(), "abp-telemetry-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(_solutionDirectory, "catalog"));
        Directory.CreateDirectory(Path.Combine(_solutionDirectory, "ordering"));
    }

    [Fact]
    public async Task Should_Handle_Modules_That_Import_Each_Other()
    {
        File.WriteAllText(Path.Combine(_solutionDirectory, "MySolution.abpsln"), """
            {
              "modules": {
                "Catalog": { "path": "catalog/Catalog.abpmdl" },
                "Ordering": { "path": "ordering/Ordering.abpmdl" }
              }
            }
            """);
        File.WriteAllText(Path.Combine(_solutionDirectory, "catalog", "Catalog.abpmdl"), """
            {
              "imports": {
                "Volo.Abp.SettingManagement": { "version": "10.7.0" },
                "Ordering": { "path": "../ordering/Ordering.abpmdl" }
              }
            }
            """);
        File.WriteAllText(Path.Combine(_solutionDirectory, "ordering", "Ordering.abpmdl"), """
            {
              "imports": {
                "Volo.Abp.FeatureManagement": { "version": "10.7.0" },
                "Catalog": { "path": "../catalog/Catalog.abpmdl" }
              }
            }
            """);

        var storage = Substitute.For<ITelemetryActivityStorage>();
        storage.ShouldAddSolutionInformation(Arg.Any<Guid>()).Returns(true);
        var enricher = new TelemetrySolutionInfoEnricher(storage, new ServiceCollection().BuildServiceProvider());

        var context = ActivityContext.Create("Test");
        context.Current[ActivityPropertyNames.SolutionId] = Guid.NewGuid();
        context.ExtraProperties[ActivityPropertyNames.SolutionPath] = Path.Combine(_solutionDirectory, "MySolution.abpsln");

        await enricher.EnrichAsync(context);

        context.Current[ActivityPropertyNames.HasSolutionInfo].ShouldBe(true);
        var moduleNames = ((List<Dictionary<string, object?>>)context.Current[ActivityPropertyNames.InstalledModules]!)
            .Select(x => x[ActivityPropertyNames.ModuleName])
            .ToList();
        moduleNames.ShouldContain("Volo.Abp.SettingManagement");
        moduleNames.ShouldContain("Volo.Abp.FeatureManagement");
    }

    public void Dispose()
    {
        if (Directory.Exists(_solutionDirectory))
        {
            Directory.Delete(_solutionDirectory, true);
        }
    }
}
