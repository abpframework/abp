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

  const asterisk = collectAsteriskBarrel(tree, indexPath);
  const named = collectNamedBarrel(tree, indexPath);

  if (asterisk.exports.length + named.exports.length)
    saveFile(indexPath + '/index.ts', generateBarrelContent(asterisk, named));
}

function generateBarrelContent(asterisk: AsteriskBarrel, named: NamedBarrel): string {
  const namedImports = !named.imports.length
    ? ''
    : named.imports.join(`
`) +
      `
`;

  const namedExports = !named.exports.length
    ? ''
    : `export { ${named.exports.join(', ')} };
`;

  const asteriskExports = !asterisk.exports.length
    ? ''
    : asterisk.exports.join(`
`) +
      `
`;

  return namedImports + asteriskExports + namedExports;
}

function collectNamedBarrel(tree: Tree, indexPath: string) {
  const dir = tree.getDir(indexPath);
  const barrel = new NamedBarrel();

  dir.subdirs.forEach(fragment => {
    const subDirPath = indexPath + '/' + fragment;
    const subDir = tree.getDir(subDirPath);
    let hasFiles = false;
    subDir.visit(() => (hasFiles = true));
    if (!hasFiles) return;

    const namespaceFragment = strings.classify(fragment);
    barrel.imports.push(`import * as ${namespaceFragment} from './${fragment}';`);
    barrel.exports.push(namespaceFragment);
    generateBarrelFromPath(tree, subDirPath);
  });

  barrel.imports.sort();
  barrel.exports.sort();

  return barrel;
}

function collectAsteriskBarrel(tree: Tree, indexPath: string) {
  const dir = tree.getDir(indexPath);
  const barrel = new AsteriskBarrel();

  dir.subfiles.forEach(fragment => {
    if (!fragment.endsWith('.ts') || fragment === 'index.ts') return;

    barrel.exports.push(`export * from './${fragment.replace(/\.ts$/, '')}';`);
  });

  barrel.exports.sort();

  return barrel;
}

abstract class Barrel {
  imports: string[] = [];
  exports: string[] = [];
}

class AsteriskBarrel extends Barrel {
  type = 'Asterisk' as const;
}

class NamedBarrel extends Barrel {
  type = 'Named' as const;
}
