. ".\common.ps1"

# Unit test for all solutions
foreach($solution in $solutions) {
    $solutionFolder = Join-Path $rootFolder $solution
    Set-Location $solutionFolder
    dotnet test --no-build
 
 
}