using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public static class IdentityDbContextModelBuilderExtensions
    {
        public static void ConfigureAbpIdentity(
            [NotNull] this ModelBuilder builder, 
            [CanBeNull] string tablePrefix = AbpIdentityConsts.DefaultDbTablePrefix, 
            [CanBeNull] string schema = AbpIdentityConsts.DefaultDbSchema)
        {
            Check.NotNull(builder, nameof(builder));

            if (tablePrefix == null)
            {
                tablePrefix = "";
            }

            //TODO: Set column names for all EF Core mappings as did for some of the user properties below (this is needed for table splitting scenario):

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(tablePrefix + "Users", schema);
                
                b.Property(u => u.UserName).IsRequired().HasMaxLength(IdentityUserConsts.MaxUserNameLength).HasColumnName("UserName");
                b.Property(u => u.NormalizedUserName).IsRequired().HasMaxLength(IdentityUserConsts.MaxNormalizedUserNameLength);
                b.Property(u => u.Email).HasMaxLength(IdentityUserConsts.MaxEmailLength).HasColumnName("Email");
                b.Property(u => u.NormalizedEmail).HasMaxLength(IdentityUserConsts.MaxNormalizedEmailLength);
                b.Property(u => u.PhoneNumber).HasMaxLength(IdentityUserConsts.MaxPhoneNumberLength).HasColumnName("PhoneNumber");
                b.Property(u => u.PasswordHash).HasMaxLength(IdentityUserConsts.MaxPasswordHashLength);
                b.Property(u => u.SecurityStamp).IsRequired().HasMaxLength(IdentityUserConsts.MaxSecurityStampLength);
                b.Property(u => u.ConcurrencyStamp).IsRequired().HasMaxLength(IdentityUserConsts.MaxConcurrencyStampLength);
                b.Property(u => u.EmailConfirmed).HasDefaultValue(false).HasColumnName("EmailConfirmed");
                b.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false).HasColumnName("PhoneNumberConfirmed");
                b.Property(u => u.TwoFactorEnabled).HasDefaultValue(false);
                b.Property(u => u.LockoutEnabled).HasDefaultValue(false);
                b.Property(u => u.AccessFailedCount).HasDefaultValue(0);

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
                b.ToTable(tablePrefix + "UserClaims", schema);

                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityUserClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityUserClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.UserId);
            });

            builder.Entity<IdentityUserRole>(b =>
            {
                b.ToTable(tablePrefix + "UserRoles", schema);

                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasOne<IdentityUser>().WithMany(u => u.Roles).HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(ur => new { ur.RoleId, ur.UserId });
            });

            builder.Entity<IdentityUserLogin>(b =>
            {
                b.ToTable(tablePrefix + "UserLogins", schema);

                b.HasKey(x => new { x.UserId, x.LoginProvider });

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserLoginConsts.MaxLoginProviderLength).IsRequired();
                b.Property(ul => ul.ProviderKey).HasMaxLength(IdentityUserLoginConsts.MaxProviderKeyLength).IsRequired();
                b.Property(ul => ul.ProviderDisplayName).HasMaxLength(IdentityUserLoginConsts.MaxProviderDisplayNameLength);

                b.HasIndex(l => new { l.LoginProvider, l.ProviderKey });
            });

            builder.Entity<IdentityUserToken>(b =>
            {
                b.ToTable(tablePrefix + "UserTokens", schema);

                b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserTokenConsts.MaxLoginProviderLength).IsRequired();
                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserTokenConsts.MaxNameLength).IsRequired();
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable(tablePrefix + "Roles", schema);

                b.Property(r => r.Name).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNameLength);
                b.Property(r => r.NormalizedName).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNormalizedNameLength);

                b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

                b.HasIndex(r => r.NormalizedName);
            });

            builder.Entity<IdentityRoleClaim>(b =>
            {
                b.ToTable(tablePrefix + "RoleClaims", schema);

                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityRoleClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityRoleClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.RoleId);
            });
        }
    }
}
