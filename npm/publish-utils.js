const { execFileSync } = require('child_process');
const { program } = require('commander');
const fse = require('fs-extra');
const semverLt = require('semver/functions/lt');
const semverParse = require('semver/functions/parse');
const semverValid = require('semver/functions/valid');

program.version('0.0.1');
program.option('-n, --nextVersion', 'version in common.props');
program.option('-pr, --prerelease', 'whether version is prerelease');
program.option('-cv, --customVersion <customVersion>', 'set exact version');
program.option(
  '-dt, --distTag <registry>',
  'print a dist-tag when this version is older than the registry latest tag',
);

program.parse(process.argv);

if (program.nextVersion) console.log(getVersion());

if (program.prerelease)
  console.log(!!semverParse(getVersion()).prerelease?.length);

if (program.distTag) console.log(resolvePublishDistTag(program.distTag));

function getVersion() {
  if (program.customVersion) return program.customVersion;
  const commonProps = fse.readFileSync('../common.props').toString();
  const versionTag = '<Version>';
  const versionEndTag = '</Version>';
  const first = commonProps.indexOf(versionTag) + versionTag.length;
  const last = commonProps.indexOf(versionEndTag);
  return commonProps.substring(first, last);
}

// npm refuses to move the "latest" dist-tag to an older version. A patch of a
// previous release line (10.6.2 while 10.7.0 is latest) must use another tag.
function resolvePublishDistTag(registry) {
  const version = getVersion();
  const parsed = semverParse(version);
  if (!parsed || parsed.prerelease.length) {
    return '';
  }

  let latest = '';
  try {
    latest = execFileSync(
      'npm',
      ['view', '@abp/core', 'version', '--registry', registry],
      {
        encoding: 'utf8',
        shell: true,
        stdio: ['ignore', 'pipe', 'ignore'],
      },
    ).trim();
  } catch (e) {
    return '';
  }

  if (!semverValid(latest) || !semverLt(version, latest)) {
    return '';
  }

  return `latest-${parsed.major}.${parsed.minor}`;
}
