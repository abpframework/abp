namespace Volo.Abp.Ldap.Modeling
{
    public interface ILdapUser : ILdapEntry
    {
        string SamAccountName { get; set; }
        string UserPrincipalName { get; set; }
        string DisplayName { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
    }
}