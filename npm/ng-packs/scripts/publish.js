// ESM syntax is supported.
import execa from 'execa';
import fse from 'fs-extra';

const versions = ['major', 'minor', 'patch', 'premajor', 'preminor', 'prepatch', 'prerelease'];
let nextSemanticVersion = (process.argv[2] || '').toLowerCase();

if (versions.indexOf(nextSemanticVersion) < 0) {
  console.log(
    "Please enter the next semantic version like this: 'npm run publish patch'. Available versions:\n " +
      JSON.stringify(versions),
  );

  process.exit(1);
}

(async () => {
  try {
    await execa('yarn', ['install-new-dependencies'], { stdout: 'inherit' });

    await fse.rename('../lerna.version.json', '../lerna.json');

    await execa(
      'yarn',
      ['lerna', 'version', nextSemanticVersion, '--yes', '--no-commit-hooks', '--skip-git'],
      { stdout: 'inherit', cwd: '../' },
    );

    await fse.rename('../lerna.json', '../lerna.version.json');

    await execa('yarn', ['build', '--noInstall'], { stdout: 'inherit' });

    await fse.rename('../lerna.exec.json', '../lerna.json');

    await execa('yarn', ['lerna', 'exec', '--', 'npm', 'publish'], {
      stdout: 'inherit',
      cwd: '../',
    });

    await fse.rename('../lerna.json', '../lerna.exec.json');
  } catch (error) {
    console.error(error.stderr);
    process.exit(1);
  }

  process.exit(0);
})();
