using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingChecker_Tests : OperationRateLimitingTestBase
{
    private readonly IOperationRateLimitingChecker _checker;

    public OperationRateLimitingChecker_Tests()
    {
        _checker = GetRequiredService<IOperationRateLimitingChecker>();
    }

    [Fact]
    public async Task Should_Allow_Within_Limit()
    {
        var context = new OperationRateLimitingContext { Parameter = "test@example.com" };

        // Should not throw for 3 requests (max is 3)
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
    }

    [Fact]
    public async Task Should_Reject_When_Exceeded()
    {
        var param = $"exceed-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestSimple", context);
        });

        exception.PolicyName.ShouldBe("TestSimple");
        exception.Result.IsAllowed.ShouldBeFalse();
        exception.HttpStatusCode.ShouldBe(429);
        exception.Code.ShouldBe(AbpOperationRateLimitingErrorCodes.ExceedLimit);
    }

    [Fact]
    public async Task Should_Return_Correct_RemainingCount()
    {
        var param = $"remaining-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        var status = await _checker.GetStatusAsync("TestSimple", context);
        status.IsAllowed.ShouldBeTrue();
        status.RemainingCount.ShouldBe(3);
        status.CurrentCount.ShouldBe(0);

        // Increment once
        await _checker.CheckAsync("TestSimple", context);

        status = await _checker.GetStatusAsync("TestSimple", context);
        status.IsAllowed.ShouldBeTrue();
        status.RemainingCount.ShouldBe(2);
        status.CurrentCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Return_Correct_RetryAfter()
    {
        var param = $"retry-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestSimple", context);
        });

        exception.Result.RetryAfter.ShouldNotBeNull();
        exception.Result.RetryAfter!.Value.TotalSeconds.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Handle_Composite_Policy_All_Pass()
    {
        var userId = Guid.NewGuid();

        using (var scope = ServiceProvider.CreateScope())
        {
            var principalAccessor = scope.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            var claimsPrincipal = CreateClaimsPrincipal(userId);

            using (principalAccessor.Change(claimsPrincipal))
            {
                var checker = scope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();
                var context = new OperationRateLimitingContext { Parameter = $"composite-{Guid.NewGuid()}" };

                // Should pass: both rules within limits
                await checker.CheckAsync("TestComposite", context);
                await checker.CheckAsync("TestComposite", context);
                await checker.CheckAsync("TestComposite", context);
            }
        }
    }

    [Fact]
    public async Task Should_Reject_Composite_Policy_When_Any_Rule_Exceeds()
    {
        var userId = Guid.NewGuid();

        using (var scope = ServiceProvider.CreateScope())
        {
            var principalAccessor = scope.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            var claimsPrincipal = CreateClaimsPrincipal(userId);

            using (principalAccessor.Change(claimsPrincipal))
            {
                var checker = scope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();
                var param = $"composite-reject-{Guid.NewGuid()}";
                var context = new OperationRateLimitingContext { Parameter = param };

                await checker.CheckAsync("TestComposite", context);
                await checker.CheckAsync("TestComposite", context);
                await checker.CheckAsync("TestComposite", context);

                // 4th request: Rule1 (max 3 per hour by parameter) should fail
                var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
                {
                    await checker.CheckAsync("TestComposite", context);
                });

                exception.PolicyName.ShouldBe("TestComposite");
            }
        }
    }

    [Fact]
    public async Task Should_Reset_Counter()
    {
        var param = $"reset-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        // Should be at limit
        await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestSimple", context);
        });

        // Reset
        await _checker.ResetAsync("TestSimple", context);

        // Should be allowed again
        await _checker.CheckAsync("TestSimple", context);
    }

    [Fact]
    public async Task Should_Use_Custom_ErrorCode()
    {
        var param = $"custom-error-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        await _checker.CheckAsync("TestCustomErrorCode", context);
        await _checker.CheckAsync("TestCustomErrorCode", context);

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestCustomErrorCode", context);
        });

        exception.Code.ShouldBe("Test:CustomError");
    }

    [Fact]
    public async Task Should_Throw_For_Unknown_Policy()
    {
        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _checker.CheckAsync("NonExistentPolicy");
        });
    }

    [Fact]
    public async Task Should_Skip_When_Disabled()
    {
        var options = GetRequiredService<Microsoft.Extensions.Options.IOptions<AbpOperationRateLimitingOptions>>();
        var originalValue = options.Value.IsEnabled;

        try
        {
            options.Value.IsEnabled = false;

            var param = $"disabled-{Guid.NewGuid()}";
            var context = new OperationRateLimitingContext { Parameter = param };

            // Should pass unlimited times
            for (var i = 0; i < 100; i++)
            {
                await _checker.CheckAsync("TestSimple", context);
            }
        }
        finally
        {
            options.Value.IsEnabled = originalValue;
        }
    }

    [Fact]
    public async Task Should_Work_With_IsAllowedAsync()
    {
        var param = $"is-allowed-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        // IsAllowedAsync does not consume quota
        (await _checker.IsAllowedAsync("TestSimple", context)).ShouldBeTrue();
        (await _checker.IsAllowedAsync("TestSimple", context)).ShouldBeTrue();

        // Status should still show 0 consumed
        var status = await _checker.GetStatusAsync("TestSimple", context);
        status.CurrentCount.ShouldBe(0);
        status.RemainingCount.ShouldBe(3);

        // Now consume all
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        (await _checker.IsAllowedAsync("TestSimple", context)).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Partition_By_Different_Parameters()
    {
        var param1 = $"param1-{Guid.NewGuid()}";
        var param2 = $"param2-{Guid.NewGuid()}";

        var context1 = new OperationRateLimitingContext { Parameter = param1 };
        var context2 = new OperationRateLimitingContext { Parameter = param2 };

        // Consume all for param1
        await _checker.CheckAsync("TestSimple", context1);
        await _checker.CheckAsync("TestSimple", context1);
        await _checker.CheckAsync("TestSimple", context1);

        // param2 should still be allowed
        await _checker.CheckAsync("TestSimple", context2);
        (await _checker.IsAllowedAsync("TestSimple", context2)).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Support_ExtraProperties_In_Exception_Data()
    {
        var param = $"extra-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext
        {
            Parameter = param,
            ExtraProperties =
            {
                ["Email"] = "test@example.com",
                ["UserId"] = "user-123"
            }
        };

        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);
        await _checker.CheckAsync("TestSimple", context);

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestSimple", context);
        });

        exception.Data["Email"].ShouldBe("test@example.com");
        exception.Data["UserId"].ShouldBe("user-123");
        exception.Data["PolicyName"].ShouldBe("TestSimple");
        exception.Data["MaxCount"].ShouldBe(3);
    }

    [Fact]
    public async Task Should_Partition_By_Email_Via_Parameter()
    {
        var email = $"email-param-{Guid.NewGuid()}@example.com";
        var context = new OperationRateLimitingContext { Parameter = email };

        await _checker.CheckAsync("TestEmailBased", context);
        await _checker.CheckAsync("TestEmailBased", context);
        await _checker.CheckAsync("TestEmailBased", context);

        await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestEmailBased", context);
        });
    }

    [Fact]
    public async Task Should_Partition_By_Email_Via_CurrentUser_Fallback()
    {
        var userId = Guid.NewGuid();

        using (var scope = ServiceProvider.CreateScope())
        {
            var principalAccessor = scope.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            var claimsPrincipal = CreateClaimsPrincipal(userId);

            using (principalAccessor.Change(claimsPrincipal))
            {
                var checker = scope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();

                // No Parameter set, should fall back to ICurrentUser.Email
                var context = new OperationRateLimitingContext();

                await checker.CheckAsync("TestEmailBased", context);
                await checker.CheckAsync("TestEmailBased", context);
                await checker.CheckAsync("TestEmailBased", context);

                await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
                {
                    await checker.CheckAsync("TestEmailBased", context);
                });
            }
        }
    }

    [Fact]
    public async Task Should_Partition_By_PhoneNumber_Via_Parameter()
    {
        var phone = $"phone-param-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = phone };

        await _checker.CheckAsync("TestPhoneNumberBased", context);
        await _checker.CheckAsync("TestPhoneNumberBased", context);
        await _checker.CheckAsync("TestPhoneNumberBased", context);

        await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestPhoneNumberBased", context);
        });
    }

    [Fact]
    public async Task Should_Partition_By_PhoneNumber_Via_CurrentUser_Fallback()
    {
        var userId = Guid.NewGuid();

        using (var scope = ServiceProvider.CreateScope())
        {
            var principalAccessor = scope.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            var claimsPrincipal = CreateClaimsPrincipal(userId);

            using (principalAccessor.Change(claimsPrincipal))
            {
                var checker = scope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();

                // No Parameter set, should fall back to ICurrentUser.PhoneNumber
                var context = new OperationRateLimitingContext();

                await checker.CheckAsync("TestPhoneNumberBased", context);
                await checker.CheckAsync("TestPhoneNumberBased", context);
                await checker.CheckAsync("TestPhoneNumberBased", context);

                await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
                {
                    await checker.CheckAsync("TestPhoneNumberBased", context);
                });
            }
        }
    }

    [Fact]
    public async Task Should_Throw_When_Email_Not_Available()
    {
        // No Parameter and no authenticated user
        var context = new OperationRateLimitingContext();

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _checker.CheckAsync("TestEmailBased", context);
        });
    }

    [Fact]
    public async Task Should_Not_Waste_Rule1_Count_When_Rule2_Blocks()
    {
        // TestCompositeRule2First: Rule1 (Parameter, 5/hour), Rule2 (CurrentUser, 2/hour)
        // Rule2 triggers at 2. Rule1 should NOT be incremented for blocked requests.
        var userId = Guid.NewGuid();

        using (var scope = ServiceProvider.CreateScope())
        {
            var principalAccessor = scope.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            var claimsPrincipal = CreateClaimsPrincipal(userId);

            using (principalAccessor.Change(claimsPrincipal))
            {
                var checker = scope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();
                var param = $"no-waste-{Guid.NewGuid()}";
                var context = new OperationRateLimitingContext { Parameter = param };

                // 2 successful requests (Rule1: 2/5, Rule2: 2/2)
                await checker.CheckAsync("TestCompositeRule2First", context);
                await checker.CheckAsync("TestCompositeRule2First", context);

                // 3rd request: Rule2 blocks (2/2 at max)
                await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
                {
                    await checker.CheckAsync("TestCompositeRule2First", context);
                });

                // Verify Rule1 was NOT incremented for the blocked request
                // Rule1 should still be at 2/5, not 3/5
                var status = await checker.GetStatusAsync("TestCompositeRule2First", context);
                // GetStatusAsync returns the most restrictive rule (Rule2 at 2/2)
                // But we can verify Rule1 by checking RuleResults
                status.RuleResults.ShouldNotBeNull();
                status.RuleResults!.Count.ShouldBe(2);

                // Rule1 (index 0): should be 2/5, remaining 3
                status.RuleResults[0].RemainingCount.ShouldBe(3);
                status.RuleResults[0].MaxCount.ShouldBe(5);

                // Rule2 (index 1): should be 2/2, remaining 0
                status.RuleResults[1].RemainingCount.ShouldBe(0);
                status.RuleResults[1].MaxCount.ShouldBe(2);
            }
        }
    }

    [Fact]
    public async Task Should_Composite_ParamIp_Ip_Triggers_First()
    {
        // TestCompositeParamIp: Rule1 (Parameter, 5/hour), Rule2 (ClientIp, 3/hour)
        // IP limit (3) is lower, should trigger first
        var param = $"param-ip-{Guid.NewGuid()}";
        var context = new OperationRateLimitingContext { Parameter = param };

        // 3 successful requests
        await _checker.CheckAsync("TestCompositeParamIp", context);
        await _checker.CheckAsync("TestCompositeParamIp", context);
        await _checker.CheckAsync("TestCompositeParamIp", context);

        // 4th: IP rule blocks (3/3)
        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestCompositeParamIp", context);
        });

        exception.PolicyName.ShouldBe("TestCompositeParamIp");

        // Verify counts: Rule1 should be 3/5, Rule2 should be 3/3
        var status = await _checker.GetStatusAsync("TestCompositeParamIp", context);
        status.RuleResults.ShouldNotBeNull();
        status.RuleResults!.Count.ShouldBe(2);

        status.RuleResults[0].RemainingCount.ShouldBe(2); // Parameter: 3/5, remaining 2
        status.RuleResults[0].MaxCount.ShouldBe(5);
        status.RuleResults[1].RemainingCount.ShouldBe(0); // IP: 3/3, remaining 0
        status.RuleResults[1].MaxCount.ShouldBe(3);
    }

    [Fact]
    public async Task Should_Composite_ParamIp_Different_Params_Share_Ip()
    {
        // Different parameters should have independent Rule1 counters
        // but share the same Rule2 (IP) counter
        var param1 = $"share-ip-1-{Guid.NewGuid()}";
        var param2 = $"share-ip-2-{Guid.NewGuid()}";
        var context1 = new OperationRateLimitingContext { Parameter = param1 };
        var context2 = new OperationRateLimitingContext { Parameter = param2 };

        // 2 requests with param1
        await _checker.CheckAsync("TestCompositeParamIp", context1);
        await _checker.CheckAsync("TestCompositeParamIp", context1);

        // 1 request with param2 (IP counter is now at 3/3)
        await _checker.CheckAsync("TestCompositeParamIp", context2);

        // 4th request with param2: IP rule blocks (3/3 from combined)
        await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestCompositeParamIp", context2);
        });

        // param1 Rule1 should be at 2/5
        var status1 = await _checker.GetStatusAsync("TestCompositeParamIp", context1);
        status1.RuleResults![0].RemainingCount.ShouldBe(3); // Parameter: 2/5
        status1.RuleResults[0].MaxCount.ShouldBe(5);

        // param2 Rule1 should be at 1/5
        var status2 = await _checker.GetStatusAsync("TestCompositeParamIp", context2);
        status2.RuleResults![0].RemainingCount.ShouldBe(4); // Parameter: 1/5
        status2.RuleResults[0].MaxCount.ShouldBe(5);
    }

    [Fact]
    public async Task Should_Composite_Triple_Lowest_Limit_Triggers_First()
    {
        // TestCompositeTriple: Rule1 (Parameter, 5/hour), Rule2 (User, 4/hour), Rule3 (IP, 3/hour)
        // IP limit (3) is lowest, should trigger first
        var userId = Guid.NewGuid();

        using (var scope = ServiceProvider.CreateScope())
        {
            var principalAccessor = scope.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            var claimsPrincipal = CreateClaimsPrincipal(userId);

            using (principalAccessor.Change(claimsPrincipal))
            {
                var checker = scope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();
                var param = $"triple-{Guid.NewGuid()}";
                var context = new OperationRateLimitingContext { Parameter = param };

                // 3 successful requests
                await checker.CheckAsync("TestCompositeTriple", context);
                await checker.CheckAsync("TestCompositeTriple", context);
                await checker.CheckAsync("TestCompositeTriple", context);

                // 4th: IP rule blocks (3/3)
                await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
                {
                    await checker.CheckAsync("TestCompositeTriple", context);
                });

                // Verify all three rules
                var status = await checker.GetStatusAsync("TestCompositeTriple", context);
                status.RuleResults.ShouldNotBeNull();
                status.RuleResults!.Count.ShouldBe(3);

                status.RuleResults[0].RemainingCount.ShouldBe(2); // Parameter: 3/5
                status.RuleResults[0].MaxCount.ShouldBe(5);
                status.RuleResults[1].RemainingCount.ShouldBe(1); // User: 3/4
                status.RuleResults[1].MaxCount.ShouldBe(4);
                status.RuleResults[2].RemainingCount.ShouldBe(0); // IP: 3/3
                status.RuleResults[2].MaxCount.ShouldBe(3);
            }
        }
    }

    [Fact]
    public async Task Should_Composite_Triple_No_Wasted_Increment_On_Block()
    {
        // When IP (Rule3) blocks, Rule1 and Rule2 should NOT be incremented
        var userId = Guid.NewGuid();

        using (var scope = ServiceProvider.CreateScope())
        {
            var principalAccessor = scope.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            var claimsPrincipal = CreateClaimsPrincipal(userId);

            using (principalAccessor.Change(claimsPrincipal))
            {
                var checker = scope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();
                var param = $"triple-nowaste-{Guid.NewGuid()}";
                var context = new OperationRateLimitingContext { Parameter = param };

                // 3 successful requests (all rules increment to 3)
                await checker.CheckAsync("TestCompositeTriple", context);
                await checker.CheckAsync("TestCompositeTriple", context);
                await checker.CheckAsync("TestCompositeTriple", context);

                // Attempt 3 more blocked requests
                for (var i = 0; i < 3; i++)
                {
                    await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
                    {
                        await checker.CheckAsync("TestCompositeTriple", context);
                    });
                }

                // Verify Rule1 and Rule2 were NOT incremented beyond 3
                var status = await checker.GetStatusAsync("TestCompositeTriple", context);
                status.RuleResults![0].RemainingCount.ShouldBe(2); // Parameter: still 3/5
                status.RuleResults[1].RemainingCount.ShouldBe(1);  // User: still 3/4
                status.RuleResults[2].RemainingCount.ShouldBe(0);  // IP: still 3/3
            }
        }
    }

    [Fact]
    public async Task Should_Composite_Reset_All_Rules()
    {
        // Verify reset clears all rules in a composite policy
        var userId = Guid.NewGuid();

        using (var scope = ServiceProvider.CreateScope())
        {
            var principalAccessor = scope.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            var claimsPrincipal = CreateClaimsPrincipal(userId);

            using (principalAccessor.Change(claimsPrincipal))
            {
                var checker = scope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();
                var param = $"triple-reset-{Guid.NewGuid()}";
                var context = new OperationRateLimitingContext { Parameter = param };

                // Exhaust IP limit
                await checker.CheckAsync("TestCompositeTriple", context);
                await checker.CheckAsync("TestCompositeTriple", context);
                await checker.CheckAsync("TestCompositeTriple", context);

                await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
                {
                    await checker.CheckAsync("TestCompositeTriple", context);
                });

                // Reset
                await checker.ResetAsync("TestCompositeTriple", context);

                // All rules should be cleared
                var status = await checker.GetStatusAsync("TestCompositeTriple", context);
                status.IsAllowed.ShouldBeTrue();
                status.RuleResults![0].RemainingCount.ShouldBe(5); // Parameter: 0/5
                status.RuleResults[1].RemainingCount.ShouldBe(4);  // User: 0/4
                status.RuleResults[2].RemainingCount.ShouldBe(3);  // IP: 0/3

                // Should be able to use again
                await checker.CheckAsync("TestCompositeTriple", context);
            }
        }
    }

    [Fact]
    public async Task Should_Throw_When_PhoneNumber_Not_Available()
    {
        // No Parameter and no authenticated user
        var context = new OperationRateLimitingContext();

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _checker.CheckAsync("TestPhoneNumberBased", context);
        });
    }

    [Fact]
    public async Task Should_Deny_First_Request_When_MaxCount_Is_Zero()
    {
        var context = new OperationRateLimitingContext { Parameter = $"ban-{Guid.NewGuid()}" };

        var exception = await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestBanPolicy", context);
        });

        exception.Result.IsAllowed.ShouldBeFalse();
        exception.Result.MaxCount.ShouldBe(0);
        exception.Result.RetryAfter.ShouldBeNull();
        exception.HttpStatusCode.ShouldBe(429);
    }

    [Fact]
    public async Task Should_IsAllowed_Return_False_When_MaxCount_Is_Zero()
    {
        var context = new OperationRateLimitingContext { Parameter = $"ban-allowed-{Guid.NewGuid()}" };

        var allowed = await _checker.IsAllowedAsync("TestBanPolicy", context);
        allowed.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_GetStatus_Show_Not_Allowed_When_MaxCount_Is_Zero()
    {
        var context = new OperationRateLimitingContext { Parameter = $"ban-status-{Guid.NewGuid()}" };

        var status = await _checker.GetStatusAsync("TestBanPolicy", context);
        status.IsAllowed.ShouldBeFalse();
        status.MaxCount.ShouldBe(0);
        status.RemainingCount.ShouldBe(0);
        status.RetryAfter.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Partition_By_Custom_Resolver()
    {
        // TestCustomResolver uses PartitionBy(ctx => $"action:{ctx.Parameter}")
        // Two different parameters => independent counters
        var param1 = $"op1-{Guid.NewGuid()}";
        var param2 = $"op2-{Guid.NewGuid()}";

        var ctx1 = new OperationRateLimitingContext { Parameter = param1 };
        var ctx2 = new OperationRateLimitingContext { Parameter = param2 };

        // Exhaust param1's quota (max=2)
        await _checker.CheckAsync("TestCustomResolver", ctx1);
        await _checker.CheckAsync("TestCustomResolver", ctx1);

        await Assert.ThrowsAsync<AbpOperationRateLimitingException>(async () =>
        {
            await _checker.CheckAsync("TestCustomResolver", ctx1);
        });

        // param2 should still be allowed
        await _checker.CheckAsync("TestCustomResolver", ctx2);
        (await _checker.IsAllowedAsync("TestCustomResolver", ctx2)).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Throw_When_Custom_Resolver_Returns_Null()
    {
        var context = new OperationRateLimitingContext { Parameter = "test" };

        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _checker.CheckAsync("TestCustomResolverNull", context);
        });

        exception.Message.ShouldContain("Custom partition key resolver returned null or empty");
    }

    [Fact]
    public void Should_Throw_When_Policy_Has_Duplicate_Rules()
    {
        var options = new AbpOperationRateLimitingOptions();

        Assert.Throws<AbpException>(() =>
        {
            options.AddPolicy("DuplicateRulePolicy", policy =>
            {
                policy.AddRule(r => r.WithFixedWindow(TimeSpan.FromHours(1), 5).PartitionByParameter());
                policy.AddRule(r => r.WithFixedWindow(TimeSpan.FromHours(1), 5).PartitionByParameter());
            });
        });
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(Guid userId)
    {
        return new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(AbpClaimTypes.UserId, userId.ToString()),
                    new Claim(AbpClaimTypes.Email, "test@example.com"),
                    new Claim(AbpClaimTypes.PhoneNumber, "1234567890")
                },
                "TestAuth"));
    }
}
