const glob = require('glob');
var path = require('path');
const childProcess = require('child_process');
const { program } = require('commander');

program.version('0.0.1');
program.option('-pr, --prerelease', 'whether version is prerelease');
program.option('-rg, --registry <registry>', 'target npm server registry');
program.parse(process.argv);

const packages = (process.argv[3] || 'abp').split(',').join('|');

const check = (pkgJsonPath) => {
  try {
    return childProcess
      .execSync(
        `ncu "/^@(${packages}).*$/" --packageFile ${pkgJsonPath} -u${
          program.prerelease ? ' --target greatest' : ' --target patch'
        }${program.registry ? ` --registry ${program.registry}` : ''}`
      )
      .toString();
  } catch (error) {
    console.log('exec error: ' + error.message);
    process.exit(error.status);
  }
};

const folder = process.argv[2] || '.';

glob(folder + '/**/package.json', {}, (er, files) => {
  files.forEach((file) => {
    if (
      file.includes('node_modules') ||
      file.includes('ng-packs/dist') ||
      file.includes('wwwroot') ||
      file.includes('bin/Debug')
    ) {
      return;
    }

    console.log(check(file));
  });
});
