import { Model } from '../models';
import { parseNamespace } from './namespace';

export function createImportRefToModelMapper(solution: string) {
  return (importRef: string) => {
    return new Model({
      namespace: parseNamespace(solution, importRef),
    });
  };
}
