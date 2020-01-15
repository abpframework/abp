﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity
{
    public abstract class IdentityUserRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IIdentityUserRepository UserRepository { get; }
        protected ILookupNormalizer LookupNormalizer { get; }

        protected IdentityUserRepository_Tests()
        {
            UserRepository = ServiceProvider.GetRequiredService<IIdentityUserRepository>();
            LookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
        }

        [Fact]
        public async Task FindByNormalizedUserNameAsync()
        {
            (await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash")).ConfigureAwait(false)).ShouldNotBeNull();
            (await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("undefined-user")).ConfigureAwait(false)).ShouldBeNull();
        }

        [Fact]
        public async Task FindByNormalizedEmailAsync()
        {
            (await UserRepository.FindByNormalizedEmailAsync(LookupNormalizer.NormalizeEmail("john.nash@abp.io")).ConfigureAwait(false)).ShouldNotBeNull();
            (await UserRepository.FindByNormalizedEmailAsync(LookupNormalizer.NormalizeEmail("david@abp.io")).ConfigureAwait(false)).ShouldNotBeNull();
            (await UserRepository.FindByNormalizedEmailAsync(LookupNormalizer.NormalizeEmail("undefined-user@abp.io")).ConfigureAwait(false)).ShouldBeNull();
        }

        [Fact]
        public async Task GetRoleNamesAsync()
        {
            var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash")).ConfigureAwait(false);
            var roles = await UserRepository.GetRoleNamesAsync(john.Id).ConfigureAwait(false);
            roles.Count.ShouldBe(2);
            roles.ShouldContain("moderator");
            roles.ShouldContain("supporter");
        }

        [Fact]
        public async Task FindByLoginAsync()
        {
            var user = await UserRepository.FindByLoginAsync("github", "john").ConfigureAwait(false);
            user.ShouldNotBeNull();
            user.UserName.ShouldBe("john.nash");

            user = await UserRepository.FindByLoginAsync("twitter", "johnx").ConfigureAwait(false);
            user.ShouldNotBeNull();
            user.UserName.ShouldBe("john.nash");

            (await UserRepository.FindByLoginAsync("github", "undefinedid").ConfigureAwait(false)).ShouldBeNull();
        }

        [Fact]
        public async Task GetListByClaimAsync()
        {
            var users = await UserRepository.GetListByClaimAsync(new Claim("TestClaimType", "42")).ConfigureAwait(false);
            users.Count.ShouldBe(2);
            users.ShouldContain(u => u.UserName == "administrator");
            users.ShouldContain(u => u.UserName == "john.nash");

            users = await UserRepository.GetListByClaimAsync(new Claim("TestClaimType", "43")).ConfigureAwait(false);
            users.Count.ShouldBe(1);
            users.ShouldContain(u => u.UserName == "neo");

            users = await UserRepository.GetListByClaimAsync(new Claim("TestClaimType", "undefined")).ConfigureAwait(false);
            users.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetListByNormalizedRoleNameAsync()
        {
            var users = await UserRepository.GetListByNormalizedRoleNameAsync(LookupNormalizer.NormalizeName("supporter")).ConfigureAwait(false);
            users.Count.ShouldBe(2);
            users.ShouldContain(u => u.UserName == "john.nash");
            users.ShouldContain(u => u.UserName == "neo");
        }

        [Fact]
        public async Task GetListAsync()
        {
            var users = await UserRepository.GetListAsync("UserName DESC", 5, 0, "n").ConfigureAwait(false);

            users.Count.ShouldBeGreaterThan(1);
            users.Count.ShouldBeLessThanOrEqualTo(5);
            
            //Filter check
            users.ShouldAllBe(u => u.UserName.Contains("n") || u.Email.Contains("n"));

            //Order check
            for (var i = 0; i < users.Count - 1; i++)
            {
                string.Compare(
                    users[i].UserName,
                    users[i + 1].UserName,
                    StringComparison.OrdinalIgnoreCase
                ).ShouldBeGreaterThan(0);
            }

            users = await UserRepository.GetListAsync(null, 999, 0, "undefined-username").ConfigureAwait(false);
            users.Count.ShouldBe(0);
        }

        [Fact]
        public async Task GetRolesAsync()
        {
            var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash")).ConfigureAwait(false);
            var roles = await UserRepository.GetRolesAsync(john.Id).ConfigureAwait(false);
            roles.Count.ShouldBe(2);
            roles.ShouldContain(r => r.Name == "moderator");
            roles.ShouldContain(r => r.Name == "supporter");
        }

        [Fact]
        public async Task GetCountAsync()
        {
            (await UserRepository.GetCountAsync("n").ConfigureAwait(false)).ShouldBeGreaterThan(1);
            (await UserRepository.GetCountAsync("undefined-username").ConfigureAwait(false)).ShouldBe(0);
        }

        [Fact]
        public async Task Should_Eager_Load_User_Collections()
        {
            var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash")).ConfigureAwait(false);

            john.Roles.ShouldNotBeNull();
            john.Roles.Any().ShouldBeTrue();

            john.Logins.ShouldNotBeNull();
            john.Logins.Any().ShouldBeTrue();

            john.Claims.ShouldNotBeNull();
            john.Claims.Any().ShouldBeTrue();

            john.Tokens.ShouldNotBeNull();
            john.Tokens.Any().ShouldBeTrue();
        }
    }
}
