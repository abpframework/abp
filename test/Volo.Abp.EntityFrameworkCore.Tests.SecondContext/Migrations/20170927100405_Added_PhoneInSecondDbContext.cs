using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.Abp.EntityFrameworkCore.Tests.SecondContext.Migrations
{
    public partial class Added_PhoneInSecondDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* SecondDbContext depends on TestAppDbContext,
               so no need to add migration for phones since TestAppDbContext already contains it */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
