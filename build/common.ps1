$full = $args[0]

# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
$solutionPaths = @(
		"../framework",
		"../studio",
		"../modules/basic-theme",
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
		"../modules/cms-kit",
		"../modules/blob-storing-database",
		"../studio"
	)

if ($full -eq "-f")
{
	# List of additional solutions required for full build
	$solutionPaths += (
		"../modules/client-simulation",
		"../modules/virtual-file-explorer",
		"../modules/docs",
		"../modules/blogging",
		"../templates/module/aspnet-core",
		"../templates/app/aspnet-core",
		"../templates/console",
		"../templates/wpf",
		"../templates/app-nolayers/aspnet-core",
		"../abp_io/AbpIoLocalization"
	) 
}else{ 
	Write-host ""
	Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
	Write-host "" 
} 
