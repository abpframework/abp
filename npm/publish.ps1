& cd ng-packs\scripts
& npm install
& yarn build
& cd ../../
& yarn
& yarn lerna publish --no-push
& cd ng-packs\scripts
& yarn sync
& cd ../../
& yarn update:templates