$folders = Get-ChildItem -Directory

foreach ($folder in $folders) {
    $zipFile = "./" + $folder.Name + ".zip"
    
    if (Test-Path $zipFile) {
        Remove-Item $zipFile
    }

    Compress-Archive -Path $folder.FullName -DestinationPath $zipFile
}

Write-Host "All templates have been zipped."
