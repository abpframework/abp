namespace Volo.Abp;

/// <summary>
/// Used to standardize soft deleting entities.
/// Soft-delete entities are not actually deleted,
/// marked as IsDeleted = true in the database,
/// but can not be retrieved to the application normally.
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Used to mark an Entity as 'Deleted'. 
    /// </summary>
    bool IsDeleted { get; set; }
}
