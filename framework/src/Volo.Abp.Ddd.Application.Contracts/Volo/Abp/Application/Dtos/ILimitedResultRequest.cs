namespace Volo.Abp.Application.Dtos;

/// <summary>
/// This interface is defined to standardize to request a limited result.
/// </summary>
public interface ILimitedResultRequest
{
    /// <summary>
    /// Maximum result count should be returned.
    /// This is generally used to limit result count on paging.
    /// </summary>
    int MaxResultCount { get; set; }
}
