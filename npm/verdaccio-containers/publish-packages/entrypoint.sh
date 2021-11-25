#!/bin/bash
echo "v" $next_version " packages to be published to the verdaccio"


echo "Waiting for the Verdaccio"
while ! curl -v --silent verdaccio:4873 &> /dev/null
do
    printf "%c" "."
done

var="$(curl -v --silent verdaccio:4873 2>&1 | grep Trying)"
echo $var

curl -XPUT -H "Content-type: application/json" -d '{ "name": "volo", "password": "123456", "email": "verdaccio@volo.com" }' 'verdaccio:4873/-/user/org.couchdb.user:your_username'

npx npm-cli-login -u volo -p 123456 -e "verdaccio@volo.com" -r "http://verdaccio:4873"
npm whoami --registry http://verdaccio:4873


cd /publish/abp/npm/ng-packs/scripts

npm install

echo "Running the publish script for abp packages"

npm run publish-packages -- --nextVersion $next_version --skipGit --registry "http://verdaccio:4873"

cd /publish/abp/npm/ng-packs

echo '@abp:registry=http://verdaccio:4873' >> .npmrc

npx npm-check-updates --filter '/^@(abp)\/.*$/' --registry http://verdaccio:4873 --target greatest --packageFile package.json -u

cd scripts

npm install

echo "Running the publish script for abp packages"

npm run publish-packages -- --nextVersion $next_version --skipGit --registry "http://verdaccio:4873"