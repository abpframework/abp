using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Sms
{
    public class BackgroundSmsSendingJob : AsyncBackgroundJob<BackgroundSmsSendingJobArgs>, ITransientDependency
    {
        protected ISmsSender SmsSender { get; }

        public BackgroundSmsSendingJob(ISmsSender smsSender)
        {
            SmsSender = smsSender;
        }

        public async override Task ExecuteAsync(BackgroundSmsSendingJobArgs args)
        {
            await SmsSender.SendAsync(new SmsMessage(args.PhoneNumber, args.Text, args.Properties));
        }
    }
}
