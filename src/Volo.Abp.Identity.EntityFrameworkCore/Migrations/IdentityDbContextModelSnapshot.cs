using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    partial class IdentityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Volo.Abp.Identity.IdentityRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName");

                    b.ToTable("IdentityRoles");
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityRoleClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(1024);

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("IdentityRoleClaims");
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("LockoutEnabled")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail");

                    b.HasIndex("NormalizedUserName");

                    b.ToTable("IdentityUsers");
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(1024);

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("IdentityUserClaims");
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUserLogin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("ProviderDisplayName")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId", "LoginProvider", "ProviderKey");

                    b.ToTable("IdentityUserLogins");
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("IdentityUserId1");

                    b.Property<Guid>("RoleId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId1");

                    b.HasIndex("RoleId", "UserId");

                    b.HasIndex("UserId", "RoleId");

                    b.ToTable("IdentityUserRoles");
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUserToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Name");

                    b.Property<Guid>("UserId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "LoginProvider", "Name");

                    b.ToTable("IdentityUserTokens");
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityRoleClaim", b =>
                {
                    b.HasOne("Volo.Abp.Identity.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUserClaim", b =>
                {
                    b.HasOne("Volo.Abp.Identity.IdentityUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUserLogin", b =>
                {
                    b.HasOne("Volo.Abp.Identity.IdentityUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUserRole", b =>
                {
                    b.HasOne("Volo.Abp.Identity.IdentityUser")
                        .WithMany("Roles")
                        .HasForeignKey("IdentityUserId1");

                    b.HasOne("Volo.Abp.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Volo.Abp.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Volo.Abp.Identity.IdentityUserToken", b =>
                {
                    b.HasOne("Volo.Abp.Identity.IdentityUser")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
