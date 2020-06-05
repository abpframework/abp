using Novell.Directory.Ldap;

namespace Volo.Abp.Ldap.Modeling
{
    public class LdapUser : LdapEntryBase, ILdapUser
    {
        public string SamAccountName { get; set; }
        public string UserPrincipalName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public LdapUser() { }

        public LdapUser( LdapAttributeSet attributeSet)
            : base(attributeSet)
        {
            SamAccountName = attributeSet.getAttribute("sAMAccountName")?.StringValue;
            UserPrincipalName = attributeSet.getAttribute("userPrincipalName")?.StringValue;
            DisplayName = attributeSet.getAttribute("displayName")?.StringValue;
            Email = attributeSet.getAttribute("mail")?.StringValue;
            Phone = attributeSet.getAttribute("telephoneNumber")?.StringValue;
        }
    }
}