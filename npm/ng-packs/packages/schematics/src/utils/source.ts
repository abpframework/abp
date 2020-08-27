import { SchematicsException, Tree } from '@angular-devkit/schematics';
import got from 'got';
import { Exception } from '../enums';
import { ApiDefinition, Project, ProxyConfig, WriteOp } from '../models';
import { getAssignedPropertyFromObjectliteral } from './ast';
import { interpolate } from './common';
import { readEnvironment } from './workspace';

export async function getApiDefinition(url: string) {
  let body: any;

  try {
    ({ body } = await got(url, {
      responseType: 'json',
      searchParams: { includeTypes: true },
      https: { rejectUnauthorized: false },
    }));
  } catch ({ response }) {
    // handle redirects
    if (response?.body && response.statusCode < 400) return response.body;

    throw new SchematicsException(Exception.NoApi);
  }

  return body;
}

export function getRootNamespace(tree: Tree, project: Project, moduleName: string) {
  const environmentExpr = readEnvironment(tree, project.definition);

  if (!environmentExpr)
    throw new SchematicsException(interpolate(Exception.NoEnvironment, project.name));

  let assignment = getAssignedPropertyFromObjectliteral(environmentExpr, [
    'apis',
    moduleName,
    'rootNamespace',
  ]);

  if (!assignment)
    assignment = getAssignedPropertyFromObjectliteral(environmentExpr, [
      'apis',
      'default',
      'rootNamespace',
    ]);

  if (!assignment)
    throw new SchematicsException(interpolate(Exception.NoRootNamespace, project.name, moduleName));

  return assignment.replace(/[`'"]/g, '');
}

export function getSourceUrl(tree: Tree, project: Project, moduleName: string) {
  const environmentExpr = readEnvironment(tree, project.definition);

  if (!environmentExpr)
    throw new SchematicsException(interpolate(Exception.NoEnvironment, project.name));

  let assignment = getAssignedPropertyFromObjectliteral(environmentExpr, [
    'apis',
    moduleName,
    'url',
  ]);

  if (!assignment)
    assignment = getAssignedPropertyFromObjectliteral(environmentExpr, ['apis', 'default', 'url']);

  if (!assignment)
    throw new SchematicsException(interpolate(Exception.NoApiUrl, project.name, moduleName));

  return assignment.replace(/[`'"]/g, '');
}

export function createProxyConfigReader(targetPath: string) {
  return (tree: Tree) => {
    try {
      const buffer = tree.read(targetPath);
      return JSON.parse(buffer!.toString()) as ProxyConfig;
    } catch (_) {}

    throw new SchematicsException(interpolate(Exception.NoApiDefinition, targetPath));
  };
}

export function createProxyConfigSaver(apiDefinition: ApiDefinition, targetPath: string) {
  const createProxyConfigJson = createProxyConfigJsonCreator(apiDefinition);
  const readPreviousConfig = createProxyConfigReader(targetPath);
  const writeNewConfig = createProxyConfigWriter(targetPath);

  return (tree: Tree) => {
    const generated: string[] = [];
    let op: WriteOp = 'create';

    if (tree.exists(targetPath)) {
      op = 'overwrite';

      try {
        readPreviousConfig(tree).generated.forEach(m => generated.push(m));
      } catch (_) {}
    }

    writeNewConfig(tree, op, createProxyConfigJson(generated));
  };
}

export function createProxyConfigWriter(targetPath: string) {
  return (tree: Tree, op: WriteOp, data: string) => {
    try {
      tree[op](targetPath, data);
      return tree;
    } catch (_) {}

    throw new SchematicsException(interpolate(Exception.FileWriteFailed, targetPath));
  };
}

function createProxyConfigJsonCreator(apiDefinition: ApiDefinition) {
  return (generated: string[]) => JSON.stringify({ generated, ...apiDefinition }, null, 2);
}
