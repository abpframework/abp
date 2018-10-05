using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(AbpIdentityServerMongoDbTestModule))]
    public class AbpIdentityServerDomainTestModule : AbpModule
    {

    }
}
