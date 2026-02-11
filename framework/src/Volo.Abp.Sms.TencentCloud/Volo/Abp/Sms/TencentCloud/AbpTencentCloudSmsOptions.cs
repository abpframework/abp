namespace Volo.Abp.Sms.TencentCloud;

public class AbpTencentCloudSmsOptions
{
    public string SmsSdkAppId { get; set; } = default!;

    public string SecretKey  { get; set; } = default!;

    public string SecretId  { get; set; } = default!;

    public string Endpoint  { get; set; } = "sms.tencentcloudapi.com";

    public string Region  { get; set; } = "ap-guangzhou";
}
