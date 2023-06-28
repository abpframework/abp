import { getProjects, readJson, readProjectConfiguration, Tree } from '@nx/devkit';

export const IGNORED_PROJECT_NAMES = ['apex-chart-components', 'bs-components', 'workspace-plugin'];

export function getPackageJsonList(tree: Tree, packages: string[]): string[] {
  const project = getProjects(tree);

  const result = ['/package.json'];
  project.forEach((value, key) => {
    
    if (value.projectType !== 'library') {

       return;
    }
    if (IGNORED_PROJECT_NAMES.some(x => x === key)) {
      return;
    }
    const projectConfiguration = readProjectConfiguration(tree, key);

    if (packages.length && !packages.includes(value.name)) {
      return;
    }
    result.push(projectConfiguration.root + '/package.json');
  });
  return result;
}

export function getPackageNameList(tree: Tree, packageJsonList: string[]) {
  return packageJsonList.map(packageJson => {
    const jsonFile = readJson(tree, packageJson);
    return jsonFile.name;
  });
}

const leptonPackages = [
  '@abp/ng.theme.lepton-x',
  '@volosoft/ngx-lepton-x',
  '@volo/abp.ng.lepton-x.core',
  '@volo/ngx-lepton-x.core',
  '@volo/ngx-lepton-x.lite',
  '@volosoft/abp.ng.theme.lepton-x',
];
const abpPackageNameRegex = /^@(abp|volo|volosoft)\/.*/;

export function isAbpPack(packageName) {
  return abpPackageNameRegex.test(packageName) && !leptonPackages.includes(packageName);
}
export function functionisLeptonXPack(packageName) {
  return leptonPackages.includes(packageName);
}

export function getVersionByPackageNameFactory(abpVersionName: string, leptonXVersionName: string) {
  return (packageName: string) => {
    if (isAbpPack(packageName)) {
      return abpVersionName;
    }
    if (functionisLeptonXPack(packageName)) {
      return leptonXVersionName;
    }
    return '';
  };
}

export const semverRegex =
  /\d+\.\d+\.\d+(?:-[a-zA-Z0-9]+(?:\.[a-zA-Z0-9-]+)*)?(?:\+[a-zA-Z0-9]+(?:\.[a-zA-Z0-9-]+)*)?$/;
