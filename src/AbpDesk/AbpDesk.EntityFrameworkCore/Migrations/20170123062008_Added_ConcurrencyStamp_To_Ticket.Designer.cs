using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AbpDesk.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(AbpDeskDbContext))]
    [Migration("20170123062008_Added_ConcurrencyStamp_To_Ticket")]
    partial class Added_ConcurrencyStamp_To_Ticket
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AbpDesk.Tickets.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .HasMaxLength(65536);

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("DskTickets");
                });
        }
    }
}
