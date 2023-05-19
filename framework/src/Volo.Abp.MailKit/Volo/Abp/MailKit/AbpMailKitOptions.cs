using MailKit.Security;

namespace Volo.Abp.MailKit;

public class AbpMailKitOptions
{
    public SecureSocketOptions? SecureSocketOption { get; set; }
}
