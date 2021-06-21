const { program } = require('commander');
const fse = require('fs-extra');

program.version('0.0.1');
program.option('-n, --nextVersion', 'version in common.props');
program.option('-r, --rc', 'whether version is rc');
program.option('-cv, --customVersion <customVersion>', 'set exact version');

program.parse(process.argv);

if (program.nextVersion) console.log(getVersion());

if (program.rc) console.log(getVersion().includes('rc'));

function getVersion() {
  if (program.customVersion) return program.customVersion;
  const commonProps = fse.readFileSync('../common.props').toString();
  const versionTag = '<Version>';
  const versionEndTag = '</Version>';
  const first = commonProps.indexOf(versionTag) + versionTag.length;
  const last = commonProps.indexOf(versionEndTag);
  return commonProps.substring(first, last);
}
