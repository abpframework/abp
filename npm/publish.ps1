& cd ng-packs\scripts
& npm install
& yarn publish-packages patch
& cd ../../
& yarn
& yarn lerna publish patch --no-push --yes --no-git-reset --no-commit-hooks --no-git-tag-version --force-publish
& yarn update:templates
& yarn global add gulp
& yarn gulp:app
& yarn gulp:module
