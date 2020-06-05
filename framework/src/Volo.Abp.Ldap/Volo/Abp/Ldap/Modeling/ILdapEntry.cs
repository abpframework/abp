namespace Volo.Abp.Ldap.Modeling
{
    public interface ILdapEntry
    {
        string ObjectCategory { get; set; }
        string[] ObjectClass { get; set; }
        string Name { get; set; }
        string DistinguishedName { get; set; }
        string CommonName { get; set; }
    }
}