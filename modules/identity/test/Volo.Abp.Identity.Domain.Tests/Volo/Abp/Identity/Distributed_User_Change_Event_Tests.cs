﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Testing.Utils;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Abp.Identity
{
    public class Distributed_User_Change_Event_Tests : AbpIdentityDomainTestBase
    {
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IdentityUserManager _userManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITestCounter _testCounter;

        public Distributed_User_Change_Event_Tests()
        {
            _userRepository = GetRequiredService<IIdentityUserRepository>();
            _userManager = GetRequiredService<IdentityUserManager>();
            _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            _testCounter = GetRequiredService<ITestCounter>();
        }

        [Fact]
        public async Task Should_Register_Handler()
        {
            var options = GetRequiredService<IOptions<AbpDistributedEventBusOptions>>().Value;
            options.EtoMappings.ShouldContain(m => m.Key == typeof(IdentityUser) && m.Value.EtoType == typeof(UserEto));
            options.Handlers.ShouldContain(h => h == typeof(DistributedUserUpdateHandler));
        }

        [Fact]
        public async Task Should_Trigger_Distributed_EntityUpdated_Event()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash")).ConfigureAwait(false);
                await _userManager.SetEmailAsync(user, "john.nash_UPDATED@abp.io").ConfigureAwait(false);

                _testCounter.GetValue("EntityUpdatedEto<UserEto>").ShouldBe(0);
                await uow.CompleteAsync().ConfigureAwait(false);
            }

            _testCounter.GetValue("EntityUpdatedEto<UserEto>").ShouldBe(1);
        }
    }
}
