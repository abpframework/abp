using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Volo.Abp.Sms.TencentCloud;

public class TencentCloudSmsSenderTests : AbpSmsTencentCloudTestBase
{
    private readonly ISmsSender _smsSender;
    private readonly IConfiguration _configuration;

    public TencentCloudSmsSenderTests()
    {
        _configuration = GetRequiredService<IConfiguration>();
        _smsSender = GetRequiredService<ISmsSender>();
    }

    [Fact]
    public async Task SendSms_Test()
    {
        var config = _configuration.GetSection("AbpTencentCloudSms");

        // Please fill in the real parameters in the appsettings.json file.
        if (config["SecretId"] == "<Enter your SecretId from TencentCloud>")
        {
            return;
        }

        var msg = new SmsMessage(config["TargetPhoneNumber"],
            config["TemplateParam"]);
        msg.Properties.Add(TencentCloudSmsProperties.SignName, config["SignName"]);
        msg.Properties.Add(TencentCloudSmsProperties.TemplateId, config["TemplateId"]);

        await _smsSender.SendAsync(msg);
    }
}
