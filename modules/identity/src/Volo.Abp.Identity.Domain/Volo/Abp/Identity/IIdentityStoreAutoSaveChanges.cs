namespace Volo.Abp.Identity
{
    public interface IIdentityStoreAutoSaveChanges
    {
        bool AutoSaveChanges { get; set; }
    }
}