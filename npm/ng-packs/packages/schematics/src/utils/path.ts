import { dir } from './text';

export function relativePathFromServiceToModel(serviceNamespace: string, modelNamespace: string) {
  const repeats = serviceNamespace ? serviceNamespace.split('.').length : 0;
  const path = '..' + '/..'.repeat(repeats) + '/models/' + dir(modelNamespace);
  return removeTrailingSlash(path);
}

function removeTrailingSlash(path: string) {
  return path.replace(/\/+$/, '');
}
