. ..\nupkg\common.ps1

cd ..

Write-Info "Committing changes to GitHub"
echo "`n-----=====[ COMMITTING CHANGES ]=====-----`n"
git add .
git commit -m Update_NPM_Package_Versions
git push 

echo "`n-----=====[ COMMITTING CHANGES COMPLETED ]=====-----`n"
Write-Info "Completed: Committing changes to GitHub"

cd deploy #always return to the deploy directory