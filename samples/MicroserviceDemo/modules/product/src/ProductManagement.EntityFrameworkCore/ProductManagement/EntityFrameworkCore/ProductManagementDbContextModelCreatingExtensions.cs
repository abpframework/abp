using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ProductManagement.EntityFrameworkCore
{
    public static class ProductManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureProductManagement(
            this ModelBuilder builder,
            Action<ProductManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ProductManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);
            
            builder.Entity<Product>(b =>
            {
                b.ToTable(options.TablePrefix + "Products", options.Schema);

                b.ConfigureAudited();

                b.Property(x => x.Code).IsRequired().HasMaxLength(ProductConsts.MaxCodeLength);
                b.Property(x => x.Name).IsRequired().HasMaxLength(ProductConsts.MaxNameLength);

                b.HasIndex(q => q.Code);
                b.HasIndex(q => q.Name);
            });
        }
    }
}