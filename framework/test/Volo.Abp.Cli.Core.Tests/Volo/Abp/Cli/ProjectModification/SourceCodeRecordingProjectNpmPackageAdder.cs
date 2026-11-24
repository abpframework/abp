using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.LIbs;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.Utils;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ProjectModification;

/// <summary>
/// Replaces the source code download (an HTTP call to abp.io) with a recorded call that writes
/// the folder a real download would leave behind.
/// </summary>
public class SourceCodeRecordingProjectNpmPackageAdder : ProjectNpmPackageAdder
{
    public List<(string Directory, string PackageName, string Version)> Downloads { get; } = [];

    public bool IsDownloadable { get; set; } = true;

    public SourceCodeRecordingProjectNpmPackageAdder(
        IJsonSerializer jsonSerializer,
        SourceCodeDownloadService sourceCodeDownloadService,
        AngularSourceCodeAdder angularSourceCodeAdder,
        IRemoteServiceExceptionHandler remoteServiceExceptionHandler,
        IInstallLibsService installLibsService,
        ICmdHelper cmdHelper,
        CliHttpClientFactory cliHttpClientFactory,
        INpmPackageInfoProvider npmPackageInfoProvider,
        NpmHelper npmHelper)
        : base(
            jsonSerializer,
            sourceCodeDownloadService,
            angularSourceCodeAdder,
            remoteServiceExceptionHandler,
            installLibsService,
            cmdHelper,
            cliHttpClientFactory,
            npmPackageInfoProvider,
            npmHelper)
    {
    }

    protected override Task<bool> DownloadAngularSourceCode(string angularDirectory, NpmPackageInfo package,
        string version = null)
    {
        Downloads.Add((angularDirectory, package.Name, version));

        if (!IsDownloadable)
        {
            return Task.FromResult(false);
        }

        var sourceCodeFolder = Path.Combine(
            angularDirectory, "projects", package.Name.RemovePreFix("@").Replace("/", "-"));

        Directory.CreateDirectory(sourceCodeFolder);
        File.WriteAllText(Path.Combine(sourceCodeFolder, "package.json"), $"{{ \"name\": \"{package.Name}\" }}");

        return Task.FromResult(true);
    }
}
