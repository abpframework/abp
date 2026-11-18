using System;
using System.Collections.Generic;
using System.IO;
using Shouldly;
using Volo.Abp.Cli.ProjectModification;
using Xunit;

namespace Volo.Abp.Cli;

public class EfCoreMigrationManager_Tests : IDisposable
{
    private readonly string _tempFolder;

    public EfCoreMigrationManager_Tests()
    {
        _tempFolder = Path.Combine(Path.GetTempPath(), "AbpCliEfCoreMigrationManagerTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempFolder);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempFolder))
        {
            Directory.Delete(_tempFolder, recursive: true);
        }
    }

    [Fact]
    public void AddMigration_WithDefaultAndTenantDbContexts_PassesContextForBoth()
    {
        CreateDbContextFile("DefaultDbContext.cs");
        CreateDbContextFile("TenantDbContext.cs");

        var manager = CreateManager();
        manager.AddMigration(Path.Combine(_tempFolder, "MyProject.EntityFrameworkCore.csproj"), "Volo.Abp.TestModule");

        manager.Calls.Count.ShouldBe(2);
        manager.Calls[0].DbContext.ShouldBe("TenantDbContext");
        manager.Calls[0].OutputDirectory.ShouldBe("TenantMigrations");
        manager.Calls[1].DbContext.ShouldBe("DefaultDbContext");
        manager.Calls[1].OutputDirectory.ShouldBe("Migrations");
        manager.Calls[0].MigrationName.ShouldBe(manager.Calls[1].MigrationName);
    }

    [Fact]
    public void AddMigration_WithSingleDbContext_PassesContext()
    {
        CreateDbContextFile("BookStoreDbContext.cs");

        var manager = CreateManager();
        manager.AddMigration(Path.Combine(_tempFolder, "MyProject.EntityFrameworkCore.csproj"), "Volo.Abp.TestModule");

        // Previously the migration was created without --context whenever no
        // tenant DbContext was detected, which fails with
        // "More than one DbContext was found" as soon as a second context exists.
        manager.Calls.Count.ShouldBe(1);
        manager.Calls[0].DbContext.ShouldBe("BookStoreDbContext");
        manager.Calls[0].OutputDirectory.ShouldBe("Migrations");
    }

    [Fact]
    public void AddMigration_WithMultipleNonTenantDbContexts_PassesContextForEach_WithDistinctMigrationNames()
    {
        CreateDbContextFile("DefaultDbContext.cs");
        CreateDbContextFile("ReportingDbContext.cs");

        var manager = CreateManager();
        manager.AddMigration(Path.Combine(_tempFolder, "MyProject.EntityFrameworkCore.csproj"), "Volo.Abp.TestModule");

        manager.Calls.Count.ShouldBe(2);
        manager.Calls[0].DbContext.ShouldBe("DefaultDbContext");
        manager.Calls[1].DbContext.ShouldBe("ReportingDbContext");
        manager.Calls[0].MigrationName.ShouldNotBe(manager.Calls[1].MigrationName);
    }

    [Fact]
    public void AddMigration_WithNoDbContextFile_FallsBackToNoContext()
    {
        var manager = CreateManager();
        manager.AddMigration(Path.Combine(_tempFolder, "MyProject.EntityFrameworkCore.csproj"), "Volo.Abp.TestModule");

        manager.Calls.Count.ShouldBe(1);
        manager.Calls[0].DbContext.ShouldBeNull();
        manager.Calls[0].OutputDirectory.ShouldBe("Migrations");
    }

    private void CreateDbContextFile(string fileName)
    {
        File.WriteAllText(Path.Combine(_tempFolder, fileName), "public class " + Path.GetFileNameWithoutExtension(fileName) + " { }");
    }

    private TestableEfCoreMigrationManager CreateManager()
    {
        return new TestableEfCoreMigrationManager();
    }

    private class TestableEfCoreMigrationManager : EfCoreMigrationManager
    {
        public List<(string MigrationName, string DbContext, string OutputDirectory)> Calls { get; } = new();

        public TestableEfCoreMigrationManager()
            : base(cmdHelper: null)
        {
        }

        protected override void RunAddMigrationCommand(
            string dbMigrationsProjectFolder,
            string migrationName,
            string dbContext,
            string outputDirectory)
        {
            Calls.Add((migrationName, dbContext, outputDirectory));
        }

        protected override string GetUniquePostFix()
        {
            return "_123";
        }
    }
}
