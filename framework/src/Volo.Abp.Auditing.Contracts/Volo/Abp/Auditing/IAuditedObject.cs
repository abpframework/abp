namespace Volo.Abp.Auditing;

/// <summary>
/// This interface can be implemented to add standard auditing properties to a class.
/// </summary>
public interface IAuditedObject : ICreationAuditedObject, IModificationAuditedObject
{

}

/// <summary>
/// Extends <see cref="IAuditedObject"/> to add user navigation properties.
/// </summary>
/// <typeparam name="TUser">Type of the user</typeparam>
public interface IAuditedObject<TUser> : IAuditedObject, ICreationAuditedObject<TUser>, IModificationAuditedObject<TUser>
{

}
