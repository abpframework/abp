echo "`n-----=====[ CREATING NUGET PACKAGES ]=====-----`n"

cd ..\nupkg
powershell -File pack.ps1

echo "`n-----=====[ CREATING NUGET PACKAGES COMPLETED ]=====-----`n"
