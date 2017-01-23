using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext : AbpDbContext<IdentityDbContext>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityDbContext"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
            
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public DbSet<IdentityUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
        /// </summary>
        public DbSet<IdentityUserClaim> UserClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
        /// </summary>
        public DbSet<IdentityUserLogin> UserLogins { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User roles.
        /// </summary>
        public DbSet<IdentityUserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User tokens.
        /// </summary>
        public DbSet<IdentityUserToken> UserTokens { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
        /// </summary>
        public DbSet<IdentityRole> Roles { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of role claims.
        /// </summary>
        public DbSet<IdentityRoleClaim> RoleClaims { get; set; }

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Split configuration to dedicated classes

            //TODO: Set Default Values for properties
            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable("IdentityUsers");

                b.Property(u => u.UserName).HasMaxLength(IdentityUser.MaxUserNameLength);
                b.Property(u => u.NormalizedUserName).HasMaxLength(IdentityUser.MaxNormalizedUserNameLength);
                b.Property(u => u.Email).HasMaxLength(IdentityUser.MaxEmailLength);
                b.Property(u => u.NormalizedEmail).HasMaxLength(IdentityUser.MaxNormalizedEmailLength);
                b.Property(u => u.EmailConfirmed).HasDefaultValue(false);
                b.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false);
                b.Property(u => u.TwoFactorEnabled).HasDefaultValue(false);
                b.Property(u => u.LockoutEnabled).HasDefaultValue(false);
                b.Property(u => u.AccessFailedCount).HasDefaultValue(0);

                b.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.Tokens).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(u => u.NormalizedUserName);
                b.HasIndex(u => u.NormalizedEmail);
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("IdentityRoles");

                b.Property(u => u.Name).HasMaxLength(IdentityRole.MaxNameLength);
                b.Property(u => u.NormalizedName).HasMaxLength(IdentityRole.MaxNormalizedNameLength);

                b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

                b.HasIndex(r => r.NormalizedName);
            });

            builder.Entity<IdentityUserClaim>(b => 
            {
                b.ToTable("IdentityUserClaims");

                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityUserClaim.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityUserClaim.MaxClaimValueLength);

                b.HasIndex(uc => uc.UserId);
            });

            builder.Entity<IdentityRoleClaim>(b => 
            {
                b.ToTable("IdentityRoleClaims");

                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityRoleClaim.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityRoleClaim.MaxClaimValueLength);

                b.HasIndex(uc => uc.RoleId);
            });

            builder.Entity<IdentityUserRole>(b => 
            {
                b.ToTable("IdentityUserRoles");

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(r => new { r.UserId, r.RoleId });
                b.HasIndex(r => new { r.RoleId, r.UserId });
            });

            builder.Entity<IdentityUserLogin>(b =>
            {
                b.ToTable("IdentityUserLogins");

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserLogin.MaxLoginProviderLength).IsRequired();
                b.Property(ul => ul.ProviderKey).HasMaxLength(IdentityUserLogin.MaxProviderKeyLength).IsRequired();
                b.Property(ul => ul.ProviderDisplayName).HasMaxLength(IdentityUserLogin.MaxProviderDisplayNameLength);

                b.HasIndex(l => new { l.UserId, l.LoginProvider, l.ProviderKey });
                b.HasIndex(l => new { l.LoginProvider, l.ProviderKey });
            });

            builder.Entity<IdentityUserToken>(b => 
            {
                b.ToTable("IdentityUserTokens");

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserToken.MaxLoginProviderLength).IsRequired();

                b.HasIndex(l => new { l.UserId, l.LoginProvider, l.Name });
            });
        }
    }
}