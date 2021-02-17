namespace Volo.Abp.Application.Dtos
{
    public interface IDetailedPagedResult<T> : IPagedResult<T>
    {
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        int PageSize { get; set; }
        long FirstRowOnPage { get; }
        long LastRowOnPage { get; }
    }
}
