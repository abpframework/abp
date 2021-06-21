const glob = require('glob');
var path = require('path');
const childProcess = require('child_process');
const execa = require('execa');
const fse = require('fs-extra');
const { program } = require('commander');

program.version('0.0.1');
program.option('-r, --rc', 'whether version is rc');
program.parse(process.argv);

const gulp = (folderPath) => {
  if (
    !fse.existsSync(folderPath + 'gulpfile.js') ||
    !glob.sync(folderPath + '*.csproj').length
  ) {
    return;
  }

  try {
    execa.sync(`yarn`, ['install'], { cwd: folderPath, stdio: 'inherit' });
    execa.sync(`yarn`, ['gulp'], { cwd: folderPath, stdio: 'inherit' });
  } catch (error) {
    console.log('\x1b[31m', 'Error: ' + error.message);
  }
};

const updatePackages = (pkgJsonPath) => {
  try {
    const result = childProcess
      .execSync(
        `ncu "/^@abp.*$/" --packageFile ${pkgJsonPath} -u${
          program.rc ? ' --target greatest' : ''
        }`
      )
      .toString();
    console.log('\x1b[0m', result);
  } catch (error) {
    console.log('\x1b[31m', 'Error: ' + error.message);
  }
};

console.time();
glob('../**/package.json', {}, (er, files) => {
  files = files.filter(
    (f) =>
      f &&
      !f.includes('node_modules') &&
      !f.includes('wwwroot') &&
      !f.includes('bin') &&
      !f.includes('obj')
  );

  files.forEach((file) => {
    updatePackages(file);
    gulp(file.replace('package.json', ''));
  });
  console.timeEnd();
});
