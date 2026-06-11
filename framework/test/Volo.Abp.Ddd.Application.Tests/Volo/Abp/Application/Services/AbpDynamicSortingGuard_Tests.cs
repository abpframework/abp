using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Shouldly;
using Volo.Abp.Validation;
using Xunit;

namespace Volo.Abp.Application.Services;

public class AbpDynamicSortingGuard_Tests : AbpDddApplicationTestBase
{
    private readonly IQueryable<FakeUser> _users;

    public AbpDynamicSortingGuard_Tests()
    {
        _users = new List<FakeUser>
        {
            new() { Name = "alice", Age = 30, PasswordHash = "AQAAhash_alice", Tenant = new FakeTenant { Name = "acme" } },
            new() { Name = "bob",   Age = 25, PasswordHash = "BQAAhash_bob",   Tenant = new FakeTenant { Name = "beta" } },
            new() { Name = "carl",  Age = 40, PasswordHash = "CQAAhash_carl",  Tenant = new FakeTenant { Name = "corp" } },
        }.AsQueryable();
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("Name desc")]
    [InlineData("Name asc, PasswordHash desc")]
    [InlineData("Age desc")] // value type → EF/Dynamic.Core wraps selector in Convert(MemberAccess, object)
    [InlineData("Tenant.Name")] // chained MemberAccess
    [InlineData("Tenant.Name desc, Age asc")] // mixed chained + value-type, multi-column
    [InlineData("Name.Length desc")] // Length is a property getter, not a method call
    public void Should_Accept_Plain_Property_Sorting(string sorting)
    {
        Should.NotThrow(() => _users.OrderBy(sorting).ToList());
    }

    [Theory]
    [InlineData("PasswordHash.Substring(0,1) desc")]
    [InlineData("PasswordHash.StartsWith(\"A\") desc")]
    [InlineData("PasswordHash.Contains(\"hash\") desc")]
    [InlineData("Name asc, PasswordHash.Substring(0,1) desc")] // multi-column with attack in 2nd
    public void Should_Reject_Method_Call_On_Property(string sorting)
    {
        Should.Throw<AbpValidationException>(() => _users.OrderBy(sorting).ToList())
            .Message.ShouldBe("Sorting expression is not supported.");
    }

    [Theory]
    [InlineData("(PasswordHash == \"AQAA\") desc")]
    [InlineData("(PasswordHash > \"M\") desc")]
    [InlineData("(PasswordHash != \"AQAA\") asc")]
    public void Should_Reject_Binary_Expressions(string sorting)
    {
        Should.Throw<AbpValidationException>(() => _users.OrderBy(sorting).ToList())
            .Message.ShouldBe("Sorting expression is not supported.");
    }

    [Fact]
    public void Should_Not_Affect_Where_Expressions()
    {
        // The guard only inspects Queryable.OrderBy/ThenBy nodes. Where with the same
        // sub-expression is left alone (Where is a separate vulnerability class).
        Should.NotThrow(() => _users.Where("PasswordHash.StartsWith(\"A\")").ToList());
    }

    [Fact]
    public void Install_Chains_Existing_QueryOptimizer()
    {
        // Reset guard state so Install() actually re-installs and exercises the
        // `previous != null ? previous(expression) : expression` branch.
        AbpDynamicSortingGuard.Reset();
        try
        {
            var preExistingFired = false;
            ExtensibilityPoint.QueryOptimizer = e =>
            {
                preExistingFired = true;
                return e;
            };

            AbpDynamicSortingGuard.Install();

            _users.OrderBy("Name").ToList();
            preExistingFired.ShouldBeTrue();
        }
        finally
        {
            // Leave the AppDomain with a single-layer guard. Reset() clears whatever
            // we wrapped in this test; Install() then puts a fresh guard on top of
            // an empty QueryOptimizer — never double-wraps an existing guard.
            AbpDynamicSortingGuard.Reset();
            AbpDynamicSortingGuard.Install();
        }
    }

    [Fact]
    public void Install_Reinstalls_When_QueryOptimizer_Was_Replaced()
    {
        // Simulate someone (e.g. a test teardown, another module) overwriting our
        // optimizer. The next Install() must detect the mismatch and wrap again.
        try
        {
            ExtensibilityPoint.QueryOptimizer = e => e; // not our wrapper

            AbpDynamicSortingGuard.Install();

            // Guard must be active again — attack payload still gets rejected.
            Should.Throw<AbpValidationException>(() =>
                _users.OrderBy("PasswordHash.Substring(0,1) desc").ToList());
        }
        finally
        {
            AbpDynamicSortingGuard.Reset();
            AbpDynamicSortingGuard.Install();
        }
    }

    private class FakeUser
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string PasswordHash { get; set; } = "";
        public FakeTenant Tenant { get; set; } = new();
    }

    private class FakeTenant
    {
        public string Name { get; set; } = "";
    }
}
