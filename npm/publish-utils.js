const { program } = require('commander');
const fse = require('fs-extra');
const semverParse = require('semver/functions/parse');

program.version('0.0.1');
program.option('-n, --nextVersion', 'version in common.props');
program.option('-pr, --prerelase', 'whether version is prerelase');
program.option('-cv, --customVersion <customVersion>', 'set exact version');

program.parse(process.argv);

if (program.nextVersion) console.log(getVersion());

if (program.prerelase)
  console.log(!!semverParse(getVersion()).prerelease?.length);

function getVersion() {
  if (program.customVersion) return program.customVersion;
  const commonProps = fse.readFileSync('../common.props').toString();
  const versionTag = '<Version>';
  const versionEndTag = '</Version>';
  const first = commonProps.indexOf(versionTag) + versionTag.length;
  const last = commonProps.indexOf(versionEndTag);
  return commonProps.substring(first, last);
}