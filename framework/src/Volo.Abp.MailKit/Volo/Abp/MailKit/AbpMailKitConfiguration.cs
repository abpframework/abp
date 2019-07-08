using MailKit.Security;

namespace Volo.Abp.MailKit
{
    public class AbpMailKitConfiguration : IAbpMailKitConfiguration
    {
        public SecureSocketOptions? SecureSocketOption { get; set; }
    }
}
