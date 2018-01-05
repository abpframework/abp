# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src"

# List of projects
$projects = (
    "Volo.Abp.Core",
    "Volo.Abp.Authorization",
    "Volo.Abp.Validation",
    "Volo.Abp.Localization",
    "Volo.Abp.ObjectMapping",
    "Volo.Abp.Security",
    "Volo.Abp.Session",
    "Volo.Abp.Data",
    "Volo.Abp.Json",
    "Volo.Abp.VirtualFileSystem",
    "Volo.Abp.ApiVersioning.Abstractions",
    "Volo.Abp.MultiTenancy.Abstractions",

    "Volo.Abp", # Meta package (uses all above)

	"Volo.Abp.Guids",
	"Volo.Abp.Threading",
	"Volo.Abp.Timing",
	"Volo.Abp.Ddd",
	"Volo.Abp.MultiTenancy",

	"Volo.Abp.Castle.Core",
	"Volo.Abp.AutoFac",

	"Volo.Abp.AutoMapper",

   	"Volo.Abp.Http.Abstractions",
   	"Volo.Abp.Http",
	"Volo.Abp.Http.Client",
    
	"Volo.Abp.UI",
	"Volo.Abp.UI.Navigation",

	"Volo.Abp.AspNetCore",
	"Volo.Abp.AspNetCore.EmbeddedFiles",
	"Volo.Abp.AspNetCore.MultiTenancy",
	"Volo.Abp.AspNetCore.Mvc",
	"Volo.Abp.AspNetCore.Mvc.UI",
	"Volo.Abp.AspNetCore.Mvc.UI.Bootstrap",
	"Volo.Abp.AspNetCore.TestBase",
    
	"Volo.Abp.MemoryDb",
	"Volo.Abp.MongoDB",
	"Volo.Abp.EntityFrameworkCore",

	"Volo.Abp.TestBase",

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

	"Volo.Abp.Account.Application",
	"Volo.Abp.Account.Application.Contracts",
	"Volo.Abp.Account.Web"
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