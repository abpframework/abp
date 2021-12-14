. ..\nupkg\common.ps1
Write-Info "Creating NuGet packages"

echo "`n-----=====[ CREATING NUGET PACKAGES ]=====-----`n"
cd ..\nupkg
powershell -File pack.ps1
echo "`n-----=====[ CREATING NUGET PACKAGES COMPLETED ]=====-----`n"

Write-Info "Creating NuGet packages completed"
cd ..\deploy #always return to the deploy directory