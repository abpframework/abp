// ESM syntax is supported.
import execa from 'execa';
import fse from 'fs-extra';
import program from 'commander';

(async () => {
  program.option('-c, --noCommit', 'skip commit process', false);
  program.option('-i, --noInstall', 'skip updating package.json and installation', false);

  program.parse(process.argv);

  try {
    if (!program.noInstall) {
      await execa('yarn', ['install-new-dependencies'], { stdout: 'inherit' });
    }

    execa.sync(
      'yarn',
      [
        'symlink',
        'copy',
        '--angular',
        '--no-watch',
        '--sync',
        '--packages',
        '@abp/ng.core,@abp/ng.theme.shared',
      ],
      { stdout: 'inherit', cwd: '../' },
    );

    await execa(
      'yarn',
      [
        'symlink',
        'copy',
        '--angular',
        '--no-watch',
        '--packages',
        '@abp/ng.feature-management,@abp/ng.permission-management,@abp/ng.account.config,@abp/ng.identity.config,@abp/ng.setting-management.config,@abp/ng.tenant-management.config',
      ],
      { stdout: 'inherit', cwd: '../' },
    );

    await execa(
      'yarn',
      [
        'symlink',
        'copy',
        '--angular',
        '--no-watch',
        '--all-packages',
        '--excluded-packages',
        '@abp/ng.core,@abp/ng.theme.shared,@abp/ng.feature-management,@abp/ng.permission-management,@abp/ng.account.config,@abp/ng.identity.config,@abp/ng.setting-management.config,@abp/ng.tenant-management.config',
      ],
      { stdout: 'inherit', cwd: '../' },
    );
  } catch (error) {
    console.error(error.stderr);
    process.exit(1);
  }

  if (!program.noCommit) {
    await execa('git', ['add', '../dist/*', '../package.json'], { stdout: 'inherit' });
    await execa('git', ['commit', '-m', 'Build ng packages', '--no-verify'], { stdout: 'inherit' });
  }

  process.exit(0);
})();
