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

                b.ConfigureFullAuditedAggregateRoot();
                b.ConfigureAbpUser();

                b.Property(u => u.NormalizedUserName).IsRequired().HasMaxLength(IdentityUserConsts.MaxNormalizedUserNameLength).HasColumnName(nameof(IdentityUser.NormalizedUserName));
                b.Property(u => u.NormalizedEmail).HasMaxLength(IdentityUserConsts.MaxNormalizedEmailLength).HasColumnName(nameof(IdentityUser.NormalizedEmail));
                b.Property(u => u.PasswordHash).HasMaxLength(IdentityUserConsts.MaxPasswordHashLength).HasColumnName(nameof(IdentityUser.PasswordHash));
                b.Property(u => u.SecurityStamp).IsRequired().HasMaxLength(IdentityUserConsts.MaxSecurityStampLength).HasColumnName(nameof(IdentityUser.SecurityStamp));
                b.Property(u => u.TwoFactorEnabled).HasDefaultValue(false).HasColumnName(nameof(IdentityUser.TwoFactorEnabled));
                b.Property(u => u.LockoutEnabled).HasDefaultValue(false).HasColumnName(nameof(IdentityUser.LockoutEnabled));
                b.Property(u => u.AccessFailedCount).HasDefaultValue(0).HasColumnName(nameof(IdentityUser.AccessFailedCount));

                b.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.Tokens).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(u => u.NormalizedUserName);
                b.HasIndex(u => u.NormalizedEmail);
                b.HasIndex(u => u.UserName);
                b.HasIndex(u => u.Email);
            });

            builder.Entity<IdentityUserClaim>(b =>
            {
                b.ToTable(options.TablePrefix + "UserClaims", options.Schema);

                b.Property(x => x.Id).ValueGeneratedNever();
                
                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityUserClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityUserClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.UserId);
            });

            builder.Entity<IdentityUserRole>(b =>
            {
                b.ToTable(options.TablePrefix + "UserRoles", options.Schema);

                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasOne<IdentityUser>().WithMany(u => u.Roles).HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(ur => new { ur.RoleId, ur.UserId });
            });

            builder.Entity<IdentityUserLogin>(b =>
            {
                b.ToTable(options.TablePrefix + "UserLogins", options.Schema);

                b.HasKey(x => new { x.UserId, x.LoginProvider });

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserLoginConsts.MaxLoginProviderLength).IsRequired();
                b.Property(ul => ul.ProviderKey).HasMaxLength(IdentityUserLoginConsts.MaxProviderKeyLength).IsRequired();
                b.Property(ul => ul.ProviderDisplayName).HasMaxLength(IdentityUserLoginConsts.MaxProviderDisplayNameLength);

                b.HasIndex(l => new { l.LoginProvider, l.ProviderKey });
            });

            builder.Entity<IdentityUserToken>(b =>
            {
                b.ToTable(options.TablePrefix + "UserTokens", options.Schema);

                b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserTokenConsts.MaxLoginProviderLength).IsRequired();
                b.Property(ul => ul.Name).HasMaxLength(IdentityUserTokenConsts.MaxNameLength).IsRequired();
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable(options.TablePrefix + "Roles", options.Schema);

                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();

                b.Property(r => r.Name).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNameLength);
                b.Property(r => r.NormalizedName).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNormalizedNameLength);
                b.Property(u => u.ConcurrencyStamp).IsRequired().IsConcurrencyToken().HasMaxLength(IdentityRoleConsts.MaxConcurrencyStampLength).HasColumnName(nameof(IdentityRole.ConcurrencyStamp));
                b.Property(r => r.IsDefault).HasColumnName(nameof(IdentityRole.IsDefault));
                b.Property(r => r.IsStatic).HasColumnName(nameof(IdentityRole.IsStatic));
                b.Property(r => r.IsPublic).HasColumnName(nameof(IdentityRole.IsPublic));

                b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

                b.HasIndex(r => r.NormalizedName);
            });

            builder.Entity<IdentityRoleClaim>(b =>
            {
                b.ToTable(options.TablePrefix + "RoleClaims", options.Schema);

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityRoleClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityRoleClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.RoleId);
            });

            builder.Entity<IdentityClaimType>(b =>
            {
                b.ToTable(options.TablePrefix + "ClaimTypes", options.Schema);

                b.ConfigureExtraProperties();

                b.Property(uc => uc.Name).HasMaxLength(IdentityClaimTypeConsts.MaxNameLength).IsRequired(); // make unique
                b.Property(uc => uc.Regex).HasMaxLength(IdentityClaimTypeConsts.MaxRegexLength);
                b.Property(uc => uc.RegexDescription).HasMaxLength(IdentityClaimTypeConsts.MaxRegexDescriptionLength);
                b.Property(uc => uc.Description).HasMaxLength(IdentityClaimTypeConsts.MaxDescriptionLength);
                b.Property(uc => uc.ConcurrencyStamp).IsRequired().IsConcurrencyToken().HasMaxLength(IdentityClaimTypeConsts.MaxConcurrencyStampLength).HasColumnName(nameof(IdentityClaimType.ConcurrencyStamp));
            });
        }
    }
}
