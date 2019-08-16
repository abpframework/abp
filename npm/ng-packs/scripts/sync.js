// ESM syntax is supported.
import fse from 'fs-extra';
import execa from 'execa';

(async () => {
  const { projects } = await fse.readJSON('../angular.json');
  const projectNames = Object.keys(projects);

  projectNames.forEach(async project => {
    const { dependencies: distDependencies, version } = await fse.readJSON(`../dist/${project}/package.json`);
    const srcPackagePath = `../packages/${project}/package.json`;
    const srcPackage = await fse.readJSON(srcPackagePath);

    if (distDependencies) {
      for (const key in srcPackage.dependencies) {
        if (distDependencies.hasOwnProperty(key)) {
          srcPackage.dependencies[key] = distDependencies[key];
        }
      }
    }

    await fse.writeJSON(srcPackagePath, { ...srcPackage, version }, { spaces: 2 });
  });

  await execa('git', ['add', '../packages/*', '../package.json'], { stdout: 'inherit' });
  await execa('git', ['commit', '-m', 'Update source packages versions'], { stdout: 'inherit' });

  process.exit(0);
})();
