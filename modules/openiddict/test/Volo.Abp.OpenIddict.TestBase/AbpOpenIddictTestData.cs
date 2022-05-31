using System;

namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictTestData
{
    public static Guid App1Id { get; set; } = Guid.NewGuid();
    public static string App1ClientId { get; set; } = "Client1";
    public static Guid App2Id { get; set; } = Guid.NewGuid();
    public static string App2ClientId { get; set; } = "Client2";

    public static Guid Scope1Id { get; set; } = Guid.NewGuid();
    public static string Scope1Name { get; set; } = "Scope1";
    public static Guid Scope2Id { get; set; } = Guid.NewGuid();
    public static string Scope2Name { get; set; } = "Scope2";
}
