# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src"

# List of projects
$projects = (
    "Volo.Abp",
    "Volo.Abp.Account.Application",
    "Volo.Abp.Account.Application.Contracts",
    "Volo.Abp.Account.Web",    
    "Volo.Abp.ApiVersioning.Abstractions",
    "Volo.Abp.AspNetCore",
    "Volo.Abp.AspNetCore.EmbeddedFiles",
    "Volo.Abp.AspNetCore.MultiTenancy",
    "Volo.Abp.AspNetCore.Mvc",
    "Volo.Abp.AspNetCore.Mvc.UI",
    "Volo.Abp.AspNetCore.Mvc.UI.Bootstrap",
    "Volo.Abp.AspNetCore.TestBase",    
    "Volo.Abp.Authorization",
    "Volo.Abp.AutoFac",
    "Volo.Abp.AutoMapper",
    "Volo.Abp.Caching",
    "Volo.Abp.Castle.Core",
    "Volo.Abp.Core",
    "Volo.Abp.Data",
    "Volo.Abp.Ddd",
    "Volo.Abp.EntityFrameworkCore",
    "Volo.Abp.EventBus",
    "Volo.Abp.Guids",
    "Volo.Abp.Http",
    "Volo.Abp.Http.Abstractions",
    "Volo.Abp.Http.Client",    
    "Volo.Abp.Identity.Application",
    "Volo.Abp.Identity.Application.Contracts",
    "Volo.Abp.Identity.Domain",
    "Volo.Abp.Identity.Domain.Shared",
    "Volo.Abp.Identity.EntityFrameworkCore",
    "Volo.Abp.Identity.HttpApi",
    "Volo.Abp.Identity.HttpApi.Client",
    "Volo.Abp.Identity.HttpApi.Host",
    "Volo.Abp.Identity.Web",    
    "Volo.Abp.IdentityServer.Application",
    "Volo.Abp.IdentityServer.Application.Contracts",
    "Volo.Abp.IdentityServer.Domain",
    "Volo.Abp.IdentityServer.Domain.Shared",
    "Volo.Abp.IdentityServer.EntityFrameworkCore",
    "Volo.Abp.Json",
    "Volo.Abp.Localization",
    "Volo.Abp.MemoryDb",
    "Volo.Abp.MongoDB",
    "Volo.Abp.MultiTenancy.Abstractions",
    "Volo.Abp.MultiTenancy.Application",
    "Volo.Abp.MultiTenancy.Application.Contracts",
    "Volo.Abp.MultiTenancy.Domain",
    "Volo.Abp.MultiTenancy.Domain.Shared",
    "Volo.Abp.MultiTenancy.EntityFrameworkCore",
    "Volo.Abp.MultiTenancy.HttpApi",
    "Volo.Abp.MultiTenancy.Web",    
    "Volo.Abp.ObjectMapping",
    "Volo.Abp.Permissions.Application",
    "Volo.Abp.Permissions.Application.Contracts",
    "Volo.Abp.Permissions.Domain",
    "Volo.Abp.Permissions.Domain.Shared",
    "Volo.Abp.Permissions.EntityFrameworkCore",
    "Volo.Abp.Permissions.Web",    
    "Volo.Abp.Security",
    "Volo.Abp.Serialization",
    "Volo.Abp.Session",
    "Volo.Abp.Settings",
    "Volo.Abp.Settings.Domain",
    "Volo.Abp.Settings.Domain.Shared",
    "Volo.Abp.Settings.EntityFrameworkCore",
    "Volo.Abp.TestBase",
    "Volo.Abp.Threading",
    "Volo.Abp.Timing",
    "Volo.Abp.UI",
    "Volo.Abp.UI.Navigation",
    "Volo.Abp.Validation",
    "Volo.Abp.VirtualFileSystem"
)

# Rebuild solution
Set-Location $slnPath
& dotnet restore

# Copy all nuget packages to the pack folder
foreach($project in $projects) {
    
    $projectFolder = Join-Path $srcPath $project

    # Create nuget pack
    Set-Location $projectFolder
    Remove-Item -Recurse (Join-Path $projectFolder "bin/Release")
    & dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true

    # Copy nuget package
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $project + ".*.nupkg")
    Move-Item $projectPackPath $packFolder

}

# Go back to the pack folder
Set-Location $packFolder