using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    [ConnectionStringName(DocsConsts.ConnectionStringName)]
    public interface IDocsDbContext : IEfCoreDbContext
    {
        DbSet<Project> Projects { get; set; }
    }
}