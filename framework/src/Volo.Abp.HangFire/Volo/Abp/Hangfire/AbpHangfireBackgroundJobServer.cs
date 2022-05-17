using Hangfire;

namespace Volo.Abp.Hangfire;

public class AbpHangfireBackgroundJobServer
{
    public BackgroundJobServer HangfireJobServer { get; }

    public AbpHangfireBackgroundJobServer(BackgroundJobServer hangfireJobServer)
    {
        HangfireJobServer = hangfireJobServer;
    }
}
