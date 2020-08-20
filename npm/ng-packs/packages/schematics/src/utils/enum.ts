import { SchematicsException } from '@angular-devkit/schematics';
import { Exception } from '../enums';
import { Type } from '../models';
import { interpolate } from './common';
import { parseNamespace } from './namespace';

export function getEnumNamesFromImports(serviceImports: Record<string, string[]>) {
  return Object.keys(serviceImports)
    .filter(path => path.includes('/enums/'))
    .reduce((acc: string[], path) => {
      serviceImports[path].forEach(_import => acc.push(_import));
      return acc;
    }, []);
}

export function createImportRefToEnumMapper(solution: string, types: Record<string, Type>) {
  return (ref: string) => {
    const { enumNames, enumValues } = types[ref];
    if (!enumNames || !enumValues)
      throw new SchematicsException(interpolate(Exception.NoTypeDefinition, ref));

    const namespace = parseNamespace(solution, ref);
    const members = enumNames!.map((key, i) => ({ key, value: enumValues[i] }));

    return {
      namespace,
      name: ref.split('.').pop()!,
      members,
    };
  };
}
