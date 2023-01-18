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
import * as ts from 'typescript';

import { join, normalize } from '@angular-devkit/core';
import {
  addImportToModule,
  addRouteDeclarationToModule,
  applyWithOverwrite,
  camel,
  getFirstApplication,
  getWorkspace,
  InsertChange,
  interpolate,
  isLibrary,
  JSONFile,
  kebab,
  pascal,
  readWorkspaceSchema,
  resolveProject,
  updateWorkspace,
} from '../../utils';
import { ProjectDefinition, WorkspaceDefinition } from '../../utils/angular/workspace';
import { addLibToWorkspaceFile } from '../../utils/angular-schematic/generate-lib';
import * as cases from '../../utils/text';
import { Exception } from '../../enums/exception';
import { GenerateLibSchema } from './models/generate-lib-schema';

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
      importConfigModuleToDefaultProjectAppModule(workspace, packageName),
      addRoutingToAppRoutingModule(workspace, packageName),
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
    const jsonPathConfig = ['compilerOptions', 'paths', `${packageName}/config`];
    file.modify(jsonPath, [`${path}/src/public-api.ts`]);
    file.modify(jsonPathConfig, [`${path}/config/src/public-api.ts`]);
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

export function importConfigModuleToDefaultProjectAppModule(
  workspace: WorkspaceDefinition,
  packageName: string,
) {
  return (tree: Tree) => {
    const projectName = readWorkspaceSchema(tree).defaultProject || getFirstApplication(tree).name!;
    const project = workspace.projects.get(projectName);
    const appModulePath = `${project?.sourceRoot}/app/app.module.ts`;
    const appModule = tree.read(appModulePath);
    if (!appModule) {
      return;
    }
    const appModuleContent = appModule.toString();
    if (appModuleContent.includes(`${camel(packageName)}ConfigModule`)) {
      return;
    }

    const forRootStatement = `${pascal(packageName)}ConfigModule.forRoot()`;
    const text = tree.read(appModulePath);
    if (!text) {
      return;
    }
    const sourceText = text.toString();
    if (sourceText.includes(forRootStatement)) {
      return;
    }
    const source = ts.createSourceFile(appModulePath, sourceText, ts.ScriptTarget.Latest, true);

    const changes = addImportToModule(
      source,
      appModulePath,
      forRootStatement,
      `${kebab(packageName)}/config`,
    );
    const recorder = tree.beginUpdate(appModulePath);
    for (const change of changes) {
      if (change instanceof InsertChange) {
        recorder.insertLeft(change.pos, change.toAdd);
      }
    }
    tree.commitUpdate(recorder);

    return;
  };
}

export function addRoutingToAppRoutingModule(workspace: WorkspaceDefinition, packageName: string) {
  return (tree: Tree) => {
    const projectName = readWorkspaceSchema(tree).defaultProject || getFirstApplication(tree).name!;
    const project = workspace.projects.get(projectName);
    const appRoutingModulePath = `${project?.sourceRoot}/app/app-routing.module.ts`;
    const appRoutingModule = tree.read(appRoutingModulePath);
    if (!appRoutingModule) {
      return;
    }
    const appRoutingModuleContent = appRoutingModule.toString();
    const moduleName = `${pascal(packageName)}Module`;
    if (appRoutingModuleContent.includes(moduleName)) {
      return;
    }

    const source = ts.createSourceFile(
      appRoutingModulePath,
      appRoutingModuleContent,
      ts.ScriptTarget.Latest,
      true,
    );
    const importPath = `${kebab(packageName)}`;
    const importStatement = `() => import('${importPath}').then(m => m.${moduleName}.forLazy())`;
    const routeDefinition = `{ path: '${kebab(packageName)}', loadChildren: ${importStatement} }`;
    const change = addRouteDeclarationToModule(source, `${kebab(packageName)}`, routeDefinition);

    const recorder = tree.beginUpdate(appRoutingModulePath);
    if (change instanceof InsertChange) {
      recorder.insertLeft(change.pos, change.toAdd);
    }
    tree.commitUpdate(recorder);

    return;
  };
}

