using Volo.Abp.BackgroundJobs;

namespace Volo.Abp.Emailing
{
    public class BackgroundEmailSendingJob : BackgroundJob<BackgroundEmailSendingJobArgs>
    {
        protected IEmailSender EmailSender { get; }

        public BackgroundEmailSendingJob(IEmailSender emailSender)
        {
            EmailSender = emailSender;
        }

        public override void Execute(BackgroundEmailSendingJobArgs args)
        {
            EmailSender.Send(args.To, args.Subject, args.Body, args.IsBodyHtml);
        }
    }
}