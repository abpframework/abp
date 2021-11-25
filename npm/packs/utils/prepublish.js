const fse = require('fs-extra');
const execa = require('execa');

fse.copyFileSync('./package.json', './projects/utils/package.json');
fse.copyFileSync('./README.md', './projects/utils/README.md');

try {
  execa.sync('yarn', ['build'], { stdout: 'inherit' });
  process.exit(0);
} catch (error) {
  console.error(error);
  process.exit(1);
}
