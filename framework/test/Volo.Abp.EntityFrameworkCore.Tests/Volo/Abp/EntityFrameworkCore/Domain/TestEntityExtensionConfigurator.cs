using Volo.Abp.ObjectExtending;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Threading;

namespace Volo.Abp.EntityFrameworkCore.Domain
{
    public static class TestEntityExtensionConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ObjectExtensionManager.Instance
                    .MapEfCoreProperty<City, string>(
                        "PhoneCode",
                        p => p.HasMaxLength(8)
                    );
            });
        }
    }
}
