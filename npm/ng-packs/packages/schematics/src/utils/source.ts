import type { workspaces } from '@angular-devkit/core';
import { SchematicsException, Tree } from '@angular-devkit/schematics';
import got from 'got';
import { Exception } from '../enums';
import { getAssignedPropertyFromObjectliteral } from './ast';
import { readEnvironment } from './workspace';

export async function getSourceJson(url: string) {
  let body: any;

  try {
    ({ body } = await got(url, {
      responseType: 'json',
      searchParams: { includeTypes: true },
      https: { rejectUnauthorized: false },
    }));
  } catch (e) {
    throw new SchematicsException(Exception.NoApi);
  }

  return body;
}

export function getSourceUrl(tree: Tree, projectDefinition: workspaces.ProjectDefinition) {
  const environmentExpr = readEnvironment(tree, projectDefinition);
  let assignment: string | undefined;

  if (environmentExpr) {
    assignment = getAssignedPropertyFromObjectliteral(environmentExpr, ['apis', 'default', 'url']);
  }

  if (!assignment) throw new SchematicsException(Exception.RequiredApiUrl);

  return assignment.replace(/[`'"]/g, '');
}
