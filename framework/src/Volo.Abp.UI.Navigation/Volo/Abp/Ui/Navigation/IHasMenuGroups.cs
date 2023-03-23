namespace Volo.Abp.UI.Navigation;

public interface IHasMenuGroups
{
    /// <summary>
    /// Menu groups.
    /// </summary>
    ApplicationMenuGroupList Groups { get; }
}
