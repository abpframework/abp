import { strings } from '@angular-devkit/core';
import { Tree } from '@angular-devkit/schematics';
import { PROXY_PATH } from '../constants';
import { createFileSaver } from './file';

export function createProxyIndexGenerator(targetPath: string) {
  return createBarrelsGenerator(targetPath + PROXY_PATH);
}

export function createBarrelsGenerator(rootPath: string) {
  return (tree: Tree) => {
    generateBarrelFromPath(tree, rootPath);
    return tree;
  };
}

export function generateBarrelFromPath(tree: Tree, indexPath: string) {
  const saveFile = createFileSaver(tree);
  const dir = tree.getDir(indexPath);

  const _exports: string[] = [];

  dir.subfiles.forEach(fragment => {
    if (!fragment.endsWith('.ts') || fragment === 'index.ts') return;

    _exports.push(`export * from './${fragment.replace(/\.ts$/, '')}';`);
  });

  dir.subdirs.forEach(fragment => {
    const subDirPath = indexPath + '/' + fragment;
    const subDir = tree.getDir(subDirPath);
    let hasFiles = false;
    subDir.visit(() => (hasFiles = true));
    if (!hasFiles) return;

    _exports.push(`export * as ${strings.classify(fragment)} from './${fragment}';`);
    generateBarrelFromPath(tree, subDirPath);
  });

  _exports.sort();

  if (_exports.length)
    saveFile(
      indexPath + '/index.ts',
      _exports.join(`
`) +
        `
`,
    );
}
