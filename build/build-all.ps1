$full = $args[0] 

. ".\common.ps1" $full

# Build all solutions   

Write-Host $solutionPaths

dotnet workload install wasm-tools
dotnet workload install maui-tizen

foreach ($solutionPath in $solutionPaths) {    
    $solutionAbsPath = (Join-Path $rootFolder $solutionPath)
    Set-Location $solutionAbsPath
    dotnet build
    if (-Not $?) {
        Write-Host ("Build failed for the solution: " + $solutionPath)
        Set-Location $rootFolder
        exit $LASTEXITCODE
    }
}

Set-Location $rootFolder
