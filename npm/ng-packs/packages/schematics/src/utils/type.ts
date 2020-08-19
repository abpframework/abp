import { strings } from '@angular-devkit/core';
import { SYSTEM_TYPES } from '../constants';
import { eImportKeyword } from '../enums';
import { Import, TypeWithEnum } from '../models';
import { parseNamespace } from './namespace';
import { relativePathToEnum, relativePathToModel } from './path';
import { parseGenerics } from './tree';

export function createTypeSimplifier(solution: string) {
  const solutionRegex = new RegExp(solution.replace(/\./g, `\.`) + `\.`);
  const voloRegex = /^Volo\.(Abp\.?)(Application\.?)/;

  return createTypeParser(
    type =>
      type
        .replace(voloRegex, '')
        .replace(solutionRegex, '')
        .split('.')
        .pop()!,
  );
}

export function createTypeParser(replacerFn = (t: string) => t) {
  return (originalType: string) =>
    flattenUnionTypes([], originalType)
      .map(type => {
        type = removeTypeModifiers(type);
        type = type.replace(
          /System\.([0-9A-Za-z]+)/g,
          (_, match) => SYSTEM_TYPES.get(match) ?? strings.camelize(match),
        );

        return replacerFn(type);
      })
      .join(' | ');
}

export function flattenUnionTypes(types: string[], type: string) {
  type
    .replace(/^{/, '')
    .replace(/}$/, '')
    .split(':')
    .forEach(t => types.push(t));

  return types;
}

export function removeTypeModifiers(type: string) {
  type = type.startsWith('[') ? type.slice(1, -1) + '[]' : type;
  return type.replace(/\?/g, '');
}

export function createTypesToImportsReducer(solution: string, namespace: string) {
  const mapTypeToImport = createTypeToImportMapper(solution, namespace);

  return (imports: Import[], types: TypeWithEnum[]) => {
    types.forEach(({ type, isEnum }) => {
      const newImport = mapTypeToImport(type, isEnum);
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

  return (type: string, isEnum: boolean) => {
    if (!type || type.startsWith('System')) return;

    const modelNamespace = parseNamespace(solution, type);
    const refs = [type];
    const specifiers = [adaptType(type.split('<')[0])];
    const path = type.startsWith('Volo.Abp.Application.Dtos')
      ? '@abp/ng.core'
      : isEnum
      ? relativePathToEnum(namespace, modelNamespace, specifiers[0])
      : relativePathToModel(namespace, modelNamespace);

    return new Import({ keyword: eImportKeyword.Type, path, refs, specifiers });
  };
}

export function createTypeAdapter(solution: string) {
  const simplifyType = createTypeSimplifier(solution);
  return (type: string) => parseGenerics(type, node => simplifyType(node.data)).toString();
}
