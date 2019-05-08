#!/bin/bash

cd /src/ProductService.Host

until dotnet ef database update --no-build; do
>&2 echo "SQL Server is starting up"
sleep 1
done

cd /src/AuthServer.Host && dotnet ef database update --no-build