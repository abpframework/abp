using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Autofac;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace Volo.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpOperationRateLimitingModule),
    typeof(AbpExceptionHandlingModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule)
)]
public class AbpOperationRateLimitingTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var mockWebClientInfoProvider = Substitute.For<IWebClientInfoProvider>();
        mockWebClientInfoProvider.ClientIpAddress.Returns("127.0.0.1");
        context.Services.AddSingleton<IWebClientInfoProvider>(mockWebClientInfoProvider);

        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            options.AddPolicy("TestSimple", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                      .PartitionByParameter();
            });

            options.AddPolicy("TestUserBased", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromMinutes(30), maxCount: 5)
                      .PartitionByCurrentUser();
            });

            options.AddPolicy("TestComposite", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromDays(1), maxCount: 10)
                    .PartitionByCurrentUser());
            });

            options.AddPolicy("TestCustomErrorCode", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 2)
                      .PartitionByParameter()
                      .WithErrorCode("Test:CustomError");
            });

            options.AddPolicy("TestTenantBased", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                      .PartitionByCurrentTenant();
            });

            options.AddPolicy("TestClientIp", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromMinutes(15), maxCount: 10)
                      .PartitionByClientIp();
            });

            options.AddPolicy("TestEmailBased", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                      .PartitionByEmail();
            });

            options.AddPolicy("TestPhoneNumberBased", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                      .PartitionByPhoneNumber();
            });

            // Composite where Rule2 triggers before Rule1 (to test no-wasted-increment)
            options.AddPolicy("TestCompositeRule2First", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 2)
                    .PartitionByCurrentUser());
            });

            // Composite: ByParameter + ByClientIp (different partition types, no auth)
            options.AddPolicy("TestCompositeParamIp", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                    .PartitionByClientIp());
            });

            // Composite: Triple - ByParameter + ByCurrentUser + ByClientIp
            options.AddPolicy("TestCompositeTriple", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 4)
                    .PartitionByCurrentUser());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                    .PartitionByClientIp());
            });

            // Fix #6: policy where both rules block simultaneously with different RetryAfter durations.
            // Used to verify that Phase 1 checks ALL rules and reports the maximum RetryAfter.
            // Rule0: 5-minute window → RetryAfter ~5 min when full
            // Rule1: 2-hour window  → RetryAfter ~2 hr  when full
            options.AddPolicy("TestCompositeMaxRetryAfter", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(5), maxCount: 1)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(2), maxCount: 1)
                    .PartitionByParameter());
            });

            // Fix #6: policy where only Rule0 blocks but Rule1 is still within limit.
            // Used to verify that RuleResults contains all rules, not just the blocking one.
            options.AddPolicy("TestCompositePartialBlock", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 1)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 100)
                    .PartitionByParameter());
            });

            // Ban policy: maxCount=0 should always deny
            options.AddPolicy("TestBanPolicy", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 0)
                      .PartitionByParameter();
            });

            // Custom resolver: combines Parameter + a static prefix to simulate multi-value key
            options.AddPolicy("TestCustomResolver", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 2)
                      .PartitionBy(ctx => Task.FromResult($"action:{ctx.Parameter}"));
            });

            // Multi-tenant: ByParameter with tenant isolation - same param, different tenants = different counters
            options.AddPolicy("TestMultiTenantByParameter", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 2)
                      .WithMultiTenancy()
                      .PartitionByParameter();
            });

            // Multi-tenant: ByClientIp (global) - same IP, different tenants = same counter
            options.AddPolicy("TestMultiTenantByClientIp", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 2)
                      .PartitionByClientIp();
            });
        });
    }
}
