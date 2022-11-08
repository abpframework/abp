﻿using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB;

public static class AbpPermissionManagementMongoDbContextExtensions
{
    public static void ConfigurePermissionManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<PermissionGroupDefinitionRecord>(b =>
        {
            b.CollectionName = AbpPermissionManagementDbProperties.DbTablePrefix + "PermissionGroups";
        });
        
        builder.Entity<PermissionDefinitionRecord>(b =>
        {
            b.CollectionName = AbpPermissionManagementDbProperties.DbTablePrefix + "Permissions";
        });
        
        builder.Entity<PermissionGrant>(b =>
        {
            b.CollectionName = AbpPermissionManagementDbProperties.DbTablePrefix + "PermissionGrants";
        });
    }
}
