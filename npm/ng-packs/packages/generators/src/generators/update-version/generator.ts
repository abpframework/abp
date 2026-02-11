import { Tree, updateJson } from '@nx/devkit';
import { UpdateVersionGeneratorSchema } from './schema';
import { getPackageJsonList, getVersionByPackageNameFactory, semverRegex } from './utils';

export function updateVersionGenerator(tree: Tree, schema: UpdateVersionGeneratorSchema) {
  const packageJsonList = getPackageJsonList(tree, schema.packages || []);
  const getVersionNumberByPackageName = getVersionByPackageNameFactory(
    schema.abpVersion,
    schema.leptonXVersion,
  );
  packageJsonList.forEach(path => {
    updateJson(tree, path, pkgJson => {
      pkgJson.version = getVersionNumberByPackageName(pkgJson.name) || pkgJson.version;
      console.log('\x1b[32m', `Updated ${pkgJson.name} version to ${pkgJson.version}`);

      Object.keys(pkgJson.dependencies || {}).forEach(key => {
        const v = getVersionNumberByPackageName(key);
        if (!v) {
          return;
        }
        pkgJson.dependencies[key] = pkgJson.dependencies[key].replace(semverRegex, v);
        console.log('\x1b[32m', `Updated   ${key} version to ${v} in dependencies`);
      });

      Object.keys(pkgJson.peerDependencies || {}).forEach(key => {
        const v = getVersionNumberByPackageName(key);
        if (!v) {
          return;
        }
        pkgJson.peerDependencies[key] = pkgJson.peerDependencies[key].replace(semverRegex, v);
        console.log('\x1b[32m', `Updated ${key} version to ${schema.abpVersion} in peerDependencies`);
      });

      Object.keys(pkgJson.devDependencies || {}).forEach(key => {
        const v = getVersionNumberByPackageName(key);
        if (!v) {
          return;
        }
        pkgJson.devDependencies[key] = pkgJson.devDependencies[key].replace(semverRegex, v);
        console.log('\x1b[32m', `Updated ${key} version to ${schema.abpVersion} on devDependencies`);
      });

      return pkgJson;
    });
  });
  // eslint-disable-next-line @typescript-eslint/no-empty-function
  return () => {};
}

export default updateVersionGenerator;
