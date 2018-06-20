using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    [ConnectionStringName("Docs")]
    public interface IDocsDbContext : IEfCoreDbContext
    {
        DbSet<Project> Projects { get; set; }
    }
}