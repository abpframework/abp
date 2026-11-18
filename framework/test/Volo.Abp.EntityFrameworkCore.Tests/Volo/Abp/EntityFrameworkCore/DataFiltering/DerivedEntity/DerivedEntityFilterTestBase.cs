using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

// A derived entity type that implements ISoftDelete / IMultiTenant while its base entity type does not
// must still be filtered, although EF Core accepts query filters on the root of the hierarchy only.
public abstract class DerivedEntityFilterTestBase<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private static readonly Guid TenantA = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid TenantB = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    private readonly ICurrentTenant _currentTenant;
    private readonly IDataFilter _dataFilter;
    private readonly IRepository<Notice, Guid> _noticeRepository;
    private readonly IRepository<TenantNotice, Guid> _tenantNoticeRepository;
    private readonly IRepository<Announcement, Guid> _announcementRepository;
    private readonly IRepository<TenantAnnouncement, Guid> _tenantAnnouncementRepository;

    protected abstract bool UseDbFunction { get; }

    protected DerivedEntityFilterTestBase()
    {
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _dataFilter = GetRequiredService<IDataFilter>();
        _noticeRepository = GetRequiredService<IRepository<Notice, Guid>>();
        _tenantNoticeRepository = GetRequiredService<IRepository<TenantNotice, Guid>>();
        _announcementRepository = GetRequiredService<IRepository<Announcement, Guid>>();
        _tenantAnnouncementRepository = GetRequiredService<IRepository<TenantAnnouncement, Guid>>();
    }

    [Fact]
    public async Task Should_Filter_Derived_Entity_That_Adds_SoftDelete_And_MultiTenant_To_Unfiltered_Root()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var dbContext = await _tenantNoticeRepository.GetDbContextAsync();
            var useDbFunction = dbContext is DerivedEntityTptFilterTestDbContext tptContext
                ? tptContext.UseDbFunction()
                : dbContext.As<DerivedEntityFilterTestDbContext>().UseDbFunction();
            useDbFunction.ShouldBe(UseDbFunction);

            await _noticeRepository.InsertAsync(new Notice { Text = "Host-wide" });
            using (_currentTenant.Change(TenantA))
            {
                await _tenantNoticeRepository.InsertAsync(new TenantNotice { Text = "A-live" });
                await _tenantNoticeRepository.InsertAsync(new TenantNotice { Text = "A-deleted", IsDeleted = true });
            }

            using (_currentTenant.Change(TenantB))
            {
                await _tenantNoticeRepository.InsertAsync(new TenantNotice { Text = "B-live" });
            }
        });

        await WithUnitOfWorkAsync(async () =>
        {
            using (_currentTenant.Change(TenantA))
            {
                (await GetTextsAsync(_tenantNoticeRepository, x => x.Text)).ShouldBe(new[] { "A-live" });
                (await _tenantNoticeRepository.FindAsync(x => x.Text == "B-live")).ShouldBeNull();

                // Rows of the root type itself are not filtered
                (await GetTextsAsync(_noticeRepository, x => x.Text)).ShouldBe(new[] { "A-live", "Host-wide" });

                using (_dataFilter.Disable<ISoftDelete>())
                {
                    (await GetTextsAsync(_tenantNoticeRepository, x => x.Text)).ShouldBe(new[] { "A-deleted", "A-live" });
                }

                using (_dataFilter.Disable<IMultiTenant>())
                {
                    (await GetTextsAsync(_tenantNoticeRepository, x => x.Text)).ShouldBe(new[] { "A-live", "B-live" });
                    (await GetTextsAsync(_noticeRepository, x => x.Text)).ShouldBe(new[] { "A-live", "B-live", "Host-wide" });
                }
            }

            using (_currentTenant.Change(null))
            {
                (await GetTextsAsync(_tenantNoticeRepository, x => x.Text)).ShouldBeEmpty();
                (await GetTextsAsync(_noticeRepository, x => x.Text)).ShouldBe(new[] { "Host-wide" });
            }
        });
    }

    [Fact]
    public async Task Should_Filter_Derived_Entity_That_Adds_MultiTenant_To_SoftDelete_Filtered_Root()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            await _announcementRepository.InsertAsync(new Announcement { Text = "Host-live" });
            await _announcementRepository.InsertAsync(new Announcement { Text = "Host-deleted", IsDeleted = true });
            using (_currentTenant.Change(TenantA))
            {
                await _tenantAnnouncementRepository.InsertAsync(new TenantAnnouncement { Text = "A-live" });
                await _tenantAnnouncementRepository.InsertAsync(new TenantAnnouncement { Text = "A-deleted", IsDeleted = true });
            }

            using (_currentTenant.Change(TenantB))
            {
                await _tenantAnnouncementRepository.InsertAsync(new TenantAnnouncement { Text = "B-live" });
            }
        });

        await WithUnitOfWorkAsync(async () =>
        {
            using (_currentTenant.Change(TenantA))
            {
                (await GetTextsAsync(_tenantAnnouncementRepository, x => x.Text)).ShouldBe(new[] { "A-live" });
                (await GetTextsAsync(_announcementRepository, x => x.Text)).ShouldBe(new[] { "A-live", "Host-live" });

                using (_dataFilter.Disable<ISoftDelete>())
                {
                    (await GetTextsAsync(_tenantAnnouncementRepository, x => x.Text)).ShouldBe(new[] { "A-deleted", "A-live" });
                    (await GetTextsAsync(_announcementRepository, x => x.Text)).ShouldBe(new[] { "A-deleted", "A-live", "Host-deleted", "Host-live" });
                }

                using (_dataFilter.Disable<IMultiTenant>())
                {
                    (await GetTextsAsync(_tenantAnnouncementRepository, x => x.Text)).ShouldBe(new[] { "A-live", "B-live" });
                }
            }

            using (_currentTenant.Change(null))
            {
                (await GetTextsAsync(_tenantAnnouncementRepository, x => x.Text)).ShouldBeEmpty();
                (await GetTextsAsync(_announcementRepository, x => x.Text)).ShouldBe(new[] { "Host-live" });
            }
        });
    }

    private static async Task<string[]> GetTextsAsync<TEntity>(IRepository<TEntity, Guid> repository, Func<TEntity, string> text)
        where TEntity : class, IEntity<Guid>
    {
        return (await repository.GetListAsync()).Select(text).OrderBy(x => x).ToArray();
    }
}
