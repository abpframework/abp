import { Model } from '../models';
import { parseNamespace } from './namespace';

export function createImportRefsToModelMapper(solution: string) {
  return (importRefs: string[]) => {
    return new Model({
      namespace: parseNamespace(solution, importRefs[0]),
    });
  };
}
