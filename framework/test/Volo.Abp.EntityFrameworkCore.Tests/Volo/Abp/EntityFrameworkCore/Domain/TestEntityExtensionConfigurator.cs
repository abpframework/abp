using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.ObjectExtending;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
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
                    ).MapEfCoreEntity<City>(b =>
                    {
                        b.As<EntityTypeBuilder<City>>()
                            .Property(x=>x.Name).IsRequired().HasMaxLength(200);

                    }).MapEfCoreEntity(typeof(Person), b =>
                    {
                        b.As<EntityTypeBuilder<Person>>()
                            .HasIndex(x=>x.Birthday);
                    });

                ObjectExtensionManager.Instance.MapEfCoreDbContext<TestAppDbContext>(b =>
                {
                    b.Entity<City>().Property(x => x.Name).IsRequired();
                });
            });
        }
    }
}
