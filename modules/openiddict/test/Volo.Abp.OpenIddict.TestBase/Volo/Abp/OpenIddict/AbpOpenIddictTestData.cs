using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictTestData : ISingletonDependency
{
    public Guid App1Id { get; set; } = Guid.NewGuid();
    public string App1ClientId { get; set; } = "Client1";
    public Guid App2Id { get; set; } = Guid.NewGuid();
    public string App2ClientId { get; set; } = "Client2";
    public Guid Scope1Id { get; set; } = Guid.NewGuid();
    public string Scope1Name { get; set; } = "Scope1";
    public Guid Scope2Id { get; set; } = Guid.NewGuid();
    public string Subject1 { get; set; } = "Subject1";
    public string Subject2 { get; set; } = "Subject2";
    public string Subject3 { get; set; } = "Subject3";

    public string Scope2Name { get; set; } = "Scope2";

    public Guid Token1Id { get; set; } = Guid.NewGuid();

    public Guid Token2Id { get; set; } = Guid.NewGuid();

    public Guid Authorization1Id { get; set; } = Guid.NewGuid();

    public Guid Authorization2Id { get; set; } = Guid.NewGuid();
}
