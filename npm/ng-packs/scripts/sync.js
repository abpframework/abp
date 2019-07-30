// ESM syntax is supported.
import execa from 'execa';
import fse from 'fs-extra';

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
})();
