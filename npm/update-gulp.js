const glob = require('fast-glob');
var path = require('path');
const childProcess = require('child_process');
const execa = require('execa');
const fse = require('fs-extra');
const { program } = require('commander');

program.version('0.0.1');
program.option('-pr, --prerelease', 'whether version is prerelease');
program.option(
  '-rg, --registry <registry>',
  'NPM server registry',
  'https://registry.npmjs.org'
);
program.parse(process.argv);

const gulp = (folderPath) => {
  if (
    !fse.existsSync(folderPath + 'gulpfile.js') ||
    !glob.sync(folderPath + '*.csproj').length
  ) {
    return;
  }

  try {
    fse.removeSync(`${folderPath}/wwwroot/libs`);
    execa.sync('yarn', ['install'], { cwd: folderPath, stdio: 'inherit' });
    execa.sync('yarn', ['gulp'], { cwd: folderPath, stdio: 'inherit' });
  } catch (error) {
    console.log('\x1b[31m', 'Error: ' + error.message);
  }
};

const updatePackages = (pkgJsonPath) => {
  try {
    const result = childProcess
      .execSync(
        `ncu "/^@abp.*$/" --packageFile ${pkgJsonPath} -u${
          program.prerelease ? ' --target newest' : ''
        } --registry ${program.registry}`
      )
      .toString();
    console.log('\x1b[0m', result);
  } catch (error) {
    console.log('\x1b[31m', 'Error: ' + error.message);
  }
};

(async () => {
  console.time();
  let files = await glob('../**/package.json');
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
    const folderPath = file.replace('package.json', '');
    gulp(folderPath);
  });

  console.timeEnd();
})();
