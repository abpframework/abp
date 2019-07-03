#!/bin/bash

# COMMON PATHS

rootFolder=`pwd`

# List of solutions

solutionPaths=(
    "framework"
    "modules/users"
    "modules/permission-management"
    "modules/setting-management"
    "modules/feature-management"
    "modules/identity"
    "modules/identityserver"
    "modules/tenant-management"
    "modules/account"
    "modules/docs"
    "modules/blogging"
    "modules/audit-logging"
    "modules/background-jobs"
    "modules/client-simulation"
    "templates/mvc-module"
    "templates/mvc"
    "samples/MicroserviceDemo"
    "abp_io/AbpIoLocalization"
)

# Build all solutions

for solutionPath in ${solutionPaths[@]}
do
    cd ${rootFolder}/${solutionPath}
    dotnet build
    if [[ $? -eq 0 ]]
    then
        echo "Build completed for the solution: "${solutionPath}
    else
        echo "Build failed for the solution: "${solutionPath}
        cd ${rootFolder}
        exit 0
    fi
done

cd ${rootFolder}
