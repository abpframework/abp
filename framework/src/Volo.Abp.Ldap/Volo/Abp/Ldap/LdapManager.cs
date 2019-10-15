using System;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ldap.Exceptions;
using Volo.Abp.Ldap.Modeling;

namespace Volo.Abp.Ldap
{
    public class LdapManager : ILdapManager, ITransientDependency
    {
        private readonly string _searchBase;
        private readonly AbpLdapOptions _ldapOptions;

        private readonly string[] _attributes =
        {
            "objectCategory", "objectClass", "cn", "name", "distinguishedName",
            "ou",
            "sAMAccountName", "userPrincipalName", "telephoneNumber", "mail"
        };

        public LdapManager(IOptions<AbpLdapOptions> ldapSettingsOptions)
        {
            _ldapOptions = ldapSettingsOptions.Value;
            _searchBase = _ldapOptions.SearchBase;
        }

        #region Organization
        /// <summary>
        /// query the specified organizations.
        /// 
        /// filter: (&(name=xxx)(objectClass=organizationalUnit)) when name is not null
        /// filter: (&(objectClass=organizationalUnit)) when name is null
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<LdapOrganization> GetOrganizations(string name = null)
        {
            var conditions = new Dictionary<string, string>
            {
                {"name", name},
                {"objectClass", "organizationalUnit"},
            };
            return Query<LdapOrganization>(_searchBase, conditions);
        }

        /// <summary>
        /// query the specified organization.
        /// 
        /// filter: (&(distinguishedName=xxx)(objectClass=organizationalUnit)) when organizationName is not null
        /// 
        /// </summary>
        /// <param name="distinguishedName"></param>
        /// <returns></returns>
        public LdapOrganization GetOrganization(string distinguishedName)
        {
            distinguishedName = Check.NotNullOrWhiteSpace(distinguishedName, nameof(distinguishedName));
            var conditions = new Dictionary<string, string>
            {
                {"distinguishedName", distinguishedName},
                {"objectClass", "organizationalUnit"},
            };
            return QueryOne<LdapOrganization>(_searchBase, conditions);
        }

        public void AddSubOrganization(string organizationName, LdapOrganization parentOrganization)
        {
            organizationName = Check.NotNullOrWhiteSpace(organizationName, nameof(organizationName));
            var dn = $"OU={organizationName},{parentOrganization.DistinguishedName}";

            var attributeSet = new LdapAttributeSet
            {
                new LdapAttribute("objectCategory", $"CN=Organizational-Unit,CN=Schema,CN=Configuration,{_ldapOptions.DomainDistinguishedName}"),
                new LdapAttribute("objectClass", new[] {"top", "organizationalUnit"}),
                new LdapAttribute("name", organizationName),
            };

            var newEntry = new LdapEntry(dn, attributeSet);

            using (var ldapConnection = GetConnection())
            {
                ldapConnection.Add(newEntry);
            }
        }

        public void AddSubOrganization(string organizationName, string parentDistinguishedName)
        {
            organizationName = Check.NotNullOrWhiteSpace(organizationName, nameof(organizationName));
            parentDistinguishedName =
                Check.NotNullOrWhiteSpace(parentDistinguishedName, nameof(parentDistinguishedName));

            var parentOrganization = GetOrganization(parentDistinguishedName);
            if (null == parentOrganization)
            {
                throw new OrganizationNotExistException(parentDistinguishedName);
            }

            AddSubOrganization(organizationName, parentOrganization);
        }

        #endregion

        #region User
        /// <summary>
        /// query the specified users.
        /// 
        /// filter: (&(name=xxx)(objectCategory=person)(objectClass=user)) when name is not null
        /// filter: (&(objectCategory=person)(objectClass=user)) when name is null
        ///
        /// filter: (&(displayName=xxx)(objectCategory=person)(objectClass=user)) when displayName is not null
        /// filter: (&(objectCategory=person)(objectClass=user)) when displayName is null
        ///
        /// filter: (&(cn=xxx)(objectCategory=person)(objectClass=user)) when commonName is not null
        /// filter: (&(objectCategory=person)(objectClass=user)) when commonName is null
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="commonName"></param>
        /// <returns></returns>
        public IList<LdapUser> GetUsers(string name = null, string displayName = null, string commonName = null)
        {
            var conditions = new Dictionary<string, string>
            {
                {"objectCategory", "person"},
                {"objectClass", "user"},
                {"name", name},
                {"displayName", displayName},
                {"cn", commonName},
            };
            return Query<LdapUser>(_searchBase, conditions);
        }

        /// <summary>
        /// query the specified User.
        /// 
        /// filter: (&(distinguishedName=xxx)(objectCategory=person)(objectClass=user)) when distinguishedName is not null
        /// 
        /// </summary>
        /// <param name="distinguishedName"></param>
        /// <returns></returns>
        public LdapUser GetUser(string distinguishedName)
        {
            distinguishedName = Check.NotNullOrWhiteSpace(distinguishedName, nameof(distinguishedName));
            var conditions = new Dictionary<string, string>
            {
                {"objectCategory", "person"},
                {"objectClass", "user"},
                {"distinguishedName", distinguishedName},
            };
            return QueryOne<LdapUser>(_searchBase, conditions);
        }

        public void AddUserToOrganization(string userName, string password, LdapOrganization parentOrganization)
        {
            var dn = $"CN={userName},{parentOrganization.DistinguishedName}";
            var mail = $"{userName}@{_ldapOptions.DomainName}";
            sbyte[] encodedBytes = SupportClass.ToSByteArray(Encoding.Unicode.GetBytes($"\"{password}\""));

            var attributeSet = new LdapAttributeSet
            {
                new LdapAttribute("instanceType", "4"),
                new LdapAttribute("objectCategory", $"CN=Person,CN=Schema,CN=Configuration,{_ldapOptions.DomainDistinguishedName}"),
                new LdapAttribute("objectClass", new[] {"top", "person", "organizationalPerson", "user"}),
                new LdapAttribute("name", userName),
                new LdapAttribute("cn", userName),
                new LdapAttribute("sAMAccountName", userName),
                new LdapAttribute("userPrincipalName", userName),
                new LdapAttribute("sn", userName),
                new LdapAttribute("displayName", userName),
                new LdapAttribute("unicodePwd", encodedBytes),
                new LdapAttribute("userAccountControl",  "512"),
                new LdapAttribute("mail", mail),
            };
            var newEntry = new LdapEntry(dn, attributeSet);

            using (var ldapConnection = GetConnection())
            {
                ldapConnection.Add(newEntry);
            }
        }

        public void AddUserToOrganization(string userName, string password, string parentDistinguishedName)
        {
            var dn = $"CN={userName},{parentDistinguishedName}";
            var mail = $"{userName}@{_ldapOptions.DomainName}";
            sbyte[] encodedBytes = SupportClass.ToSByteArray(Encoding.Unicode.GetBytes($"\"{password}\""));

            var attributeSet = new LdapAttributeSet
            {
                new LdapAttribute("instanceType", "4"),
                new LdapAttribute("objectCategory", $"CN=Person,CN=Schema,CN=Configuration,{_ldapOptions.DomainDistinguishedName}"),
                new LdapAttribute("objectClass", new[] {"top", "person", "organizationalPerson", "user"}),
                new LdapAttribute("name", userName),
                new LdapAttribute("cn", userName),
                new LdapAttribute("sAMAccountName", userName),
                new LdapAttribute("userPrincipalName", userName),
                new LdapAttribute("sn", userName),
                new LdapAttribute("displayName", userName),
                new LdapAttribute("unicodePwd", encodedBytes),
                new LdapAttribute("userAccountControl",  "512"),
                new LdapAttribute("mail", mail),
            };
            var newEntry = new LdapEntry(dn, attributeSet);

            using (var ldapConnection = GetConnection())
            {
                ldapConnection.Add(newEntry);
            }
        }

        #endregion

        #region Authenticate

        /// <summary>
        /// Authenticate 
        /// </summary>
        /// <param name="userDomainName">E.g administrator@yourdomain.com.cn </param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Authenticate(string userDomainName, string password)
        {
            try
            {
                using (GetConnection(userDomainName, password))
                {
                    return true;
                }
            }
            catch (Exception )
            {
                return false;
            }
        }

        #endregion

        private ILdapConnection GetConnection(string bindUserName = null, string bindUserPassword = null)
        {
            // bindUserName/bindUserPassword only be used when authenticate
            bindUserName = bindUserName ?? _ldapOptions.Credentials.DomainUserName;
            bindUserPassword = bindUserPassword ?? _ldapOptions.Credentials.Password;

            var ldapConnection = new LdapConnection() { SecureSocketLayer = _ldapOptions.UseSsl };
            if (_ldapOptions.UseSsl)
            {
                ldapConnection.UserDefinedServerCertValidationDelegate += (sender, certificate, chain, sslPolicyErrors) => true;
            }
            ldapConnection.Connect(_ldapOptions.ServerHost, _ldapOptions.ServerPort);

            if (_ldapOptions.UseSsl)
            {
                ldapConnection.Bind(LdapConnection.Ldap_V3, bindUserName, bindUserPassword);
            }
            else
            {
                ldapConnection.Bind(bindUserName, bindUserPassword);
            }
            return ldapConnection;
        }

        private IList<T> Query<T>(string searchBase, Dictionary<string, string> conditions) where T : class, ILdapEntry
        {
            var filter = LdapHelps.BuildFilter(conditions);

            var result = new List<T>();

            using (var ldapConnection = GetConnection())
            {
                var search = ldapConnection.Search(searchBase, LdapConnection.SCOPE_SUB, filter,
                    _attributes, false, null, null);

                LdapMessage message;
                while ((message = search.getResponse()) != null)
                {
                    if (!(message is LdapSearchResult searchResultMessage))
                    {
                        continue;
                    }
                    var entry = searchResultMessage.Entry;
                    if (typeof(T) == typeof(LdapOrganization))
                    {
                        result.Add(new LdapOrganization(entry.getAttributeSet()) as T);
                    }

                    if (typeof(T) == typeof(LdapUser))
                    {
                        result.Add(new LdapUser(entry.getAttributeSet()) as T);
                    }
                }
            }
            return result;
        }

        private T QueryOne<T>(string searchBase, Dictionary<string, string> conditions) where T : class, ILdapEntry
        {
            var filter = LdapHelps.BuildFilter(conditions);

            using (var ldapConnection = GetConnection())
            {
                var search = ldapConnection.Search(searchBase, LdapConnection.SCOPE_SUB, filter,
                    _attributes, false, null, null);

                LdapMessage message;
                while ((message = search.getResponse()) != null)
                {
                    if (!(message is LdapSearchResult searchResultMessage))
                    {
                        continue;
                    }
                    var entry = searchResultMessage.Entry;
                    if (typeof(T) == typeof(LdapOrganization))
                    {
                        return new LdapOrganization(entry.getAttributeSet()) as T;
                    }

                    if (typeof(T) == typeof(LdapUser))
                    {
                        return new LdapUser(entry.getAttributeSet()) as T;
                    }
                    return null;
                }
            }
            return null;
        }

    }
}