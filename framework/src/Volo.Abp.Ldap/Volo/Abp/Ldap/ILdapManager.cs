using System.Collections.Generic;
using Volo.Abp.Ldap.Modeling;

namespace Volo.Abp.Ldap
{
    public interface ILdapManager
    {
        /// <summary>
        /// query the specified organizations.
        /// 
        /// filter: (&(name=xxx)(objectClass=organizationalUnit)) when name is not null
        /// filter: (&(name=*)(objectClass=organizationalUnit)) when name is null
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<LdapOrganization> GetOrganizations(string name = null);

        /// <summary>
        /// query the specified organization.
        /// 
        /// filter: (&(distinguishedName=xxx)(objectClass=organizationalUnit)) when organizationName is not null
        /// 
        /// </summary>
        /// <param name="distinguishedName"></param>
        /// <returns></returns>
        LdapOrganization GetOrganization(string distinguishedName);

        void AddSubOrganization(string organizationName, LdapOrganization parentOrganization);
        void AddSubOrganization(string organizationName, string parentDistinguishedName);

        /// <summary>
        /// query the specified users.
        /// 
        /// filter: (&(name=xxx)(objectCategory=person)(objectClass=user)) when name is not null
        /// filter: (&(name=*)(objectCategory=person)(objectClass=user)) when name is null
        ///
        /// filter: (&(displayName=xxx)(objectCategory=person)(objectClass=user)) when displayName is not null
        /// filter: (&(displayName=*)(objectCategory=person)(objectClass=user)) when displayName is null
        ///
        /// filter: (&(cn=xxx)(objectCategory=person)(objectClass=user)) when commonName is not null
        /// filter: (&(cn=*)(objectCategory=person)(objectClass=user)) when commonName is null
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="commonName"></param>
        /// <returns></returns>
        IList<LdapUser> GetUsers(string name = null, string displayName = null, string commonName = null);

        /// <summary>
        /// query the specified User.
        /// 
        /// filter: (&(distinguishedName=xxx)(objectCategory=person)(objectClass=user)) when distinguishedName is not null
        /// 
        /// </summary>
        /// <param name="distinguishedName"></param>
        /// <returns></returns>
        LdapUser GetUser(string distinguishedName);

        void AddUserToOrganization(string userName, string password, LdapOrganization parentOrganization);
        void AddUserToOrganization(string userName, string password, string parentDistinguishedName);

        /// <summary>
        /// Authenticate 
        /// </summary>
        /// <param name="userDomainName">E.g administrator@yourdomain.com.cn </param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Authenticate(string userDomainName, string password);
    }
}