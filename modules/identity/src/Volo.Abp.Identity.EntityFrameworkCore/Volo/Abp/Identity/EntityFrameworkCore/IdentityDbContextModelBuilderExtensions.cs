using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Users.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public static class IdentityDbContextModelBuilderExtensions
    {
        public static void ConfigureIdentity(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] Action<IdentityModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new IdentityModelBuilderConfigurationOptions(
                AbpIdentityDbProperties.DbTablePrefix,
                AbpIdentityDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(options.TablePrefix + "Users", options.Schema);

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                b.Property(u => u.NormalizedUserName).IsRequired()
                    .HasMaxLength(IdentityUserConsts.MaxNormalizedUserNameLength)
                    .HasColumnName(nameof(IdentityUser.NormalizedUserName));
                b.Property(u => u.NormalizedEmail).IsRequired()
                    .HasMaxLength(IdentityUserConsts.MaxNormalizedEmailLength)
                    .HasColumnName(nameof(IdentityUser.NormalizedEmail));
                b.Property(u => u.PasswordHash).HasMaxLength(IdentityUserConsts.MaxPasswordHashLength)
                    .HasColumnName(nameof(IdentityUser.PasswordHash));
                b.Property(u => u.SecurityStamp).IsRequired().HasMaxLength(IdentityUserConsts.MaxSecurityStampLength)
                    .HasColumnName(nameof(IdentityUser.SecurityStamp));
                b.Property(u => u.TwoFactorEnabled).HasDefaultValue(false)
                    .HasColumnName(nameof(IdentityUser.TwoFactorEnabled));
                b.Property(u => u.LockoutEnabled).HasDefaultValue(false)
                    .HasColumnName(nameof(IdentityUser.LockoutEnabled));

                b.Property(u => u.IsExternal).IsRequired().HasDefaultValue(false)
                    .HasColumnName(nameof(IdentityUser.IsExternal));

                b.Property(u => u.AccessFailedCount)
                    .If(!builder.IsUsingOracle(), p => p.HasDefaultValue(0))
                    .HasColumnName(nameof(IdentityUser.AccessFailedCount));

                b.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.Tokens).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.OrganizationUnits).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(u => u.NormalizedUserName);
                b.HasIndex(u => u.NormalizedEmail);
                b.HasIndex(u => u.UserName);
                b.HasIndex(u => u.Email);
            });

            builder.Entity<IdentityUserClaim>(b =>
            {
                b.ToTable(options.TablePrefix + "UserClaims", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityUserClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityUserClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.UserId);
            });

            builder.Entity<IdentityUserRole>(b =>
            {
                b.ToTable(options.TablePrefix + "UserRoles", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(ur => new {ur.UserId, ur.RoleId});

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasOne<IdentityUser>().WithMany(u => u.Roles).HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(ur => new {ur.RoleId, ur.UserId});
            });

            builder.Entity<IdentityUserLogin>(b =>
            {
                b.ToTable(options.TablePrefix + "UserLogins", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(x => new {x.UserId, x.LoginProvider});

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserLoginConsts.MaxLoginProviderLength)
                    .IsRequired();
                b.Property(ul => ul.ProviderKey).HasMaxLength(IdentityUserLoginConsts.MaxProviderKeyLength)
                    .IsRequired();
                b.Property(ul => ul.ProviderDisplayName)
                    .HasMaxLength(IdentityUserLoginConsts.MaxProviderDisplayNameLength);

                b.HasIndex(l => new {l.LoginProvider, l.ProviderKey});
            });

            builder.Entity<IdentityUserToken>(b =>
            {
                b.ToTable(options.TablePrefix + "UserTokens", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(l => new {l.UserId, l.LoginProvider, l.Name});

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserTokenConsts.MaxLoginProviderLength)
                    .IsRequired();
                b.Property(ul => ul.Name).HasMaxLength(IdentityUserTokenConsts.MaxNameLength).IsRequired();
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable(options.TablePrefix + "Roles", options.Schema);

                b.ConfigureByConvention();

                b.Property(r => r.Name).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNameLength);
                b.Property(r => r.NormalizedName).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNormalizedNameLength);
                b.Property(r => r.IsDefault).HasColumnName(nameof(IdentityRole.IsDefault));
                b.Property(r => r.IsStatic).HasColumnName(nameof(IdentityRole.IsStatic));
                b.Property(r => r.IsPublic).HasColumnName(nameof(IdentityRole.IsPublic));

                b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

                b.HasIndex(r => r.NormalizedName);
            });

            builder.Entity<IdentityRoleClaim>(b =>
            {
                b.ToTable(options.TablePrefix + "RoleClaims", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityRoleClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityRoleClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.RoleId);
            });

            if (builder.IsHostDatabase())
            {
                builder.Entity<IdentityClaimType>(b =>
                {
                    b.ToTable(options.TablePrefix + "ClaimTypes", options.Schema);

                    b.ConfigureByConvention();

                    b.Property(uc => uc.Name).HasMaxLength(IdentityClaimTypeConsts.MaxNameLength)
                        .IsRequired(); // make unique
                    b.Property(uc => uc.Regex).HasMaxLength(IdentityClaimTypeConsts.MaxRegexLength);
                    b.Property(uc => uc.RegexDescription).HasMaxLength(IdentityClaimTypeConsts.MaxRegexDescriptionLength);
                    b.Property(uc => uc.Description).HasMaxLength(IdentityClaimTypeConsts.MaxDescriptionLength);
                });
            }

            builder.Entity<OrganizationUnit>(b =>
            {
                b.ToTable(options.TablePrefix + "OrganizationUnits", options.Schema);

                b.ConfigureByConvention();

                b.Property(ou => ou.Code).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxCodeLength)
                    .HasColumnName(nameof(OrganizationUnit.Code));
                b.Property(ou => ou.DisplayName).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxDisplayNameLength)
                    .HasColumnName(nameof(OrganizationUnit.DisplayName));

                b.HasMany<OrganizationUnit>().WithOne().HasForeignKey(ou => ou.ParentId);
                b.HasMany(ou => ou.Roles).WithOne().HasForeignKey(our => our.OrganizationUnitId).IsRequired();

                b.HasIndex(ou => ou.Code);
            });

            builder.Entity<OrganizationUnitRole>(b =>
            {
                b.ToTable(options.TablePrefix + "OrganizationUnitRoles", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(ou => new {ou.OrganizationUnitId, ou.RoleId});

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ou => ou.RoleId).IsRequired();

                b.HasIndex(ou => new {ou.RoleId, ou.OrganizationUnitId});
            });

            builder.Entity<IdentityUserOrganizationUnit>(b =>
            {
                b.ToTable(options.TablePrefix + "UserOrganizationUnits", options.Schema);

                b.ConfigureByConvention();

                b.HasKey(ou => new {ou.OrganizationUnitId, ou.UserId});

                b.HasOne<OrganizationUnit>().WithMany().HasForeignKey(ou => ou.OrganizationUnitId).IsRequired();

                b.HasIndex(ou => new {ou.UserId, ou.OrganizationUnitId});
            });

            builder.Entity<IdentitySecurityLog>(b =>
            {
                b.ToTable(options.TablePrefix + "SecurityLogs", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.TenantName).HasMaxLength(IdentitySecurityLogConsts.MaxTenantNameLength);

                b.Property(x => x.ApplicationName).HasMaxLength(IdentitySecurityLogConsts.MaxApplicationNameLength);
                b.Property(x => x.Identity).HasMaxLength(IdentitySecurityLogConsts.MaxIdentityLength);
                b.Property(x => x.Action).HasMaxLength(IdentitySecurityLogConsts.MaxActionLength);

                b.Property(x => x.UserName).HasMaxLength(IdentitySecurityLogConsts.MaxUserNameLength);

                b.Property(x => x.ClientIpAddress).HasMaxLength(IdentitySecurityLogConsts.MaxClientIpAddressLength);
                b.Property(x => x.ClientId).HasMaxLength(IdentitySecurityLogConsts.MaxClientIdLength);
                b.Property(x => x.CorrelationId).HasMaxLength(IdentitySecurityLogConsts.MaxCorrelationIdLength);
                b.Property(x => x.BrowserInfo).HasMaxLength(IdentitySecurityLogConsts.MaxBrowserInfoLength);

                b.HasIndex(x => new { x.TenantId, x.ApplicationName });
                b.HasIndex(x => new { x.TenantId, x.Identity });
                b.HasIndex(x => new { x.TenantId, x.Action });
                b.HasIndex(x => new { x.TenantId, x.UserId });
            });

            if (builder.IsHostDatabase())
            {
                builder.Entity<IdentityLinkUser>(b =>
                {
                    b.ToTable(options.TablePrefix + "LinkUsers", options.Schema);

                    b.ConfigureByConvention();

                    b.HasIndex(x => new
                    {
                        UserId = x.SourceUserId,
                        TenantId = x.SourceTenantId,
                        LinkedUserId = x.TargetUserId,
                        LinkedTenantId = x.TargetTenantId
                    }).IsUnique();
                });
            }
        }
    }
}
