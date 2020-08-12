import { experimental } from '@angular-devkit/core';
import { SchematicsException, Tree } from '@angular-devkit/schematics';
import { Exception } from '../enums';

export function readWorkspaceSchema(tree: Tree) {
  const workspaceBuffer = tree.read('/angular.json') || tree.read('/workspace.json');
  if (!workspaceBuffer) throw new SchematicsException(Exception.NoWorkspace);

  let workspaceSchema: experimental.workspace.WorkspaceSchema;

  try {
    workspaceSchema = JSON.parse(workspaceBuffer.toString());
  } catch (_) {
    throw new SchematicsException(Exception.InvalidWorkspace);
  }

  return workspaceSchema;
}
