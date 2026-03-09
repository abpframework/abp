using System;
using System.Threading.Tasks;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingRuleBuilder
{
    private readonly OperationRateLimitingPolicyBuilder _policyBuilder;
    private TimeSpan _duration;
    private int _maxCount;
    private OperationRateLimitingPartitionType? _partitionType;
    private Func<OperationRateLimitingContext, Task<string>>? _customPartitionKeyResolver;
    private bool _isMultiTenant;

    internal bool IsCommitted { get; private set; }

    internal OperationRateLimitingRuleBuilder(OperationRateLimitingPolicyBuilder policyBuilder)
    {
        _policyBuilder = policyBuilder;
    }

    public OperationRateLimitingRuleBuilder WithFixedWindow(
        TimeSpan duration, int maxCount)
    {
        _duration = duration;
        _maxCount = maxCount;
        return this;
    }

    public OperationRateLimitingRuleBuilder WithMultiTenancy()
    {
        _isMultiTenant = true;
        return this;
    }

    /// <summary>
    /// Use context.Parameter as partition key.
    /// </summary>
    public OperationRateLimitingPolicyBuilder PartitionByParameter()
    {
        _partitionType = OperationRateLimitingPartitionType.Parameter;
        CommitToPolicyBuilder();
        return _policyBuilder;
    }

    /// <summary>
    /// Partition by the current authenticated user (ICurrentUser.Id).
    /// Use PartitionByParameter() if you need to specify the user ID explicitly.
    /// </summary>
    public OperationRateLimitingPolicyBuilder PartitionByCurrentUser()
    {
        _partitionType = OperationRateLimitingPartitionType.CurrentUser;
        CommitToPolicyBuilder();
        return _policyBuilder;
    }

    /// <summary>
    /// Partition by the current tenant (ICurrentTenant.Id). Uses "host" when no tenant is active.
    /// </summary>
    public OperationRateLimitingPolicyBuilder PartitionByCurrentTenant()
    {
        _partitionType = OperationRateLimitingPartitionType.CurrentTenant;
        CommitToPolicyBuilder();
        return _policyBuilder;
    }

    /// <summary>
    /// Partition by the client IP address (IWebClientInfoProvider.ClientIpAddress).
    /// Use PartitionByParameter() if you need to specify the IP explicitly.
    /// </summary>
    public OperationRateLimitingPolicyBuilder PartitionByClientIp()
    {
        _partitionType = OperationRateLimitingPartitionType.ClientIp;
        CommitToPolicyBuilder();
        return _policyBuilder;
    }

    /// <summary>
    /// Partition by email address.
    /// Resolves from context.Parameter, falls back to ICurrentUser.Email.
    /// </summary>
    public OperationRateLimitingPolicyBuilder PartitionByEmail()
    {
        _partitionType = OperationRateLimitingPartitionType.Email;
        CommitToPolicyBuilder();
        return _policyBuilder;
    }

    /// <summary>
    /// Partition by phone number.
    /// Resolves from context.Parameter, falls back to ICurrentUser.PhoneNumber.
    /// </summary>
    public OperationRateLimitingPolicyBuilder PartitionByPhoneNumber()
    {
        _partitionType = OperationRateLimitingPartitionType.PhoneNumber;
        CommitToPolicyBuilder();
        return _policyBuilder;
    }

    /// <summary>
    /// Custom async partition key resolver from context.
    /// </summary>
    public OperationRateLimitingPolicyBuilder PartitionBy(
        Func<OperationRateLimitingContext, Task<string>> keyResolver)
    {
        _partitionType = OperationRateLimitingPartitionType.Custom;
        _customPartitionKeyResolver = Check.NotNull(keyResolver, nameof(keyResolver));
        CommitToPolicyBuilder();
        return _policyBuilder;
    }

    protected virtual void CommitToPolicyBuilder()
    {
        _policyBuilder.AddRuleDefinition(Build());
        IsCommitted = true;
    }

    internal OperationRateLimitingRuleDefinition Build()
    {
        if (_duration <= TimeSpan.Zero)
        {
            throw new AbpException(
                "Operation rate limit rule requires a positive duration. " +
                "Call WithFixedWindow(duration, maxCount) before building the rule.");
        }

        if (_maxCount < 0)
        {
            throw new AbpException(
                "Operation rate limit rule requires maxCount >= 0. " +
                "Use maxCount: 0 to completely deny all requests (ban policy).");
        }

        if (!_partitionType.HasValue)
        {
            throw new AbpException(
                "Operation rate limit rule requires a partition type. " +
                "Call PartitionByParameter(), PartitionByCurrentUser(), PartitionByClientIp(), or another PartitionBy*() method.");
        }

        if (_partitionType == OperationRateLimitingPartitionType.Custom && _customPartitionKeyResolver == null)
        {
            throw new AbpException(
                "Custom partition type requires a key resolver. " +
                "Call PartitionBy(keyResolver) instead of setting partition type directly.");
        }

        return new OperationRateLimitingRuleDefinition
        {
            Duration = _duration,
            MaxCount = _maxCount,
            PartitionType = _partitionType.Value,
            CustomPartitionKeyResolver = _customPartitionKeyResolver,
            IsMultiTenant = _isMultiTenant
        };
    }
}
