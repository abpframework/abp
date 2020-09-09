const { program } = require('commander');
const fse = require('fs-extra');

program.version('0.0.1');
program.option('-n, --nextVersion', 'version in common.props');
program.option('-r, --rc', 'whether version is rc');

program.parse(process.argv);

if (program.nextVersion) console.log(getVersion());

if (program.rc) console.log(getVersion().includes('rc'));

function getVersion() {
  const commonProps = fse.readFileSync('../common.props').toString();
  const versionTag = '<Version>';
  const versionEndTag = '</Version>';
  const first = commonProps.indexOf(versionTag) + versionTag.length;
  const last = commonProps.indexOf(versionEndTag);
  return commonProps.substring(first, last);
}
