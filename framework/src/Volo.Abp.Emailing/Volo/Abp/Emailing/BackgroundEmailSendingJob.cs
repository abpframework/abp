using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Emailing
{
    public class BackgroundEmailSendingJob : AsyncBackgroundJob<BackgroundEmailSendingJobArgs>, ITransientDependency
    {
        protected IEmailSender EmailSender { get; }

        public BackgroundEmailSendingJob(IEmailSender emailSender)
        {
            EmailSender = emailSender;
        }

        public override async Task ExecuteAsync(BackgroundEmailSendingJobArgs args)
        {
            if (args.From.IsNullOrWhiteSpace())
            {
                await EmailSender.SendAsync(args.To, args.Subject, args.Body, args.IsBodyHtml);
            }
            else
            {
                await EmailSender.SendAsync(args.From, args.To, args.Subject, args.Body, args.IsBodyHtml);
            }
        }
    }
}
