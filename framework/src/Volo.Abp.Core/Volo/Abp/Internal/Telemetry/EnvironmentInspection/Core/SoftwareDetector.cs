using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

[ExposeServices(typeof(ISoftwareDetector))]
abstract internal class SoftwareDetector: ISoftwareDetector , ISingletonDependency
{
    public abstract string Name { get; }
    public abstract Task<SoftwareInfo?> DetectAsync();

    protected virtual async Task<string?> ExecuteCommandAsync(string command, string? arg)
    {
        var outputBuilder = new StringBuilder();

        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arg ?? "",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.EnableRaisingEvents = true;

        var tcs = new TaskCompletionSource<bool>();

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                outputBuilder.AppendLine(e.Data);
            }
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                outputBuilder.AppendLine(e.Data);
            }
        };

        process.Exited += (sender, e) =>
        {
            tcs.TrySetResult(true);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await tcs.Task;

        var output = outputBuilder.ToString().Trim();
        return string.IsNullOrWhiteSpace(output) ? null : output;
    }

    protected string? GetFileVersion(string filePath)
    {
        try
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            return versionInfo.FileVersion;
        }
        catch
        {
            return string.Empty;
        }
    }
}