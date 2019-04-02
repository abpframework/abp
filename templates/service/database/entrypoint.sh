#!/bin/bash

cd IdentityServerHost
export ConnectionStrings__Default=$IdentityServerConnectionString

until dotnet ef database update --no-build; do
>&2 echo "SQL Server is starting up"
sleep 1
done

export ConnectionStrings__Default=$MyProjectNameConnectionString
cd MyCompanyName.MyProjectName.Host && dotnet ef database update --no-build