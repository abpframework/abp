import { dir } from './text';

export function relativePathToModel(namespace: string, modelNamespace: string) {
  const repeats = namespace ? namespace.split('.').length : 0;
  const path = '..' + '/..'.repeat(repeats) + '/models/' + dir(modelNamespace);
  return removeTrailingSlash(path);
}

function removeTrailingSlash(path: string) {
  return path.replace(/\/+$/, '');
}
