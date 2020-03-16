import execa from 'execa';
import program from 'commander';

(async () => {
  program.option('-i, --noInstall', 'skip updating package.json and installation', false);

  program.parse(process.argv);

  try {
    if (!program.noInstall) {
      await execa('yarn', ['install-new-dependencies'], { stdout: 'inherit' });
    }

    await execa(
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

  process.exit(0);
})();
