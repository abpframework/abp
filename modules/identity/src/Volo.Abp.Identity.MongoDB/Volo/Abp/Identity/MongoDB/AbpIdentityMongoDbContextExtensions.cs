using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

public static class AbpIdentityMongoDbContextExtensions
{
    public static void ConfigureIdentity(this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IdentityUser>(b =>
        {
            b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "Users";
        });

        builder.Entity<IdentityRole>(b =>
        {
            b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "Roles";
        });

        builder.Entity<IdentityClaimType>(b =>
        {
            b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "ClaimTypes";
        });

        builder.Entity<OrganizationUnit>(b =>
        {
            b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "OrganizationUnits";
        });

        builder.Entity<IdentitySecurityLog>(b =>
        {
            b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "SecurityLogs";
        });

        builder.Entity<IdentityLinkUser>(b =>
        {
            b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "LinkUsers";
        });
    }
}
