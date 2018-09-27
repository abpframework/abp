# COMMON PATHS

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionsPaths = (
    "framework",
    "modules/users",
    "modules/permission-management",
    "modules/setting-management",
    "modules/identity",
    "modules/identityserver",
    "modules/tenant-management",
    "modules/account",
    "modules/docs",
    "modules/blogging",
    "modules/audit-logging",
    "modules/background-jobs"
)

# List of test projects

$testProjectPaths = (
    "framework/test/Volo.Abp.AspNetCore.Authentication.OAuth.Tests",
    "framework/test/Volo.Abp.AspNetCore.MultiTenancy.Tests",
    "framework/test/Volo.Abp.AspNetCore.Mvc.Tests",
    "framework/test/Volo.Abp.AspNetCore.Mvc.UI.Tests",
    "framework/test/Volo.Abp.AspNetCore.Mvc.Versioning.Tests",
    "framework/test/Volo.Abp.AspNetCore.Tests",
    "framework/test/Volo.Abp.Auditing.Tests",
    "framework/test/Volo.Abp.Authorization.Tests",
    "framework/test/Volo.Abp.Autofac.Tests",
    "framework/test/Volo.Abp.AutoMapper.Tests",
    "framework/test/Volo.Abp.Caching.Tests",
    "framework/test/Volo.Abp.Castle.Core.Tests",
    "framework/test/Volo.Abp.Core.Tests",
    "framework/test/Volo.Abp.Data.Tests",
    "framework/test/Volo.Abp.Ddd.Tests",
    "framework/test/Volo.Abp.EntityFrameworkCore.Tests",
    "framework/test/Volo.Abp.EventBus.Distributed.Tests",
    "framework/test/Volo.Abp.EventBus.Tests",
    "framework/test/Volo.Abp.Http.Client.Tests",
    "framework/test/Volo.Abp.Localization.Tests",
    "framework/test/Volo.Abp.MemoryDb.Tests",
    "framework/test/Volo.Abp.MongoDB.Tests",
    "framework/test/Volo.Abp.MultiTenancy.Tests",
    "framework/test/Volo.Abp.Serialization.Tests",
    "framework/test/Volo.Abp.TestApp.Tests",
    "framework/test/Volo.Abp.UI.Navigation.Tests",
    "framework/test/Volo.Abp.Uow.Tests",
    "framework/test/Volo.Abp.Validation.Tests",
    "framework/test/Volo.Abp.VirtualFileSystem.Tests",
    "modules/blogging/test/Volo.Blogging.Application.Tests",
    "modules/blogging/test/Volo.Blogging.EntityFrameworkCore.Tests",
    "modules/identity/test/Volo.Abp.Identity.Domain.Tests",
    "modules/identity/test/Volo.Abp.Identity.EntityFrameworkCore.Tests",
    "modules/identityserver/test/Volo.Abp.IdentityServer.EntityFrameworkCore.Tests",
    "modules/identity/test/Volo.Abp.Identity.MongoDB.Tests",
    "modules/identity/test/Volo.Abp.Identity.Application.Tests",
    "modules/permission-management/test/Volo.Abp.PermissionManagement.Tests",
    "modules/permission-management/test/Volo.Abp.PermissionManagement.MongoDB.Tests",
    "modules/permission-management/test/Volo.Abp.PermissionManagement.EntityFrameworkCore.Tests",
    "modules/setting-management/test/Volo.Abp.SettingManagement.EntityFrameworkCore.Tests",
    "modules/setting-management/test/Volo.Abp.SettingManagement.MongoDB.Tests",
    "modules/setting-management/test/Volo.Abp.SettingManagement.Tests",
    "modules/tenant-management/test/Volo.Abp.TenantManagement.EntityFrameworkCore.Tests",
    "modules/tenant-management/test/Volo.Abp.TenantManagement.MongoDB.Tests",
    "modules/tenant-management/test/Volo.Abp.TenantManagement.Application.Tests",
    "modules/audit-logging/test/Volo.Abp.AuditLogging.EntityFrameworkCore.Tests",
    "modules/audit-logging/test/Volo.Abp.AuditLogging.MongoDB.Tests",
    "modules/audit-logging/test/Volo.Abp.AuditLogging.Tests",
    "modules/background-jobs/test/Volo.Abp.BackgroundJobs.EntityFrameworkCore.Tests",
    "modules/background-jobs/test/Volo.Abp.BackgroundJobs.MongoDB.Tests",
    "modules/background-jobs/test/Volo.Abp.BackgroundJobs.Domain.Tests"
)

# Build all solutions

foreach ($solutionsPath in $solutionsPaths) {    
    $solutionAbsPath = (Join-Path $rootFolder $solutionsPath)
    Set-Location $solutionAbsPath
    dotnet build
    if (-Not $?) {
        Write-Host ("Build failed for the solution: " + $solutionsPath)
        exit $LASTEXITCODE
    }
}

# Test all projects

foreach ($testProjectPath in $testProjectPaths) {    
    $projectAbsPath = (Join-Path $rootFolder $testProjectPath)
    Set-Location $projectAbsPath
    dotnet test
    if (-Not $?) {
        Write-Host ("Test failed for the project: " + $testProjectPath)
        exit $LASTEXITCODE
    }
}

Set-Location $rootFolder
