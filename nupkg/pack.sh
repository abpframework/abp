#!/bin/bash
. ./common.sh
set -eE

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
done

# Go back to the pack folder
cd "$packFolder"