using Novell.Directory.Ldap;

namespace Volo.Abp.Ldap.Modeling
{
    public abstract class LdapEntryBase : ILdapEntry
    {
        public string ObjectCategory { get; set; }
        public string[] ObjectClass { get; set; }
        public string Name { get; set; }
        public string CommonName { get; set; }
        public string DistinguishedName { get; set; }

        protected LdapEntryBase() { }

        protected LdapEntryBase(LdapAttributeSet attributeSet)
        {
            ObjectCategory = attributeSet.getAttribute("objectCategory")?.StringValue;
            ObjectClass = attributeSet.getAttribute("objectClass")?.StringValueArray;
            Name = attributeSet.getAttribute("name")?.StringValue;
            CommonName = attributeSet.getAttribute("cn")?.StringValue;
            DistinguishedName = attributeSet.getAttribute("distinguishedName")?.StringValue;
        }
    }
}