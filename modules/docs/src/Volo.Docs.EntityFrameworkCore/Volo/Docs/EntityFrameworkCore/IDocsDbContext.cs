using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(DocsDbProperties.ConnectionStringName)]
    public interface IDocsDbContext : IEfCoreDbContext
    {
        DbSet<Project> Projects { get; }

        DbSet<Document> Documents { get; }

        DbSet<DocumentContributor> DocumentContributors { get; }
    }
}
