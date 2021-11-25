﻿using Volo.Abp.Data;

namespace Volo.Abp.Identity;

public static class AbpIdentityDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpIdentity";
}
