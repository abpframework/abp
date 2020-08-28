import { strings } from '@angular-devkit/core';
import { SchematicsException, Tree } from '@angular-devkit/schematics';
import got from 'got';
import { API_DEFINITION_ENDPOINT, PROXY_CONFIG_PATH, PROXY_PATH } from '../constants';
import { Exception } from '../enums';
import { ApiDefinition, GenerateProxySchema, Project, ProxyConfig, WriteOp } from '../models';
import { getAssignedPropertyFromObjectliteral } from './ast';
import { interpolate } from './common';
import { readEnvironment, resolveProject } from './workspace';

export function createApiDefinitionGetter(params: GenerateProxySchema) {
  const moduleName = strings.camelize(params.module || 'app');

  return async (host: Tree) => {
    const source = await resolveProject(host, params.source!);
    const sourceUrl = getSourceUrl(host, source, moduleName);
    return await getApiDefinition(sourceUrl);
  };
}

async function getApiDefinition(sourceUrl: string) {
  const url = sourceUrl + API_DEFINITION_ENDPOINT;
  let body: ApiDefinition;

  try {
    ({ body } = await got(url, {
      responseType: 'json',
      searchParams: { includeTypes: true },
      https: { rejectUnauthorized: false },
    }));
  } catch ({ response }) {
    // handle redirects
    if (response.statusCode >= 400 || !response?.body)
      throw new SchematicsException(Exception.NoApi);

    body = response.body;
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
  targetPath += PROXY_CONFIG_PATH;

  return (tree: Tree) => {
    try {
      const buffer = tree.read(targetPath);
      return JSON.parse(buffer!.toString()) as ProxyConfig;
    } catch (_) {}

    throw new SchematicsException(interpolate(Exception.NoProxyConfig, targetPath));
  };
}

export function createProxyClearer(targetPath: string) {
  targetPath += PROXY_PATH;

  return (tree: Tree) => {
    try {
      tree.getDir(targetPath).subdirs.forEach(dirName => {
        if (!['enums', 'models', 'services'].includes(dirName)) return;

        tree.delete(`${targetPath}/${dirName}`);
      });

      return tree;
    } catch (_) {
      throw new SchematicsException(interpolate(Exception.DirRemoveFailed, targetPath));
    }
  };
}

export function createProxyConfigSaver(apiDefinition: ApiDefinition, targetPath: string) {
  const createProxyConfigJson = createProxyConfigJsonCreator(apiDefinition);
  const readPreviousConfig = createProxyConfigReader(targetPath);
  const createProxyConfigWriter = createProxyConfigWriterCreator(targetPath);
  targetPath += PROXY_CONFIG_PATH;

  return (tree: Tree) => {
    const generated: string[] = [];
    let op: WriteOp = 'create';

    if (tree.exists(targetPath)) {
      op = 'overwrite';

      try {
        readPreviousConfig(tree).generated.forEach(m => generated.push(m));
      } catch (_) {}
    }

    const json = createProxyConfigJson(generated);
    const writeProxyConfig = createProxyConfigWriter(op, json);
    writeProxyConfig(tree);

    return tree;
  };
}

export function createProxyConfigWriterCreator(targetPath: string) {
  targetPath += PROXY_CONFIG_PATH;

  return (op: WriteOp, data: string) => (tree: Tree) => {
    try {
      tree[op](targetPath, data);
      return tree;
    } catch (_) {}

    throw new SchematicsException(interpolate(Exception.FileWriteFailed, targetPath));
  };
}

export function createProxyConfigJsonCreator(apiDefinition: ApiDefinition) {
  return (generated: string[]) => generateProxyConfigJson({ generated, ...apiDefinition });
}

export function generateProxyConfigJson(proxyConfig: ProxyConfig) {
  return JSON.stringify(proxyConfig, null, 2);
}
