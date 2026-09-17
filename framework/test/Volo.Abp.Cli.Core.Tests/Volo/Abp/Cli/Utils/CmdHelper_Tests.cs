using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Cli.Utils;
using Xunit;

namespace Volo.Abp.Cli.Utils;

public class CmdHelper_Tests : AbpCliTestBase
{
    private readonly ICmdHelper _cmdHelper;

    public CmdHelper_Tests()
    {
        _cmdHelper = GetRequiredService<ICmdHelper>();
    }

    [Fact]
    public async Task RunCmdAndGetOutput_Should_Not_Deadlock_With_Large_Stdout_And_Stderr()
    {
        // Reproduces the deadlock bug where sequential ReadToEnd() on stdout then stderr
        // would block indefinitely when both pipe buffers (~64 KB on Linux/macOS, ~4 KB on
        // Windows) filled up simultaneously. The process was blocked writing to stderr while
        // the caller was blocked waiting for stdout to close — a classic pipe deadlock.
        //
        // The fix reads both streams concurrently via ReadToEndAsync + Task.WhenAll, which
        // drains both pipes at the same time and avoids the deadlock.
        var command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? @"for /L %i in (1,1,3000) do @(echo stdout-line-%i & echo stderr-line-%i 1>&2)"
            : "for i in $(seq 1 5000); do echo stdout-line-$i; echo stderr-line-$i >&2; done";

        string output = null;
        var cmdTask = Task.Run(() => output = _cmdHelper.RunCmdAndGetOutput(command));
        var completed = await Task.WhenAny(cmdTask, Task.Delay(TimeSpan.FromSeconds(10)));

        // The original sequential code deadlocked here; 10 s is a generous upper bound.
        (completed == cmdTask).ShouldBeTrue(
            "RunCmdAndGetOutput should not deadlock when both stdout and stderr produce large output");

        await cmdTask;

        output.ShouldNotBeNullOrWhiteSpace();
        output.ShouldContain("stdout-line-");
        output.ShouldContain("stderr-line-");
    }
}
