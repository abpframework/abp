import { strings } from '@angular-devkit/core';
import { kebab } from './text';

export function relativePathToEnum(namespace: string, enumNamespace: string, enumName: string) {
  const path = calculateRelativePath(namespace, enumNamespace);
  return path + `/${kebab(enumName)}.enum`;
}

export function relativePathToModel(namespace: string, modelNamespace: string) {
  const path = calculateRelativePath(namespace, modelNamespace);
  return path + '/models';
}

function calculateRelativePath(ns1: string, ns2: string) {
  if (ns1 === ns2) return '.';

  const parts1 = ns1 ? ns1.split('.') : [];
  const parts2 = ns2 ? ns2.split('.') : [];

  while (parts1.length && parts2.length) {
    if (parts1[0] !== parts2[0]) break;

    parts1.shift();
    parts2.shift();
  }

  const up = '../'.repeat(parts1.length) || '.';
  const down = parts2.reduce((acc, p) => acc + '/' + strings.dasherize(p), '');

  return removeTrailingSlash(removeDoubleSlash(up + down));
}

function removeDoubleSlash(path: string) {
  return path.replace(/\/{2,}/g, '/');
}

function removeTrailingSlash(path: string) {
  return path.replace(/\/+$/, '');
}
