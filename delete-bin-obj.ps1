Clear-Host
Write-Host "Deleting all BIN and OBJ folders..." -ForegroundColor Cyan
Get-ChildItem -Path . -Include bin,obj -Recurse -Directory | ForEach-Object {
    if ($_.FullName -notmatch "\\node_modules\\") {
        Write-Host "Deleting:" $_.FullName -ForegroundColor Yellow
        Remove-Item $_.FullName -Recurse -Force
    } else {
        Write-Host "Skipping:" $_.FullName -ForegroundColor Magenta
    }
}

Write-Host "BIN and OBJ folders have been successfully deleted." -ForegroundColor Green

