Get-ChildItem -Recurse -Filter package.json -ErrorAction SilentlyContinue |
    Where-Object { $_.FullName -notmatch '\\node_modules\\' } |
    ForEach-Object {
        Write-Host "🔍 Checking $($_.FullName)" -ForegroundColor Cyan
        Push-Location $_.DirectoryName
        try {
           npx npm-check-updates
        }
        catch {
            Write-Host "⚠️ Error running ncu in $($_.DirectoryName)" -ForegroundColor Red
        }
        Pop-Location
    }

Write-Host "`n✅ All package.json files have been checked." -ForegroundColor Green