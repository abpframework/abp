& cd ng-packs\scripts
& npm install
& npm run build
& cd ../../
& yarn
& yarn lerna publish patch --no-push --yes --no-git-reset --no-commit-hooks --no-git-tag-version
& cd ng-packs\scripts
& npm run sync
& cd ../../
& yarn update:templates
& yarn global add gulp
& yarn gulp:app
& yarn gulp:module