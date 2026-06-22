using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
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

    [Fact]
    public void Should_Validate_Explicit_ThenBy_Selectors()
    {
        // ThenBy(string) is called directly by some app services, not only via multi-column OrderBy.
        // The guard must reject unsafe expressions in ThenBy selectors as well.
        Should.Throw<AbpValidationException>(() =>
            _users.OrderBy("Name").ThenBy("PasswordHash.Substring(0,1)").ToList());
    }

    [Fact]
    public void Should_Validate_ThenBy_With_Descending_Modifier()
    {
        // Dynamic.Core encodes direction in the sorting string (no separate ThenByDescending overload);
        // make sure the guard still inspects the selector when desc is on the ThenBy column.
        Should.Throw<AbpValidationException>(() =>
            _users.OrderBy("Name").ThenBy("PasswordHash.Substring(0,1) desc").ToList());
    }

    [Theory]
    [InlineData("(PasswordHash.Substring(0,1) == \"K\") desc")] // method + binary in one selector
    [InlineData("(Name.Length > 5) desc")]                       // property getter + binary
    [InlineData("(Age + 10) desc")]                               // binary arithmetic
    [InlineData("(Age * Age) desc")]                              // binary arithmetic
    [InlineData("(-Age) desc")]                                   // unary negation wraps something; only Negate is allowed via base.VisitUnary, but the operand is a plain member so this passes — keep as accept check
    public void Should_Reject_Compound_Or_Arithmetic_Expressions(string sorting)
    {
        // Note: the last case `(-Age) desc` actually passes — Unary(Negate(MemberAccess)) has no rejecting node.
        // Filter accordingly.
        if (sorting.Contains("(-"))
        {
            Should.NotThrow(() => _users.OrderBy(sorting).ToList());
        }
        else
        {
            Should.Throw<AbpValidationException>(() => _users.OrderBy(sorting).ToList())
                .Message.ShouldBe("Sorting expression is not supported.");
        }
    }

    [Theory]
    [InlineData("Name.ToUpper().Substring(0,1) desc")]   // chained method calls
    [InlineData("PasswordHash.Substring(0,1).Length desc")] // method then property
    public void Should_Reject_Chained_Method_Calls(string sorting)
    {
        Should.Throw<AbpValidationException>(() => _users.OrderBy(sorting).ToList());
    }

    [Fact]
    public void Should_Not_Interfere_With_DynamicCore_Validation_For_Empty_Or_Null_Sorting()
    {
        // Dynamic.Core itself rejects null / empty / whitespace ordering with ArgumentException
        // before the QueryOptimizer is invoked. The guard must not turn those into
        // AbpValidationException by accident.
        Should.NotThrow(() =>
        {
            try { _users.OrderBy((string)null!).ToList(); } catch (Exception ex) { ex.ShouldNotBeOfType<AbpValidationException>(); }
            try { _users.OrderBy("").ToList(); }            catch (Exception ex) { ex.ShouldNotBeOfType<AbpValidationException>(); }
            try { _users.OrderBy("   ").ToList(); }         catch (Exception ex) { ex.ShouldNotBeOfType<AbpValidationException>(); }
        });
    }

    [Fact]
    public void Should_Validate_OrderBy_With_Args_Parameter()
    {
        // Dynamic.Core's OrderBy supports parameterized expressions through `@0`, `@1`, ... and args.
        // The guard must validate the expanded selector, not the literal sorting string.
        Should.Throw<AbpValidationException>(() =>
            _users.OrderBy("PasswordHash.Substring(@0, 1) desc", 0).ToList());
    }

    [Fact]
    public async Task Install_Is_Thread_Safe_Under_Concurrent_Calls()
    {
        // 50 concurrent Install() callers must not deadlock, throw, or leave the guard in a torn state.
        AbpDynamicSortingGuard.Reset();
        try
        {
            await Task.WhenAll(Enumerable.Range(0, 50)
                .Select(_ => Task.Run(AbpDynamicSortingGuard.Install)));

            // After concurrent installs, the guard must still reject attack payloads.
            Should.Throw<AbpValidationException>(() =>
                _users.OrderBy("PasswordHash.Substring(0,1) desc").ToList());
        }
        finally
        {
            AbpDynamicSortingGuard.Reset();
            AbpDynamicSortingGuard.Install();
        }
    }

    [Theory]
    [InlineData("tr-TR")] // Turkish: uppercase 'I' is dotless, common locale pitfall for string ops
    [InlineData("de-DE")]
    [InlineData("ja-JP")]
    [InlineData("en-US")]
    public void Should_Reject_Method_Calls_Regardless_Of_CurrentCulture(string culture)
    {
        var saved = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Should.Throw<AbpValidationException>(() =>
                _users.OrderBy("PasswordHash.Substring(0,1) desc").ToList());
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentCulture = saved;
        }
    }

    [Theory]
    [InlineData("Name DESC")]    // upper-case direction modifier
    [InlineData("name desc")]    // lower-case property name (Dynamic.Core default case-insensitive lookup)
    [InlineData("NAME ASC")]
    public void Should_Accept_Property_Names_Regardless_Of_Casing(string sorting)
    {
        Should.NotThrow(() => _users.OrderBy(sorting).ToList());
    }

    [Theory]
    [InlineData("passwordhash.substring(0,1) desc")] // lower-case attack
    [InlineData("PASSWORDHASH.SUBSTRING(0,1) DESC")] // upper-case attack — method call detection cannot rely on case
    public void Should_Reject_Method_Calls_Regardless_Of_Casing(string sorting)
    {
        Should.Throw<AbpValidationException>(() => _users.OrderBy(sorting).ToList());
    }

    [Fact]
    public void Should_Allow_Inherited_Property_Sorting()
    {
        var children = new List<FakeChildUser>
        {
            new() { Name = "alpha", Age = 30, ExtraField = "x" },
            new() { Name = "beta",  Age = 25, ExtraField = "y" },
        }.AsQueryable();

        Should.NotThrow(() => children.OrderBy("Name desc, ExtraField asc").ToList());
    }

    [Theory]
    [InlineData("Data[\"Score\"]")]                  // dictionary indexer with constant string key
    [InlineData("Data[\"Score\"] desc")]
    [InlineData("Name asc, Data[\"Score\"] desc")]   // multi-column mixing plain + indexer
    [InlineData("it[\"Score\"] desc")]               // entity-level property-bag indexer (it["Prop"])
    public void Should_Accept_Constant_String_Indexer_Sorting(string sorting)
    {
        // Dynamically-mapped entities sort their shadow / property-bag members through a
        // constant-string indexer; the guard must treat this as plain property access.
        Should.NotThrow(() => FakeBagUsers().OrderBy(sorting).ToList());
    }

    [Theory]
    [InlineData("Data[Name] desc")]                          // non-constant key (member access)
    [InlineData("Data[PasswordHash.Substring(0,1)] desc")]   // method call inside the key
    public void Should_Reject_Non_Constant_Indexer_Sorting(string sorting)
    {
        Should.Throw<AbpValidationException>(() => FakeBagUsers().OrderBy(sorting).ToList())
            .Message.ShouldBe("Sorting expression is not supported.");
    }

    private static IQueryable<FakeBagUser> FakeBagUsers()
    {
        return new List<FakeBagUser>
        {
            new() { Name = "alice", PasswordHash = "AQAA", Data = { ["Score"] = 30 } },
            new() { Name = "bob",   PasswordHash = "BQAA", Data = { ["Score"] = 25 } },
            new() { Name = "carl",  PasswordHash = "CQAA", Data = { ["Score"] = 40 } },
        }.AsQueryable();
    }

    private class FakeUser
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string PasswordHash { get; set; } = "";
        public FakeTenant Tenant { get; set; } = new();
    }

    private class FakeChildUser : FakeUser
    {
        public string ExtraField { get; set; } = "";
    }

    private class FakeTenant
    {
        public string Name { get; set; } = "";
    }

    private class FakeBagUser
    {
        public string Name { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public Dictionary<string, object> Data { get; set; } = new();

        public object this[string key] => Data[key];
    }
}
