echo "Creating non-deployments"
kubectl create -f (Get-ChildItem -Recurse -File -Filter "*.yaml" -Exclude "*deployment*.yaml" |
Group-Object -Property Directory |
ForEach-Object {
    @(
        $_.Group |
        Resolve-Path -Relative |   # make relative path
        ForEach-Object Substring 2 # cut '.\' part
    )-join','
})

echo "Creating deployments"
kubectl create -f (Get-ChildItem -Recurse -File -Filter "*deployment*.yaml" |
Group-Object -Property Directory |
ForEach-Object {
    @(
        $_.Group |
        Resolve-Path -Relative |   # make relative path
        ForEach-Object Substring 2 # cut '.\' part
    )-join','
})

echo "Forwarding ports"
$jobs=@()
$portforward = {
    param($app, $port)
    kubectl port-forward "$app" "$port"
}

$jobs+=Start-Job -ScriptBlock $portforward -ArgumentList deployment.apps/auth-server,51511:51511
$jobs+=Start-Job -ScriptBlock $portforward -ArgumentList deployment.apps/backend-admin-app,51512:80
$jobs+=Start-Job -ScriptBlock $portforward -ArgumentList deployment.apps/public-website,51513:80
Wait-Job $jobs
