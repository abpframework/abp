using System;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Ldap
{
    
    public class Authenticate_Tests : AbpIntegratedTest<Authenticate_Tests.TestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        private readonly ILdapManager _ldapManager;
        private readonly LdapTestData _testData;

        public Authenticate_Tests()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            _testData = GetRequiredService<LdapTestData>();
            _ldapManager = GetRequiredService<ILdapManager>();
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

        [DependsOn(typeof(AbpLdapModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                // not use ssl
                // "LDAP": {
                //     "ServerHost": "192.168.101.54",
                //     "ServerPort": 389,
                //     "UseSSL": false
                // }

                // use ssl
                // "LDAP": {
                //     "ServerHost": "192.168.101.54",
                //     "ServerPort": 636,
                //     "UseSSL": true
                // }
                Configure<AbpLdapOptions>(settings =>
                {
                    settings.ServerHost = "192.168.101.54";
                    settings.ServerPort = 636;
                    settings.UseSsl = true;
                });
            }
        }
    }

}