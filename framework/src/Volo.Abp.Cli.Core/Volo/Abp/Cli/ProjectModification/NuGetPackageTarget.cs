﻿namespace Volo.Abp.Cli.ProjectModification
{
    public enum NuGetPackageTarget : byte
    {
        DomainShared = 1,
        Domain = 2,
        ApplicationContracts = 3,
        Application = 4,
        HttpApi = 5,
        HttpApiClient = 6,
        Web = 7,
        EntityFrameworkCore = 8,
        MongoDB = 9
    }
}