using MailKit.Security;

namespace Volo.Abp.MailKit
{
    public interface IAbpMailKitConfiguration
    {
        SecureSocketOptions? SecureSocketOption { get; set; }
    }
}
