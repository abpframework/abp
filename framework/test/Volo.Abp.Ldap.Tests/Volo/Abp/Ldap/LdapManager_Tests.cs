using System;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Ldap
{

    public class LdapManager_Tests : AbpIntegratedTest<LdapManager_Tests.TestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        private readonly ILdapManager _ldapManager;
        private readonly LdapTestData _testData;

        public LdapManager_Tests()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            _testData = GetRequiredService<LdapTestData>();
            _ldapManager = GetRequiredService<ILdapManager>();
        }

        [Fact(Skip = "need environment AD ")]
        public void GetOrganizations_With_Empty_Condition()
        {
            var result = _ldapManager.GetOrganizations();

            result.ShouldNotBeNull();
            result.ShouldContain(e => e.Name == _testData.DomainControllersName);
            result.ShouldContain(e => e.DistinguishedName == _testData.DomainControllersDistinguishedName);
        }

        [Fact(Skip = "need environment AD ")]
        public void GetOrganizations_With_Name()
        {
            var result = _ldapManager.GetOrganizations(_testData.DomainControllersName);

            result.ShouldNotBeNull();
            result.ShouldHaveSingleItem();
            result.ShouldContain(e => e.Name == _testData.DomainControllersName);
            result.ShouldContain(e => e.DistinguishedName == _testData.DomainControllersDistinguishedName);
        }

        [Fact(Skip = "need environment AD ")]
        public void GetOrganizations_With_Non_Existent_Name()
        {
            var result = _ldapManager.GetOrganizations("NonExistentNameA");

            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact(Skip = "need environment AD ")]
        public void GetOrganization()
        {
            var result = _ldapManager.GetOrganization(_testData.DomainControllersDistinguishedName);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(_testData.DomainControllersName);
        }

        [Fact(Skip = "need environment AD ")]
        public void GetOrganization_With_Non_Existent_DistinguishedName()
        {
            var result = _ldapManager.GetOrganization("NonExistentNameA");

            result.ShouldBeNull();
        }

        [Fact(Skip = "need environment AD ")]
        public void GetUsers_With_Empty_Condition()
        {
            var result = _ldapManager.GetUsers();

            result.ShouldNotBeNull();
            result.ShouldContain(e => e.Name == _testData.AdministratorName);
        }

        [Fact(Skip = "need environment AD ")]
        public void GetUsers_With_Name()
        {
            var result = _ldapManager.GetUsers(name: _testData.AdministratorName);

            result.ShouldNotBeNull();
            result.ShouldContain(e => e.Name == _testData.AdministratorName);
        }

        [Fact(Skip = "need environment AD ")]
        public void GetUsers_With_Non_Existent_Name()
        {
            var result = _ldapManager.GetUsers(name: "NonExistentNameA");

            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact(Skip = "need environment AD ")]
        public void GetUsers_With_CommonName()
        {
            var result = _ldapManager.GetUsers(commonName: _testData.AdministratorName);

            result.ShouldNotBeNull();
            result.ShouldContain(e => e.Name == _testData.AdministratorName);
        }

        [Fact(Skip = "need environment AD ")]
        public void GetUsers_With_Non_Existent_CommonName()
        {
            var result = _ldapManager.GetUsers(commonName: "NonExistentNameA");

            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact(Skip = "need environment AD ")]
        public void GetUsers_With_DisplayName()
        {
            var result = _ldapManager.GetUsers(displayName: _testData.AdministratorName);

            result.ShouldNotBeNull();
            // the administrator in AD. not have display name by default.
            result.ShouldBeEmpty();
        }

        [Fact(Skip = "need environment AD ")]
        public void GetUser()
        {
            var result = _ldapManager.GetUser(_testData.AdministratorDistinguishedName);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(_testData.AdministratorName);
        }

        [Fact(Skip = "need environment AD ")]
        public void GetUser_With_Non_Existent_DistinguishedName()
        {
            var result = _ldapManager.GetOrganization("NonExistentNameA");

            result.ShouldBeNull();
        }

        [Fact(Skip = "need environment AD ")]
        public void Authenticate()
        {
            var result = _ldapManager.Authenticate(_testData.AdministratorDomainName, _testData.AdministratorPassword);

            result.ShouldBeTrue();
        }

        [Fact(Skip = "need environment AD ")]
        public void Authenticate_With_Wrong_Password()
        {
            var result = _ldapManager.Authenticate("NonExistentNameA", "PasswordA");

            result.ShouldBeFalse();
        }

        [Fact(Skip = "need environment AD ")]
        public void AddSubOrganization()
        {
            var parentOrganization = _ldapManager.GetOrganization(_testData.DomainControllersDistinguishedName);
            var randomName = $"Test_{DateTime.Now.Ticks}";

            _ldapManager.AddSubOrganization(randomName, parentOrganization);

            var result = _ldapManager.GetOrganizations(randomName);
            result.ShouldNotBeNull();
            result.ShouldContain(e => e.Name == randomName);
        }

        [Fact(Skip = "need environment AD ")]
        public void AddSubOrganization_With_DistinguishedName()
        {
            var randomName = $"Test_{DateTime.Now.Ticks}";

            _ldapManager.AddSubOrganization(randomName, _testData.DomainControllersDistinguishedName);

            var result = _ldapManager.GetOrganizations(randomName);
            result.ShouldNotBeNull();
            result.ShouldContain(e => e.Name == randomName);
        }

        [Fact(Skip = "need environment AD ")]
        public void AddOrganizationUser()
        {
            var parentOrganization = _ldapManager.GetOrganization(_testData.DomainControllersDistinguishedName);
            var randomName = $"Test_{DateTime.Now:yyMMddHHmmss}";
            _ldapManager.AddUserToOrganization(randomName, _testData.AdministratorPassword, parentOrganization);

            var result = _ldapManager.GetUsers(randomName);
            result.ShouldNotBeNull();
            result.ShouldContain(e=>e.Name == randomName);
        }

        [DependsOn(typeof(AbpLdapModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                // not use ssl
                // "LDAP": {
                //     "ServerHost": "192.168.101.54",
                //     "ServerPort": 389,
                //     "UseSSL": false,
                //     "Credentials": {
                //         "DomainUserName": "administrator@yourdomain.com.cn",
                //         "Password": "yH.20190528"
                //     },
                //     "SearchBase": "CN=Users,DC=yourdomain,DC=com,DC=cn",
                //     "DomainName": "yourdomain.com.cn",
                //     "DomainDistinguishedName": "DC=yourdomain,DC=com,DC=cn"
                // }

                // use ssl
                // "LDAP": {
                //     "ServerHost": "192.168.101.54",
                //     "ServerPort": 636,
                //     "UseSSL": true,
                //     "Credentials": {
                //         "DomainUserName": "administrator@yourdomain.com.cn",
                //         "Password": "yH.20190528"
                //     },
                //     "SearchBase": "CN=Users,DC=yourdomain,DC=com,DC=cn",
                //     "DomainName": "yourdomain.com.cn",
                //     "DomainDistinguishedName": "DC=yourdomain,DC=com,DC=cn"
                // }
                Configure<AbpLdapOptions>(settings =>
                {
                    settings.ServerHost = "192.168.101.54";
                    settings.ServerPort = 636;
                    settings.UseSsl = true;
                    settings.Credentials.DomainUserName = "administrator@yourdomain.com.cn";
                    settings.Credentials.Password = "yH.20190528";
                    settings.SearchBase = "DC=yourdomain,DC=com,DC=cn";
                    settings.DomainName = "yourdomain.com.cn";
                    settings.DomainDistinguishedName = "DC=yourdomain,DC=com,DC=cn";
                });
            }
        }
    }

}