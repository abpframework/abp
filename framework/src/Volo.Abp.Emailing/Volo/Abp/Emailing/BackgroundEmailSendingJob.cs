using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Emailing
{
    public class BackgroundEmailSendingJob : BackgroundJob<BackgroundEmailSendingJobArgs>, ITransientDependency
    {
        protected IEmailSender EmailSender { get; }

        public BackgroundEmailSendingJob(IEmailSender emailSender)
        {
            EmailSender = emailSender;
        }

        public override void Execute(BackgroundEmailSendingJobArgs args)
        {
            AsyncHelper.RunSync(() => EmailSender.SendAsync(args.To, args.Subject, args.Body, args.IsBodyHtml));
        }
    }
}