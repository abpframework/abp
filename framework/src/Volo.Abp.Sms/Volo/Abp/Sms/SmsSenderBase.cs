using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;

namespace Volo.Abp.Sms
{
    public abstract class SmsSenderBase : ISmsSender
    {
        protected IBackgroundJobManager BackgroundJobManager { get; }
        public SmsSenderBase(IBackgroundJobManager backgroundJobManager)
        {
            BackgroundJobManager = backgroundJobManager;
        }
        public async Task QueueAsync(SmsMessage smsMessage)
        {
            if (!BackgroundJobManager.IsAvailable())
            {
                await SendAsync(smsMessage);
                return;
            }

            await BackgroundJobManager.EnqueueAsync(
                new BackgroundSmsSendingJobArgs
                {
                    PhoneNumber = smsMessage.PhoneNumber,
                    Text = smsMessage.Text,
                    Properties = smsMessage.Properties
                }
            );
        }

        public Task SendAsync(SmsMessage smsMessage)
        {
            return SendSmsAsync(smsMessage);
        }

        protected abstract Task SendSmsAsync(SmsMessage smsMessage);
    }
}
