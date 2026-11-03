using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.AuditLogging;

public abstract class AuditLogExcelFileRepository_Tests<TStartupModule> : AuditLoggingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IAuditLogExcelFileRepository AuditLogExcelFileRepository { get; }

    protected AuditLogExcelFileRepository_Tests()
    {
        AuditLogExcelFileRepository = GetRequiredService<IAuditLogExcelFileRepository>();
    }

    [Fact]
    public async Task GetListCreationTimeBeforeAsync_Should_Return_Oldest_Files_First()
    {
        var now = DateTime.Now;
        foreach (var daysAgo in new[] { 2, 5, 9, 7, 3 })
        {
            var file = new AuditLogExcelFile(Guid.NewGuid(), $"file-{daysAgo}.xlsx");
            ObjectHelper.TrySetProperty(file, x => x.CreationTime, () => now.AddDays(-daysAgo));
            await AuditLogExcelFileRepository.InsertAsync(file, autoSave: true);
        }

        var files = await AuditLogExcelFileRepository.GetListCreationTimeBeforeAsync(now.AddDays(-3), maxResultCount: 2);

        files.Select(x => x.FileName).ShouldBe(new[] { "file-9.xlsx", "file-7.xlsx" });
    }
}
