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
            //TODO: Set Default Values for properties

            //TODO: Unique indexes can be a problem for a multi-tenant application. Think again.

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable("IdentityUsers");

                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                b.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.Tokens).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("IdentityRoles");

                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                //TODO: Relation & Foreign Key!
                //b.HasMany(r => r.Users).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

                b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
            });

            builder.Entity<IdentityUserClaim>(b => 
            {
                b.ToTable("IdentityUserClaims");
            });

            builder.Entity<IdentityRoleClaim>(b => 
            {
                b.ToTable("IdentityRoleClaims");
            });

            builder.Entity<IdentityUserRole>(b => 
            {
                b.ToTable("IdentityUserRoles");

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();

                b.HasIndex(r => new { r.UserId, r.RoleId }).IsUnique();
            });

            builder.Entity<IdentityUserLogin>(b =>
            {
                b.ToTable("IdentityUserLogins");

                b.HasIndex(l => new { l.UserId, l.LoginProvider, l.ProviderKey }).IsUnique();
            });

            builder.Entity<IdentityUserToken>(b => 
            {
                b.ToTable("IdentityUserTokens");

                b.HasIndex(l => new { l.UserId, l.LoginProvider, l.Name }).IsUnique();
            });
        }
    }
}