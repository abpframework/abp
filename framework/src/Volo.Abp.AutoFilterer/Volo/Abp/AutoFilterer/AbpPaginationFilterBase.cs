using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.AutoFilterer.Volo.Abp.AutoFilterer
{
    [PossibleSortings(typeof(AuditedAggregateRoot))]
    public class AbpPaginationFilterBase : PaginationFilterBase, IPagedAndSortedResultRequest
    {
        public AbpPaginationFilterBase()
        {
            // As default.
            this.Sort = nameof(AuditedAggregateRoot.CreationTime);
            this.SortBy = Sorting.Descending;
        }

        int IPagedResultRequest.SkipCount { get => Page; set => Page = value; }
        int ILimitedResultRequest.MaxResultCount { get => PerPage; set => PerPage = value; }
        string ISortedResultRequest.Sorting
        {
            get => base.Sort + " " + GetAbpStringKeyword(base.SortBy);
            set => SetSortingByAbpKeyword(value);
        }

        private void SetSortingByAbpKeyword(string keyword)
        {
            var splitted = keyword.Split(' ');
            this.Sort = splitted[0];
            this.SortBy = GetSortingFromKeyword(splitted.Length > 0 ? splitted[1] : default);
        }

        private static string GetAbpStringKeyword(Sorting sorting)
        {
            return sorting == Sorting.Descending ? "DESC" : "ASC";
        }
        private static Sorting GetSortingFromKeyword(string keyword)
        {
            return keyword == "DESC" ? Sorting.Descending : Sorting.Ascending;
        }
    }
}
