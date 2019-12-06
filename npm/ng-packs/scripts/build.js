// ESM syntax is supported.
import execa from 'execa';
import fse from 'fs-extra';
import program from 'commander';

(async () => {
  const { projects } = await fse.readJSON('../angular.json');
  const projectNames = Object.keys(projects).filter(project => project !== 'dev-app');

  const packageJson = await fse.readJSON('../package.json');

  program.option('-c, --noCommit', 'skip commit process', false);

  program.parse(process.argv);

  let npmPackageNames = [];
  projectNames.forEach(project => {
    // do not convert to async
    const { name, dependencies = {}, peerDependencies = {} } = fse.readJSONSync(
      `../packages/${project}/package.json`,
    );
    npmPackageNames.push(name);

    packageJson.devDependencies = {
      ...packageJson.devDependencies,
      ...dependencies,
      ...peerDependencies,
    };
  });

  await fse.writeJSON('../package.json', packageJson, { spaces: 2 });

  try {
    await execa('yarn', ['install', '--ignore-scripts'], {
      stdout: 'inherit',
      cwd: '../',
    });

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
