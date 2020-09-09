import execa from 'execa';
import fse from 'fs-extra';

(async () => {
  await execa('yarn', ['ng', 'build', '--prod'], {
    stdout: 'inherit',
    cwd: '..',
  });

  await execa('yarn', ['install', '--ignore-scripts'], {
    stdout: 'inherit',
    cwd: '../../../templates/app/angular',
  });

  await fse.remove('../../../templates/app/angular/node_modules/@abp');
  await fse.copy('../node_modules/@abp', '../../../templates/app/angular/node_modules/@abp', {
    overwrite: true,
  });

  // TODO: Will be removed in v3.1, it is added to fix the prod build error
  await fse.copy(
    '../node_modules/@swimlane',
    '../../../templates/app/angular/node_modules/@swimlane',
    {
      overwrite: true,
    },
  );

  await execa('yarn', ['ng', 'build', '--prod'], {
    stdout: 'inherit',
    cwd: '../../../templates/app/angular',
  });
})();
