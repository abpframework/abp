const fse = require('fs-extra');

const commonProps = fse.readFileSync('../common.props').toString();

const versionTag = '<Version>';
const versionEndTag = '</Version>';
const first = commonProps.indexOf(versionTag) + versionTag.length;
const last = commonProps.indexOf(versionEndTag);

console.log(commonProps.substring(first, last));
