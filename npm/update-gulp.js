const glob = require('fast-glob');
var path = require('path');
const childProcess = require('child_process');
const execa = require('execa');
const fse = require('fs-extra');
const { program } = require('commander');
const semverLt = require('semver/functions/lt');
const semverValid = require('semver/functions/valid');

program.version('0.0.1');
program.option('-pr, --prerelease', 'whether version is prerelease');
program.option(
  '-rg, --registry <registry>',
  'NPM server registry',
  'https://registry.npmjs.org'
);
program.option(
  '-tv, --targetVersion <targetVersion>',
  'version being published; an older patch line is not moved up to the latest tag'
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

// ncu's default target is the "latest" dist-tag. Publishing 10.6.2 while
// 10.7.0 is already latest would rewrite ~10.6.2 to ~10.7.0. Stay on the
// same minor for that maintenance release. A release that is itself latest
// still follows the latest tag, so a minor bump can move 10.7.x to 10.8.0.
const ncuTargetFlag = resolveNcuTargetFlag();

const updatePackages = (pkgJsonPath) => {
  try {
    const result = childProcess
      .execSync(
        `ncu "/^@abp.*$/" --packageFile ${pkgJsonPath} -u${ncuTargetFlag} --registry ${program.registry}`
      )
      .toString();
    console.log('\x1b[0m', result);
  } catch (error) {
    console.log('\x1b[31m', 'Error: ' + error.message);
  }
};

function resolveNcuTargetFlag() {
  if (program.prerelease) {
    return ' --target newest';
  }

  const version = getReleaseVersion();
  if (!semverValid(version)) {
    return '';
  }

  let latest = '';
  try {
    latest = childProcess
      .execFileSync(
        'npm',
        ['view', '@abp/core', 'version', '--registry', program.registry],
        { encoding: 'utf8', shell: true, stdio: ['ignore', 'pipe', 'ignore'] }
      )
      .trim();
  } catch (e) {
    return '';
  }

  if (semverValid(latest) && semverLt(version, latest)) {
    return ' --target patch';
  }

  return '';
}

function getReleaseVersion() {
  if (program.targetVersion) {
    return program.targetVersion;
  }

  try {
    const commonProps = fse.readFileSync(path.join(__dirname, '../common.props')).toString();
    const versionTag = '<Version>';
    const versionEndTag = '</Version>';
    const first = commonProps.indexOf(versionTag) + versionTag.length;
    const last = commonProps.indexOf(versionEndTag);
    return commonProps.substring(first, last).trim();
  } catch (e) {
    return '';
  }
}

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
