import fse from 'fs-extra';
import execa from 'execa';

(async () => {
  const { projects } = await fse.readJSON('../angular.json');
  const projectNames = Object.keys(projects).filter(project => project !== 'dev-app');

  for (let i = 0; i < projectNames.length; i++) {
    const project = projectNames[i];
    const { dependencies: distDependencies, version } = await fse.readJson(
      `../dist/${project}/package.json`,
    );
    const srcPackagePath = `../packages/${project}/package.json`;
    const srcPackage = await fse.readJson(srcPackagePath);

    if (distDependencies) {
      for (const key in srcPackage.dependencies) {
        if (distDependencies.hasOwnProperty(key)) {
          srcPackage.dependencies[key] = distDependencies[key];
        }
      }
    }

    await fse.writeJson(srcPackagePath, { ...srcPackage, version }, { spaces: 2 });
  }

  try {
    await execa('git', ['add', '../packages/*', '../package.json'], { stdout: 'inherit' });
    await execa('git', ['commit', '-m', 'Update source packages versions', '--no-verify'], {
      stdout: 'inherit',
    });
  } catch (error) {
    console.error(error.stderr);
  }

  process.exit(0);
})();
