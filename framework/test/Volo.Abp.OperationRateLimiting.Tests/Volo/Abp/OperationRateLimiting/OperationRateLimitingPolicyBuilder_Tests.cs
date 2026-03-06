using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingPolicyBuilder_Tests
{
    [Fact]
    public void Should_Build_Simple_Policy()
    {
        var options = new AbpOperationRateLimitingOptions();
        options.AddPolicy("TestPolicy", policy =>
        {
            policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                  .PartitionByParameter();
        });

        var policy = options.Policies["TestPolicy"];

        policy.Name.ShouldBe("TestPolicy");
        policy.Rules.Count.ShouldBe(1);
        policy.Rules[0].Duration.ShouldBe(TimeSpan.FromHours(1));
        policy.Rules[0].MaxCount.ShouldBe(5);
        policy.Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.Parameter);
        policy.ErrorCode.ShouldBeNull();
    }

    [Fact]
    public void Should_Build_Composite_Policy()
    {
        var options = new AbpOperationRateLimitingOptions();
        options.AddPolicy("CompositePolicy", policy =>
        {
            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                .PartitionByParameter());

            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromDays(1), maxCount: 10)
                .PartitionByCurrentUser());
        });

        var policy = options.Policies["CompositePolicy"];

        policy.Name.ShouldBe("CompositePolicy");
        policy.Rules.Count.ShouldBe(2);
        policy.Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.Parameter);
        policy.Rules[0].MaxCount.ShouldBe(3);
        policy.Rules[1].PartitionType.ShouldBe(OperationRateLimitingPartitionType.CurrentUser);
        policy.Rules[1].MaxCount.ShouldBe(10);
    }

    [Fact]
    public void Should_Set_ErrorCode()
    {
        var options = new AbpOperationRateLimitingOptions();
        options.AddPolicy("ErrorPolicy", policy =>
        {
            policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 2)
                  .PartitionByParameter()
                  .WithErrorCode("App:Custom:Error");
        });

        var policy = options.Policies["ErrorPolicy"];
        policy.ErrorCode.ShouldBe("App:Custom:Error");
    }

    [Fact]
    public void Should_Build_Custom_Partition()
    {
        var options = new AbpOperationRateLimitingOptions();
        options.AddPolicy("CustomPolicy", policy =>
        {
            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromMinutes(30), maxCount: 5)
                .PartitionBy(ctx => Task.FromResult($"custom:{ctx.Parameter}")));
        });

        var policy = options.Policies["CustomPolicy"];

        policy.Rules.Count.ShouldBe(1);
        policy.Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.Custom);
        policy.Rules[0].CustomPartitionKeyResolver.ShouldNotBeNull();
    }

    [Fact]
    public void Should_Support_All_Partition_Types()
    {
        var options = new AbpOperationRateLimitingOptions();

        options.AddPolicy("P1", p => p.WithFixedWindow(TimeSpan.FromHours(1), 1).PartitionByParameter());
        options.AddPolicy("P2", p => p.WithFixedWindow(TimeSpan.FromHours(1), 1).PartitionByCurrentUser());
        options.AddPolicy("P3", p => p.WithFixedWindow(TimeSpan.FromHours(1), 1).PartitionByCurrentTenant());
        options.AddPolicy("P4", p => p.WithFixedWindow(TimeSpan.FromHours(1), 1).PartitionByClientIp());
        options.AddPolicy("P5", p => p.WithFixedWindow(TimeSpan.FromHours(1), 1).PartitionByEmail());
        options.AddPolicy("P6", p => p.WithFixedWindow(TimeSpan.FromHours(1), 1).PartitionByPhoneNumber());

        options.Policies["P1"].Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.Parameter);
        options.Policies["P2"].Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.CurrentUser);
        options.Policies["P3"].Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.CurrentTenant);
        options.Policies["P4"].Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.ClientIp);
        options.Policies["P5"].Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.Email);
        options.Policies["P6"].Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.PhoneNumber);
    }

    [Fact]
    public void Should_Throw_When_Policy_Has_No_Rules()
    {
        var options = new AbpOperationRateLimitingOptions();

        var exception = Assert.Throws<AbpException>(() =>
        {
            options.AddPolicy("EmptyPolicy", policy =>
            {
                // Intentionally not adding any rules
            });
        });

        exception.Message.ShouldContain("no rules");
    }

    [Fact]
    public void Should_Throw_When_WithFixedWindow_Without_PartitionBy()
    {
        var options = new AbpOperationRateLimitingOptions();

        var exception = Assert.Throws<AbpException>(() =>
        {
            options.AddPolicy("IncompletePolicy", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5);
                // Missing PartitionBy*() call - rule never committed
            });
        });

        exception.Message.ShouldContain("no rules");
    }

    [Fact]
    public void Should_Throw_When_AddRule_Without_WithFixedWindow()
    {
        var options = new AbpOperationRateLimitingOptions();

        var exception = Assert.Throws<AbpException>(() =>
        {
            options.AddPolicy("NoWindowPolicy", policy =>
            {
                policy.AddRule(rule =>
                {
                    // Missing WithFixedWindow call - duration is zero
                });
            });
        });

        exception.Message.ShouldContain("positive duration");
    }

    [Fact]
    public void Should_Allow_MaxCount_Zero_For_Ban_Policy()
    {
        var options = new AbpOperationRateLimitingOptions();

        // maxCount=0 is a valid "ban" policy - always deny
        options.AddPolicy("BanPolicy", policy =>
        {
            policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 0)
                  .PartitionByParameter();
        });

        var policy = options.Policies["BanPolicy"];
        policy.Rules[0].MaxCount.ShouldBe(0);
    }

    [Fact]
    public void Should_Throw_When_AddRule_Without_PartitionBy()
    {
        var options = new AbpOperationRateLimitingOptions();

        var exception = Assert.Throws<AbpException>(() =>
        {
            options.AddPolicy("NoPartitionPolicy", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5));
                // Missing PartitionBy*() call
            });
        });

        exception.Message.ShouldContain("partition type");
    }

    [Fact]
    public void Should_Throw_When_MaxCount_Is_Negative()
    {
        var options = new AbpOperationRateLimitingOptions();

        var exception = Assert.Throws<AbpException>(() =>
        {
            options.AddPolicy("NegativePolicy", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: -1)
                      .PartitionByParameter();
            });
        });

        exception.Message.ShouldContain("maxCount >= 0");
    }

    [Fact]
    public void Should_Allow_Same_Rule_With_Different_MultiTenancy()
    {
        var options = new AbpOperationRateLimitingOptions();

        // Same Duration/MaxCount/PartitionType but different IsMultiTenant should be allowed
        options.AddPolicy("MultiTenancyPolicy", policy =>
        {
            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                .PartitionByParameter());

            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                .WithMultiTenancy()
                .PartitionByParameter());
        });

        var policy = options.Policies["MultiTenancyPolicy"];
        policy.Rules.Count.ShouldBe(2);
        policy.Rules[0].IsMultiTenant.ShouldBeFalse();
        policy.Rules[1].IsMultiTenant.ShouldBeTrue();
    }

    [Fact]
    public void Should_Allow_Multiple_Custom_Partition_Rules()
    {
        var options = new AbpOperationRateLimitingOptions();

        // Multiple custom partition rules with same Duration/MaxCount should be allowed
        // because they may use different key resolvers
        options.AddPolicy("MultiCustomPolicy", policy =>
        {
            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                .PartitionBy(ctx => Task.FromResult($"by-ip:{ctx.Parameter}")));

            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                .PartitionBy(ctx => Task.FromResult($"by-device:{ctx.Parameter}")));
        });

        var policy = options.Policies["MultiCustomPolicy"];
        policy.Rules.Count.ShouldBe(2);
        policy.Rules[0].PartitionType.ShouldBe(OperationRateLimitingPartitionType.Custom);
        policy.Rules[1].PartitionType.ShouldBe(OperationRateLimitingPartitionType.Custom);
    }
}
