using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;
using AutoFiltererSorting = global::AutoFilterer.Enums.Sorting;

namespace Volo.Abp.AutoFilterer
{
    [PossibleSortings(typeof(AuditedAggregateRoot))]
    public class AbpPaginationFilterBase : PaginationFilterBase, IPagedAndSortedResultRequest
    {
        public AbpPaginationFilterBase()
        {
            // As default.
            this.Sort = nameof(AuditedAggregateRoot.CreationTime);
            this.SortBy = AutoFiltererSorting.Descending;
        }

        public int SkipCount { get => (Page - 1) * PerPage; set => Page = (value / PerPage) + 1; }
        public int MaxResultCount { get => PerPage; set => PerPage = value; }


        [IgnoreFilter]
        public override int Page { get => base.Page; set => base.Page = value; }

        [IgnoreFilter]
        public override int PerPage { get => base.PerPage; set => base.PerPage = value; }

        public string Sorting
        {
            get => base.Sort + " " + GetAbpStringKeyword(this.SortBy);
            set => SetSortingByAbpKeyword(value);
        }

        private void SetSortingByAbpKeyword(string keyword)
        {
            var splitted = keyword.Split(' ');
            this.Sort = Pascalize(splitted[0]);
            this.SortBy = GetSortingFromKeyword(splitted.Length > 0 ? splitted[1] : default);
        }

        private static string GetAbpStringKeyword(AutoFiltererSorting sorting)
        {
            return sorting == AutoFiltererSorting.Descending ? "DESC" : "ASC";
        }
        private static AutoFiltererSorting GetSortingFromKeyword(string keyword)
        {
            return keyword == "DESC" ? AutoFiltererSorting.Descending : AutoFiltererSorting.Ascending;
        }

        static string Pascalize(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
