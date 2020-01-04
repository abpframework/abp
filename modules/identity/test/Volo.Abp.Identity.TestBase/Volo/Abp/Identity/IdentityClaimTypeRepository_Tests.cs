﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity
{
    public abstract class IdentityClaimTypeRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IIdentityClaimTypeRepository ClaimTypeRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public IdentityClaimTypeRepository_Tests()
        {
           ClaimTypeRepository = ServiceProvider.GetRequiredService<IIdentityClaimTypeRepository>();
            GuidGenerator = ServiceProvider.GetRequiredService<IGuidGenerator>();
        }

        [Fact]
        public async Task Should_Check_Name_If_It_Is_Uniquee()
        {
            var claim = (await ClaimTypeRepository.GetListAsync()).FirstOrDefault();

            var result1 = await ClaimTypeRepository.AnyAsync(claim.Name).ConfigureAwait(false);

            result1.ShouldBe(true);

            var result2 = await ClaimTypeRepository.AnyAsync(Guid.NewGuid().ToString()).ConfigureAwait(false);

            result2.ShouldBe(false);
        }
    }
}
