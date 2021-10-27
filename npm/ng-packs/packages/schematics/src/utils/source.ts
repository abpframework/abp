/* eslint-disable no-empty */
import { SchematicsException, Tree } from '@angular-devkit/schematics';
import got from 'got';
import {
  API_DEFINITION_ENDPOINT,
  PROXY_CONFIG_PATH,
  PROXY_PATH,
  PROXY_WARNING,
  PROXY_WARNING_PATH,
} from '../constants';
import { Exception } from '../enums';
import { ApiDefinition, GenerateProxySchema, Project, ProxyConfig, WriteOp } from '../models';
import { getAssignedPropertyFromObjectliteral } from './ast';
import { interpolate } from './common';
import { readEnvironment, resolveProject } from './workspace';

export function createApiDefinitionGetter(params: GenerateProxySchema) {
  const apiName = params.apiName || 'default';

  return async (host: Tree) => {
    const source = await resolveProject(host, params.source!);
    const sourceUrl = getSourceUrl(host, source, apiName);
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
    if (!response?.body || response.statusCode >= 400)
      throw new SchematicsException(interpolate(Exception.NoApi, url));

    body = response.body;
  }

  return body;
}

export function createRootNamespaceGetter(params: GenerateProxySchema) {
  const apiName = params.apiName || 'default';

  return async (tree: Tree) => {
    const project = await resolveProject(tree, params.source!);
    const environmentExpr = readEnvironment(tree, project.definition);

    if (!environmentExpr)
      throw new SchematicsException(interpolate(Exception.NoEnvironment, project.name));

    let assignment = getAssignedPropertyFromObjectliteral(environmentExpr, [
      'apis',
      apiName,
      'rootNamespace',
    ]);

    if (!assignment)
      assignment = getAssignedPropertyFromObjectliteral(environmentExpr, [
        'apis',
        'default',
        'rootNamespace',
      ]);

    if (!assignment)
      throw new SchematicsException(interpolate(Exception.NoRootNamespace, project.name, apiName));

    return assignment.replace(/[`'"]/g, '');
  };
}

export function getSourceUrl(tree: Tree, project: Project, apiName: string) {
  const environmentExpr = readEnvironment(tree, project.definition);

  if (!environmentExpr)
    throw new SchematicsException(interpolate(Exception.NoEnvironment, project.name));

  let assignment = getAssignedPropertyFromObjectliteral(environmentExpr, ['apis', apiName, 'url']);

  if (!assignment)
    assignment = getAssignedPropertyFromObjectliteral(environmentExpr, ['apis', 'default', 'url']);

  if (!assignment)
    throw new SchematicsException(interpolate(Exception.NoApiUrl, project.name, apiName));

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
  const proxyIndexPath = `${targetPath}/index.ts`;

  return (tree: Tree) => {
    try {
      tree.getDir(targetPath).subdirs.forEach(dirName => {
        const dirPath = `${targetPath}/${dirName}`;
        tree.getDir(dirPath).visit(filePath => tree.delete(filePath));
        tree.delete(dirPath);
      });

      if (tree.exists(proxyIndexPath)) tree.delete(proxyIndexPath);

      return tree;
    } catch (_) {
      throw new SchematicsException(interpolate(Exception.DirRemoveFailed, targetPath));
    }
  };
}

export function createProxyWarningSaver(targetPath: string) {
  targetPath += PROXY_WARNING_PATH;
  const createFileWriter = createFileWriterCreator(targetPath);

  return (tree: Tree) => {
    const op = tree.exists(targetPath) ? 'overwrite' : 'create';
    const writeWarningMD = createFileWriter(op, PROXY_WARNING);
    writeWarningMD(tree);

    return tree;
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

  return createFileWriterCreator(targetPath);
}

export function createFileWriterCreator(targetPath: string) {
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
