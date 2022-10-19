import {
  applyTemplates,
  chain,
  move,
  noop,
  Rule,
  SchematicsException,
  Tree,
  url,
} from '@angular-devkit/schematics';
import { GenerateLibSchema } from './models/generate-lib-schema';
import {
  applyWithOverwrite,
  getWorkspace,
  interpolate,
  isLibrary,
  JSONFile,
  kebab,
  resolveProject,
  updateWorkspace,
} from '../../utils';
import * as cases from '../../utils/text';
import { Exception } from '../../enums';
import { join, normalize } from '@angular-devkit/core';
import {
  ProjectDefinition,
  WorkspaceDefinition,
} from '@angular-devkit/core/src/workspace/definitions';
import { addLibToWorkspaceFile } from '../../utils/angular-schematic/generate-lib';

export default function (schema: GenerateLibSchema) {
  return async (tree: Tree) => {
    if (schema.override || !(await checkLibExist(schema, tree))) {
      return chain([createLibrary(schema)]);
    }
  };
}

async function checkLibExist(options: GenerateLibSchema, tree: Tree) {
  const packageName = kebab(options.packageName);
  if (options.isSecondaryEntrypoint) {
    const lib = await resolveProject(tree, options.target);
    const ngPackagePath = `${lib?.definition.root}/${packageName}/ng-package.json`;
    const packageInfo = tree.read(ngPackagePath);
    if (packageInfo) {
      throw new SchematicsException(
        interpolate(Exception.LibraryAlreadyExists, `${lib.name}/${packageName}`),
      );
    }
    return false;
  }

  const target = await resolveProject(tree, options.packageName, null);
  if (target) {
    throw new SchematicsException(interpolate(Exception.LibraryAlreadyExists, packageName));
  }
  return false;
}

function createLibrary(options: GenerateLibSchema): Rule {
  return async (tree: Tree) => {
    const target = await resolveProject(tree, options.packageName, null);
    if (!target || options.override) {
      if (options.isModuleTemplate) {
        return createLibFromModuleTemplate(tree, options);
      }
      if (options.isSecondaryEntrypoint) {
        return createLibSecondaryEntry(tree, options);
      }
    } else {
      throw new SchematicsException(
        interpolate(Exception.LibraryAlreadyExists, options.packageName),
      );
    }
  };
}
async function resolvePackagesDirFromAngularJson(host: Tree) {
  const workspace = await getWorkspace(host);
  const projectFolder = readFirstLibInAngularJson(workspace);
  return projectFolder?.root?.split('/')?.[0] || 'projects';
}

function readFirstLibInAngularJson(workspace: WorkspaceDefinition): ProjectDefinition | undefined {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  return Array.from(workspace.projects.values()).find(value => isLibrary(value));
}

async function createLibFromModuleTemplate(tree: Tree, options: GenerateLibSchema) {
  const packagesDir = await resolvePackagesDirFromAngularJson(tree);
  const packageJson = JSON.parse(tree.read('./package.json')!.toString());
  const abpVersion = packageJson.dependencies['@abp/ng.core'];

  return chain([
    applyWithOverwrite(url('./files-package'), [
      applyTemplates({
        ...cases,
        libraryName: options.packageName,
        abpVersion,
      }),
      move(normalize(packagesDir)),
    ]),
    addLibToWorkspaceIfNotExist(options.packageName, packagesDir),
  ]);
}

export function addLibToWorkspaceIfNotExist(name: string, packagesDir: string): Rule {
  return async (tree: Tree) => {
    const workspace = await getWorkspace(tree);
    const packageName = kebab(name);
    const isProjectExist = workspace.projects.has(packageName);

    const projectRoot = join(normalize(packagesDir), packageName);
    const pathImportLib = `${packagesDir}/${packageName}`;

    return chain([
      isProjectExist
        ? updateWorkspace(w => {
            w.projects.delete(packageName);
          })
        : noop(),
      addLibToWorkspaceFile(projectRoot, packageName),
      updateTsConfig(packageName, pathImportLib),
    ]);
  };
}

export function updateTsConfig(packageName: string, path: string) {
  return (host: Tree) => {
    const files = ['tsconfig.json', 'tsconfig.app.json', 'tsconfig.base.json'];
    const tsConfig = files.find(f => host.exists(f));
    if (!tsConfig) {
      return host;
    }

    const file = new JSONFile(host, tsConfig);
    const jsonPath = ['compilerOptions', 'paths', packageName];
    file.modify(jsonPath, [`${path}/src/public-api.ts`]);
  };
}

export async function createLibSecondaryEntry(tree: Tree, options: GenerateLibSchema) {
  const targetLib = await resolveProject(tree, options.target);
  const packageName = `${kebab(targetLib.name)}/${kebab(options.packageName)}`;
  const importPath = `${targetLib.definition.root}/${kebab(options.packageName)}`;
  return chain([
    applyWithOverwrite(url('./files-secondary-entrypoint'), [
      applyTemplates({
        ...cases,
        libraryName: options.packageName,
        target: targetLib.name,
      }),
      move(normalize(targetLib.definition.root)),
      updateTsConfig(packageName, importPath),
    ]),
  ]);
}
