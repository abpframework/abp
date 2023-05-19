using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Account;

public class AccountTestData : ISingletonDependency
{
    public Guid UserJohnId { get; } = Guid.NewGuid();
}
