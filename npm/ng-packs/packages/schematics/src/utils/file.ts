import { Tree } from '@angular-devkit/schematics';

export function createFileSaver(tree: Tree) {
  return (filePath: string, fileContent: string) =>
    tree.exists(filePath)
      ? tree.overwrite(filePath, fileContent)
      : tree.create(filePath, fileContent);
}
