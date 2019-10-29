const glob = require('glob');
var path = require('path');
const childProcess = require('child_process');

const gulp = pkgJsonPath => {
  try {
    console.log('Running the yarn command... Cwd: ' + pkgJsonPath);
    childProcess.execSync(`yarn`, { cwd: pkgJsonPath });
    console.log('Running the gulp command...');
    return childProcess.execSync(`gulp`, { cwd: pkgJsonPath, stdio: 'inherit' });
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

    gulp(file.replace('/package.json', ''));
  });
});
