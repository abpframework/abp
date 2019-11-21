& cd ng-packs\scripts
& npm install
& npm run build
& cd ../../
& yarn
& yarn lerna publish --no-push
& cd ng-packs\scripts
& npm run sync
& cd ../../
& yarn update:templates
& yarn global add gulp
& yarn gulp:app
& yarn gulp:module