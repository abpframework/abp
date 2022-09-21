import { Tree } from '@angular-devkit/schematics';
import { resolveProject } from "./workspace";
export async function createTargetLibIfNotExist(tree: Tree, target: string) {
  const lib = await resolveProject(tree, target, null);

  if (!lib) {

  }
}
