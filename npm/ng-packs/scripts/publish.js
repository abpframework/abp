// ESM syntax is supported.
import execa from 'execa';
import fse from 'fs-extra';

(async () => {
  try {
    await execa('yarn', ['install-new-dependencies'], { stdout: 'inherit' });
  } catch (error) {
    console.error(error.stderr);
    process.exit(1);
  }

  process.exit(0);
})();
