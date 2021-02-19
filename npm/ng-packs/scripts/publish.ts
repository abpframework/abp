import execa from 'execa';
import fse from 'fs-extra';
import program from 'commander';
import replaceWithPreview from './replace-with-preview';

program
  .option(
    '-v, --nextVersion <version>',
    'next semantic version. Available versions: ["major", "minor", "patch", "premajor", "preminor", "prepatch", "prerelease", "or type a custom version"]',
  )
  .option('-r, --registry <registry>', 'target npm server registry')
  .option('-p, --preview', 'publishes with preview tag')
  .option('-r, --rc', 'publishes with next tag')
  .option('-g, --skipGit', 'skips git push');

program.parse(process.argv);

(async () => {
  const versions = ['major', 'minor', 'patch', 'premajor', 'preminor', 'prepatch', 'prerelease'];

  if (!program.nextVersion) {
    console.error('Please provide a version with --nextVersion attribute');
    process.exit(1);
  }

  const registry = program.registry || 'https://registry.npmjs.org';

  try {
    await fse.remove('../dist');

    await execa('yarn', ['install', '--ignore-scripts'], { stdout: 'inherit', cwd: '../' });

    await fse.rename('../lerna.version.json', '../lerna.json');

    await execa(
      'yarn',
      [
        'lerna',
        'version',
        program.nextVersion,
        '--yes',
        '--no-commit-hooks',
        '--skip-git',
        '--force-publish',
      ],
      { stdout: 'inherit', cwd: '../' },
    );

    await fse.rename('../lerna.json', '../lerna.version.json');

    await execa('yarn', ['replace-with-tilde']);

    if (program.preview) await replaceWithPreview(program.nextVersion);

    await execa('yarn', ['build', '--noInstall', '--skipNgcc'], { stdout: 'inherit' });

    await execa('yarn', ['build:schematics'], { stdout: 'inherit' });

    await fse.rename('../lerna.publish.json', '../lerna.json');

    let tag: string;
    if (program.preview) tag = 'preview';
    if (program.rc) tag = 'next';

    await execa(
      'yarn',
      ['lerna', 'exec', '--', `"npm publish --registry ${registry}${tag ? ` --tag ${tag}` : ''}"`],
      {
        stdout: 'inherit',
        cwd: '../',
      },
    );

    await fse.rename('../lerna.json', '../lerna.publish.json');

    if (!program.preview && !program.skipGit) {
      await execa('git', ['add', '../packages/*', '../package.json', '../lerna.version.json'], {
        stdout: 'inherit',
      });
      await execa('git', ['commit', '-m', 'Upgrade ng package versions', '--no-verify'], {
        stdout: 'inherit',
      });
    }
  } catch (error) {
    console.error(error.stderr);
    process.exit(1);
  }

  process.exit(0);
})();
