const fse = require('fs-extra');
const path = require('path')


// packages
fse.removeSync(path.join(__dirname, 'publish-packages/abp'))

fse.removeSync(path.join(__dirname, '../ng-packs/node_modules'))
fse.removeSync(path.join(__dirname, '../ng-packs/scripts/node_modules'))
fse.copySync(path.join(__dirname, '../ng-packs'), path.join(__dirname, 'publish-packages/abp/npm/ng-packs'), {overwrite: true, errorOnExist: false, })

// app angular template
fse.removeSync(path.join(__dirname, 'serve-app/app'))

fse.removeSync(path.join(__dirname, '../../templates/app/angular/node_modules'));
fse.copySync(path.join(__dirname, '../../templates/app/angular'), path.join(__dirname, 'serve-app/app'), {overwrite: true, errorOnExist: false, })
