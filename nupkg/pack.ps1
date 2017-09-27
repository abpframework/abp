# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src"

# List of projects
$projects = (
    "Volo.Abp",
	"Volo.Abp.AspNetCore",
	"Volo.Abp.AspNetCore.EmbeddedFiles",
	"Volo.Abp.AspNetCore.MultiTenancy",
	"Volo.Abp.AspNetCore.Mvc",
	"Volo.Abp.AspNetCore.Mvc.UI",
	"Volo.Abp.AspNetCore.Mvc.UI.Bootstrap",
	"Volo.Abp.AspNetCore.TestBase",
	"Volo.Abp.AutoFac",
	"Volo.Abp.AutoMapper",
	"Volo.Abp.Castle.Core",
	"Volo.Abp.EntityFrameworkCore",
	"Volo.Abp.Http",
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
	"Volo.Abp.MemoryDb",
	"Volo.Abp.MongoDB",
	"Volo.Abp.MultiTenancy",
	"Volo.Abp.TestBase"
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