const glob = require('glob');
var path = require('path');
const childProcess = require('child_process');

const check = pkgJsonPath => {
  try {
    return childProcess.execSync(`ncu "/^@abp.*$/" --packageFile ${pkgJsonPath} -u`).toString();
  } catch (error) {
    console.log('exec error: ' + error.message);
    process.exit(error.status);
  }
};

const folder = process.argv[2] || '.';

glob(folder + '/**/package.json', {}, (er, files) => {
  files.forEach(file => {
    if (file.includes('node_modules')) {
      return;
    }

    console.log(check(file));
  });
});
