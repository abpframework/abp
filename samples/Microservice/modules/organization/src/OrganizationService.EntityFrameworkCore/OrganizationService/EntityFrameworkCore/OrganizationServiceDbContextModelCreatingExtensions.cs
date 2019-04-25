using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrganizationService.EntityFrameworkCore
{
    public static class OrganizationServiceDbContextModelCreatingExtensions
    {
        public static void ConfigureOrganizationService(
            this ModelBuilder builder,
            Action<OrganizationServiceModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OrganizationServiceModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);
            
            builder.Entity<Organization>(b =>
            {
                b.ToTable(options.TablePrefix + "Organizations", options.Schema);
       
                //b.ConfigureAudited();

                b.Property(x => x.Code).IsRequired().HasMaxLength(BaseConsts.MaxCodeLength);
                b.Property(x => x.Name).IsRequired().HasMaxLength(BaseConsts.MaxNameLength);
                b.Property(x => x.Remark).HasMaxLength(BaseConsts.MaxRemarkLength);



            });

            builder.Entity<UserOrganization>(b =>
            {
                b.ToTable(options.TablePrefix + "UserOrganizations", options.Schema);

                //b.ConfigureAudited();

                b.Property(x => x.UserId).IsRequired();
                b.Property(x => x.OrganizationId).IsRequired();

                b.HasOne(r => r.Organization).WithMany().HasForeignKey(r => r.OrganizationId).IsRequired();

            });

        }
    }
}