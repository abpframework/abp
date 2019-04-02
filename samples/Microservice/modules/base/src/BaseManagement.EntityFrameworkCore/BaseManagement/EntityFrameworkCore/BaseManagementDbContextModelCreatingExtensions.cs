using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BaseManagement.EntityFrameworkCore
{
    public static class BaseManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureBaseManagement(
            this ModelBuilder builder,
            Action<BaseManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BaseManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);
            
            builder.Entity<BaseType>(b =>
            {
                b.ToTable(options.TablePrefix + "BaseTypes", options.Schema);
       
                b.ConfigureAudited();

                b.Property(x => x.Code).IsRequired().HasMaxLength(BaseConsts.MaxCodeLength);
                b.Property(x => x.Name).IsRequired().HasMaxLength(BaseConsts.MaxNameLength);
                b.Property(x => x.Remark).HasMaxLength(BaseConsts.MaxRemarkLength);

                b.HasMany(x => x.BaseItems);
            });

            builder.Entity<BaseItem>(b =>
            {
                b.ToTable(options.TablePrefix + "BaseItems", options.Schema);

                b.ConfigureAudited();

                b.Property(x => x.Code).IsRequired().HasMaxLength(BaseConsts.MaxCodeLength);
                b.Property(x => x.Name).IsRequired().HasMaxLength(BaseConsts.MaxNameLength);
                b.Property(x => x.Remark).HasMaxLength(BaseConsts.MaxRemarkLength);

                b.HasOne(x => x.BaseType).WithMany(x=>x.BaseItems).HasForeignKey(x=>x.BaseTypeGuid);
            });
        }
    }
}