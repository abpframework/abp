import { Rule, Tree } from '@angular-devkit/schematics';
import { Builders, JSONFile, ProjectType, updateWorkspace } from '../angular';

export function updateTsConfig(packageName: string, ...paths: string[]) {
  return (host: Tree) => {
    if (!host.exists('tsconfig.json')) {
      return host;
    }

    const file = new JSONFile(host, 'tsconfig.json');
    const jsonPath = ['compilerOptions', 'paths', packageName];
    const value = file.get(jsonPath);
    file.modify(jsonPath, Array.isArray(value) ? [...value, ...paths] : paths);
  };
}

export function addLibToWorkspaceFile(projectRoot: string, projectName: string): Rule {
  return updateWorkspace(workspace => {
    workspace.projects.add({
      name: projectName,
      root: projectRoot,
      sourceRoot: `${projectRoot}/src`,
      projectType: ProjectType.Library,
      prefix: 'lib',
      targets: {
        build: {
          builder: Builders.NgPackagr,
          defaultConfiguration: 'production',
          options: {
            project: `${projectRoot}/ng-package.json`,
          },
          configurations: {
            production: {
              tsConfig: `${projectRoot}/tsconfig.lib.prod.json`,
            },
            development: {
              tsConfig: `${projectRoot}/tsconfig.lib.json`,
            },
          },
        },
        test: {
          builder: Builders.Karma,
          options: {
            main: `${projectRoot}/src/test.ts`,
            tsConfig: `${projectRoot}/tsconfig.spec.json`,
            karmaConfig: `${projectRoot}/karma.conf.js`,
          },
        },
      },
    });
  });
}
