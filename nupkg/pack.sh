#!/bin/bash
. ./common.sh

# Rebuild all solutions
for solution in "${solutions[@]}"
do
    solutionFolder="$rootFolder/$solution"
    cd "$solutionFolder" \
    && dotnet restore
done

# Create all packages
for project in "${projects[@]}"
do
    projectFolder="$rootFolder/$project"

    # Create nuget pack
    cd "$projectFolder"
    rm -rf "$projectFolder/bin/Release" \
    & dotnet pack --no-restore -c Release -p:SourceLinkCreate=true -o "$packFolder"

    if [ $? -ne 0 ]
    then
        echo "Packaging failed for the project: $projectFolder"
        exit 1
    fi
done

# Go back to the pack folder
cd "$packFolder"