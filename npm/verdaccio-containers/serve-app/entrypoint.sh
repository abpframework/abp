#!/bin/bash
while ping -c1 publish &> /dev/null 
do echo "Waiting for publishing to be done"; sleep 10; 
done;

echo "Serving app"

cd /app/app

echo "@abp:registry=http://verdaccio:4873" >> .npmrc
echo '@volo:registry=http://verdaccio:4873' >> .npmrc

npx npm-check-updates --filter '/^@(abp|volo)\/.*$/' --registry http://verdaccio:4873 --target greatest --packageFile package.json -u

yarn

yarn ng build --prod

cd dist/MyProjectName

npx http-server-spa . index.html 4200