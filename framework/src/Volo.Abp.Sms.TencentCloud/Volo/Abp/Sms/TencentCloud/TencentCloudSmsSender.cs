using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Sms.TencentCloud;

public class TencentCloudSmsSender : ISmsSender, ITransientDependency
{
    protected AbpTencentCloudSmsOptions Options { get; }

    public TencentCloudSmsSender(IOptionsMonitor<AbpTencentCloudSmsOptions> options)
    {
        Options = options.CurrentValue;
    }

    public virtual async Task SendAsync(SmsMessage smsMessage)
    {
        var client = CreateClient();

        await client.SendSms(new SendSmsRequest()
        {
            SmsSdkAppId = Options.SmsSdkAppId,
            SignName = smsMessage.Properties.GetOrDefault(TencentCloudSmsProperties.SignName) as string,
            TemplateId = smsMessage.Properties.GetOrDefault(TencentCloudSmsProperties.TemplateId) as string,
            TemplateParamSet = smsMessage.Text.Split(','),
            PhoneNumberSet = [smsMessage.PhoneNumber]
        });
    }

    protected virtual SmsClient CreateClient()
    {
        var credential = new Credential
        {
            SecretId = Options.SecretId,
            SecretKey = Options.SecretKey
        };
        var clientProfile = new ClientProfile
        {
            HttpProfile = new HttpProfile
            {
                Endpoint = Options.Endpoint
            }
        };
        return new SmsClient(credential, Options.Region, clientProfile);
    }
}
