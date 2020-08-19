import { strings } from '@angular-devkit/core';
import { SYSTEM_TYPES } from '../constants';
import { eImportKeyword } from '../enums';
import { Import } from '../models';
import { parseNamespace } from './namespace';
import { relativePathToModel } from './path';
import { parseGenerics } from './tree';

export function createTypeSimplifier(solution: string) {
  const optionalRegex = /\?/g;
  const solutionRegex = new RegExp(solution.replace(/\./g, `\.`) + `\.`);
  const voloRegex = /^Volo\.(Abp\.?)(Application\.?)/;

  return (type: string) => {
    type = type.replace(voloRegex, '');
    type = type.replace(solutionRegex, '');
    type = type.replace(optionalRegex, '');
    type = type.replace(
      /System\.([0-9A-Za-z]+)/g,
      (_, match) => SYSTEM_TYPES.get(match) ?? strings.camelize(match),
    );
    type = type.split('.').pop()!;
    type = type.startsWith('[') ? type.slice(1, -1) + '[]' : type;
    return type;
  };
}

export function createTypesToImportsReducer(solution: string, namespace: string) {
  const mapTypeToImport = createTypeToImportMapper(solution, namespace);

  return (imports: Import[], types: string[]) => {
    types.forEach(type => {
      const newImport = mapTypeToImport(type);
      if (!newImport) return;

      const existingImport = imports.find(
        ({ keyword, path }) => keyword === newImport.keyword && path === newImport.path,
      );
      if (!existingImport) return imports.push(newImport);

      existingImport.refs = [...new Set([...existingImport.refs, ...newImport.refs])];
      existingImport.specifiers = [
        ...new Set([...existingImport.specifiers, ...newImport.specifiers]),
      ].sort();
    });

    return imports;
  };
}

export function createTypeToImportMapper(solution: string, namespace: string) {
  const adaptType = createTypeAdapter(solution);

  return (type: string) => {
    if (!type || type.startsWith('System')) return;

    const modelNamespace = parseNamespace(solution, type);
    const path = type.startsWith('Volo.Abp.Application.Dtos')
      ? '@abp/ng.core'
      : relativePathToModel(namespace, modelNamespace);
    const refs = [type];
    const specifiers = [adaptType(type.split('<')[0])];

    return new Import({ keyword: eImportKeyword.Type, path, refs, specifiers });
  };
}

export function createTypeAdapter(solution: string) {
  const simplifyType = createTypeSimplifier(solution);
  return (type: string) => parseGenerics(type, node => simplifyType(node.data)).toString();
}
