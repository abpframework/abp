using System;
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
                        (e, p) =>
                        {
                            e.HasIndex(p.Metadata.Name).IsUnique();
                            p.HasMaxLength(8);
                        }
                    ).MapEfCoreProperty<City, string>(
                        "ZipCode"
                    ).MapEfCoreProperty<City, int>(
                        "Rank"
                    ).MapEfCoreProperty<City, DateTime?>(
                        "Established"
                    ).MapEfCoreProperty<City, Guid>(
                        "Guid"
                    ).MapEfCoreProperty<City, ExtraProperties_Tests.Color?>(
                        "EnumNumber"
                    ).MapEfCoreProperty<City, ExtraProperties_Tests.Color>(
                        "EnumNumberString"
                    ).MapEfCoreProperty<City, ExtraProperties_Tests.Color>(
                        "EnumLiteral"
                    );
            });
        }
    }
}
