/* eslint-disable no-empty */
import { strings, workspaces } from '@angular-devkit/core';
import { SchematicsException, Tree } from '@angular-devkit/schematics';
import { Exception } from '../enums';
import { Project } from '../models';
import { buildDefaultPath, getWorkspace, ProjectType, WorkspaceSchema } from './angular';
import { findEnvironmentExpression } from './ast';
import { readFileInTree } from './common';
import { NOT_FOUND_VALUE } from '../constants/symbols';
import { parseJson } from '@angular/cli/src/utilities/json-file';

export function isLibrary(project: workspaces.ProjectDefinition): boolean {
  return project.extensions['projectType'] === ProjectType.Library;
}

export function readEnvironment(tree: Tree, project: workspaces.ProjectDefinition) {
  if (isLibrary(project)) return undefined;

  const srcPath = project.sourceRoot || `${project.root}/src`;
  const envPath = srcPath + '/environments/environment.ts';
  const source = readFileInTree(tree, envPath);
  return findEnvironmentExpression(source);
}
export function getFirstApplication(tree: Tree) {
  const workspace = readWorkspaceSchema(tree);
  const [name, project] =
    Object.entries(workspace.projects).find(
      ([_, project]) => project.projectType === ProjectType.Application,
    ) || [];
  return { name, project };
}

export function readWorkspaceSchema(tree: Tree) {
  if (!tree.exists('/angular.json')) throw new SchematicsException(Exception.NoWorkspace);

  let workspaceSchema: WorkspaceSchema;

  try {
    workspaceSchema = getWorkspaceSchema(tree);
  } catch (_) {
    throw new SchematicsException(Exception.InvalidWorkspace);
  }

  return workspaceSchema;
}
// eslint-disable-next-line
// @typescript-eslint/no-explicit-any
export async function resolveProject<T = any>(
  tree: Tree,
  name: string,
  // eslint-disable-next-line
  // @typescript-eslint/no-explicit-any
  notFoundValue: T = NOT_FOUND_VALUE as unknown as any,
): Promise<Project | T> {
  name = name || readWorkspaceSchema(tree).defaultProject || getFirstApplication(tree).name!;
  const workspace = await getWorkspace(tree);
  let definition: Project['definition'] | undefined;

  try {
    definition = workspace.projects.get(name);
  } catch (_) {}

  if (!definition)
    try {
      name = strings.dasherize(name);
      definition = workspace.projects.get(name);
    } catch (_) {}

  if (!definition)
    try {
      name = strings.camelize(name);
      definition = workspace.projects.get(name);
    } catch (_) {}

  if (!definition)
    try {
      name = strings.classify(name);
      definition = workspace.projects.get(name);
    } catch (_) {}

  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  // @ts-ignore
  if (!definition && notFoundValue !== NOT_FOUND_VALUE) {
    return notFoundValue;
  }
  if (!definition) throw new SchematicsException(Exception.NoProject);

  return { name, definition };
}

export function getWorkspaceSchema(host: Tree): WorkspaceSchema {
  const path = getWorkspacePath(host);
  const configBuffer = host.read(path);
  if (configBuffer === null) {
    throw new SchematicsException(`Could not find (${path})`);
  }
  const content = configBuffer.toString();
  // eslint-disable-next-line
  // @typescript-eslint/no-explicit-any
  return parseJson(content) as Record<string, any> as WorkspaceSchema;
}

export function getWorkspacePath(host: Tree): string {
  const possibleFiles = ['/angular.json', '/.angular.json'];
  const path = possibleFiles.filter(path => host.exists(path))[0];

  return path;
}

/**
 * Build a default project path for generating.
 * @param project The project which will have its default path generated.
 * @param entryPoint The secondary-entry-point.
 */
export function buildTargetPath(
  project: workspaces.ProjectDefinition,
  entryPoint?: string,
): string {
  if (entryPoint) {
    return `${project.root}/${entryPoint}/src`;
  }
  return buildDefaultPath(project);
}
