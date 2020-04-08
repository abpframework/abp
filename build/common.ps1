# COMMON PATHS

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionPaths = (
    "../framework",
    "../modules/users",
    "../modules/permission-management",
    "../modules/setting-management",
    "../modules/feature-management",
    "../modules/identity",
    "../modules/identityserver",
    "../modules/tenant-management",
    "../modules/account",
    "../modules/docs",
    "../modules/blogging",
    "../modules/audit-logging",
    "../modules/background-jobs",
    "../modules/client-simulation",
    "../templates/module/aspnet-core",
    "../templates/app/aspnet-core",
    "../samples/MicroserviceDemo",
    "../abp_io/AbpIoLocalization"
)