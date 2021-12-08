cd ..

echo "`n-----=====[ COMMITTING CHANGES ]=====-----`n"
git add .
git commit -m Update_NPM_Package_Versions
git push 

echo "`n-----=====[ PUSHING ANGULAR PACKS TO NPM ]=====-----`n"
powershell -File publish-ng.ps1

echo "`n-----=====[ PUSHING PACKS TO NPM COMPLETED ]=====-----`n"
