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
    "../modules/audit-logging",
    "../modules/background-jobs",
    "../modules/account",
    
    "../modules/client-simulation",
    "../modules/virtual-file-explorer",
    "../modules/docs",
    "../modules/blogging",
    "../templates/module/aspnet-core",
    "../templates/app/aspnet-core",
    "../abp_io/AbpIoLocalization"
)