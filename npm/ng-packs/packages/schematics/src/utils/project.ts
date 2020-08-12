import { ProjectDefinition } from '@angular-devkit/core/src/workspace';
import { SchematicsException, Tree } from '@angular-devkit/schematics';
import { Exception } from '../enums';
import { getWorkspace } from './angular/workspace';
import { readWorkspaceSchema } from './workspace';

export async function resolveProject(
  tree: Tree,
  name: string,
): Promise<{ name: string; definition: ProjectDefinition }> {
  const workspace = await getWorkspace(tree);
  let definition = workspace.projects.get(name);

  if (!definition) {
    name = readWorkspaceSchema(tree).defaultProject!;
    definition = workspace.projects.get(name);
  }

  if (!definition) throw new SchematicsException(Exception.NoProject);

  return { name, definition };
}
