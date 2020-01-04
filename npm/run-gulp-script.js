const glob = require('glob');
const execa = require('execa');

const gulp = gulpfilePath => {
  try {
    console.log('Running the yarn command... Cwd: ' + gulpfilePath);
    execa.sync(`yarn`, ['install'], { cwd: gulpfilePath });
    console.log('Running the gulp command...');
    execa.sync(`yarn`, ['gulp'], { cwd: gulpfilePath, stdio: 'inherit' });
  } catch (error) {
    console.log('exec error: ' + error.message);
    process.exit(error.status);
  }
};

const folder = process.argv[2] || '.';

glob(folder + '/**/gulpfile.js', {}, (er, files) => {
  files.forEach(file => {
    if (file.includes('node_modules') || file.includes('wwwroot')) {
      return;
    }

    gulp(file.replace('/gulpfile.js', ''));
  });
});
