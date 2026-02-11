using System;
using System.Linq;
using System.Text.RegularExpressions;
using Hangfire;
using Hangfire.States;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Hangfire;

public class AbpHangfireOptionsConfiguration : IPostConfigureOptions<AbpHangfireOptions>
{
    public void PostConfigure(string? name, AbpHangfireOptions options)
    {
        if (options.DefaultQueuePrefix.IsNullOrWhiteSpace())
        {
            return;;
        }

        // The Queue name argument must consist of lowercase letters, digits, underscore, and dash characters only.
        var queuesPrefix = Regex.Replace(options.DefaultQueuePrefix.ToLower().Replace(".", "_"), "[^a-z0-9_-]", "");
        if (queuesPrefix.IsNullOrWhiteSpace())
        {
            throw new AbpException($"The QueuesPrefix({options.DefaultQueuePrefix}) is not valid, it must consist of lowercase letters, digits, underscore, and dash characters only.");
        }

        options.DefaultQueuePrefix = queuesPrefix.EnsureEndsWith('_');

        if (options.ServerOptions == null)
        {
            var queue = $"{options.DefaultQueuePrefix}{EnqueuedState.DefaultQueue}";
            if (queue.Length > options.MaxQueueNameLength)
            {
                throw new AbpException($"The maximum length of the Hangfire queue name({queue}) is {options.MaxQueueNameLength}, Please configure the AbpHangfireOptions.DefaultQueuePrefix manually.");
            }
            options.ServerOptions = new BackgroundJobServerOptions
            {
                Queues = new[] { queue }
            };
            options.DefaultQueue = queue;
        }
        else
        {
            var queues = options.ServerOptions.Queues;
            for (var i = 0; i < queues.Length; i++)
            {
                var queue = $"{options.DefaultQueuePrefix}{queues[i]}";
                if (queue.Length > options.MaxQueueNameLength)
                {
                    throw new AbpException($"The maximum length of the Hangfire queue name({queue}) is {options.MaxQueueNameLength}, Please configure the AbpHangfireOptions.DefaultQueuePrefix manually.");
                }
                queues[i] = queue;
            }
            var defaultQueue = queues.FirstOrDefault(q => q.EndsWith(EnqueuedState.DefaultQueue));
            if (defaultQueue.IsNullOrWhiteSpace())
            {
                defaultQueue = queues.FirstOrDefault();
                if (defaultQueue.IsNullOrWhiteSpace())
                {
                    throw new AbpException("There is no queue defined in the Hangfire configuration!");
                }
            }
            options.DefaultQueue = defaultQueue;
        }
    }
}
