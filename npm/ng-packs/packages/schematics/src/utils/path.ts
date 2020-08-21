import { dir, kebab } from './text';

export function relativePathToEnum(namespace: string, enumNamespace: string, enumName: string) {
  const repeats = namespace ? namespace.split('.').length : 0;
  const path = '..' + '/..'.repeat(repeats) + '/enums/' + dir(enumNamespace);
  return removeDoubleSlash(path + '/' + kebab(enumName));
}

export function relativePathToModel(namespace: string, modelNamespace: string) {
  const repeats = namespace ? namespace.split('.').length : 0;
  const path = '..' + '/..'.repeat(repeats) + '/models/' + dir(modelNamespace);
  return removeTrailingSlash(path);
}

function removeDoubleSlash(path: string) {
  return path.replace(/\/{2,}/g, '/');
}

function removeTrailingSlash(path: string) {
  return path.replace(/\/+$/, '');
}
