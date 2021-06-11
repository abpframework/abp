import { SYSTEM_TYPES, VOLO_REGEX } from '../constants';
import { eImportKeyword } from '../enums';
import { Import, TypeWithEnum } from '../models';
import { extractSimpleGenerics } from './generics';
import { parseNamespace } from './namespace';
import { relativePathToEnum, relativePathToModel } from './path';
import { parseGenerics } from './tree';

export function createTypeSimplifier() {
  const parseType = createTypeParser(t => {
    let type = t.replace(
      /(?<![^<, ])System\.([0-9A-Za-z.]+)/g,
      (_, match) => SYSTEM_TYPES.get(match) ?? 'any',
    );

    type = /any</.test(type) ? 'any' : type;

    const { identifier, generics } = extractSimpleGenerics(type);

    return generics.length ? `${identifier}<${generics.join(', ')}>` : identifier;
  });

  return (type: string) => {
    const parsed = parseType(type);
    const last = parsed.pop()!;
    return parsed.reduceRight((record, tKey) => `Record<${tKey}, ${record}>`, last);
  };
}

export function createTypeParser(replacerFn = (t: string) => t) {
  const normalizeType = createTypeNormalizer(replacerFn);

  return (originalType: string) => flattenDictionaryTypes([], originalType).map(normalizeType);
}

export function createTypeNormalizer(replacerFn = (t: string) => t) {
  return (type: string) => {
    return replacerFn(normalizeTypeAnnotations(type));
  };
}

export function flattenDictionaryTypes(types: string[], type: string) {
  type
    .replace(/[}{]/g, '')
    .split(':')
    .forEach(t => types.push(t));

  return types;
}

export function normalizeTypeAnnotations(type: string) {
  return type.replace(/\[(.+)+\]/g, '$1[]').replace(/\?/g, '');
}

export function removeGenerics(type: string) {
  return type.replace(/<.+>/g, '');
}

export function removeTypeModifiers(type: string) {
  return type.replace(/\[\]/g, '');
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
  const adaptType = createTypeAdapter();
  const simplifyType = createTypeSimplifier();

  return (type: string, isEnum: boolean) => {
    if (!type || type.startsWith('System')) return;

    const modelNamespace = parseNamespace(solution, type);
    const refs = [removeTypeModifiers(type)];
    const specifiers = [adaptType(simplifyType(refs[0]).split('<')[0])];
    const path = VOLO_REGEX.test(type)
      ? '@abp/ng.core'
      : isEnum
      ? relativePathToEnum(namespace, modelNamespace, specifiers[0])
      : relativePathToModel(namespace, modelNamespace);

    return new Import({ keyword: eImportKeyword.Type, path, refs, specifiers });
  };
}

export function createTypeAdapter() {
  const simplifyType = createTypeSimplifier();
  return (type: string) => parseGenerics(type, node => simplifyType(node.data)).toString();
}

// naming here is depictive only
export function extendsSelf(type: string, base: string) {
  return removeGenerics(base) === removeGenerics(type);
}
