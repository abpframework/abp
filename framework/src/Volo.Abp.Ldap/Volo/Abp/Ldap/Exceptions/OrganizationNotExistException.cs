namespace Volo.Abp.Ldap.Exceptions
{
    public class OrganizationNotExistException : BusinessException
    {
        public OrganizationNotExistException(string distinguishedName)
            : base("LDAP:000001", $"the organization distinguished named {distinguishedName} does not exist.")
        {

        }
    }
}