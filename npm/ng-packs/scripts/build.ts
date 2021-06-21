import program from 'commander';
import execa from 'execa';
import fse from 'fs-extra';

(async () => {
  program.option('-i, --noInstall', 'skip updating package.json and installation', false);
  program.option('-c, --skipNgcc', 'skip ngcc', false);

  program.parse(process.argv);

  try {
    if (!program.noInstall) {
      await execa('yarn', ['install'], { stdout: 'inherit', cwd: '../' });
    }

    await execa(
      'yarn',
      [
        'symlink',
        'copy',
        '--angular',
        '--prod',
        '--no-watch',
        '--sync',
        '--packages',
        '@abp/ng.core,@abp/ng.theme.shared,@abp/ng.components',
      ],
      { stdout: 'inherit', cwd: '../' },
    );

    await execa(
      'yarn',
      [
        'symlink',
        'copy',
        '--angular',
        '--prod',
        '--no-watch',
        '--packages',
        '@abp/ng.feature-management,@abp/ng.permission-management',
      ],
      { stdout: 'inherit', cwd: '../' },
    );

    await execa(
      'yarn',
      [
        'symlink',
        'copy',
        '--angular',
        '--prod',
        '--no-watch',
        '--all-packages',
        '--excluded-packages',
        '@abp/ng.schematics,@abp/ng.core,@abp/ng.theme.shared,@abp/ng.components,@abp/ng.feature-management,@abp/ng.permission-management',
      ],
      { stdout: 'inherit', cwd: '../' },
    );

    if (!program.skipNgcc) await execa('yarn', ['compile:ivy'], { stdout: 'inherit', cwd: '../' });
  } catch (error) {
    console.error(error.stderr);
    process.exit(1);
  }

  process.exit(0);
})();
