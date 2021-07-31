using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.Aliyun.Common;
using EasyAbp.Abp.Aliyun.Sms;
using EasyAbp.Abp.Aliyun.Sms.Model.Request;
using EasyAbp.Abp.Aliyun.Sms.Model.Response;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Sms.Aliyun
{
    public class AliyunSmsSender : ISmsSender, ITransientDependency
    {
        private readonly AbpAliyunSmsOptions _abpAliyunSmsOptions;
        private readonly IAliyunApiRequester _aliyunApiRequester;
        
        public AliyunSmsSender(IOptionsSnapshot<AbpAliyunSmsOptions> options, IAliyunApiRequester aliyunApiRequester)
        {
            _aliyunApiRequester = aliyunApiRequester;
            _abpAliyunSmsOptions = options.Value;
        }

        public async Task SendAsync(SmsMessage smsMessage)
        {
            var response = await _aliyunApiRequester.SendRequestAsync<SmsCommonResponse>(new SendSmsRequest(smsMessage.PhoneNumber,
                    smsMessage.Properties.GetOrDefault("SignName") as string,
                    smsMessage.Properties.GetOrDefault("TemplateCode") as string,
                    smsMessage.Text),
                _abpAliyunSmsOptions.EndPoint);

            if (response.Code != "OK")
            {
                throw new BusinessException("Volo.Abp.Sms.Aliyun:00001",
                    "Alibaba Cloud SMS failed to send, please check the Details property for specific errors.",
                    response.Message);
            }
        }
    }
}