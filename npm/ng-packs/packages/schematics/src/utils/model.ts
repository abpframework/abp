import { strings } from '@angular-devkit/core';
import { Import, Interface, Model, Property, Type, TypeWithEnum } from '../models';
import { isEnumImport } from './enum';
import { parseNamespace } from './namespace';
import { relativePathToModel } from './path';
import { parseGenerics } from './tree';
import {
  createTypeSimplifier,
  createTypesToImportsReducer,
  flattenUnionTypes,
  normalizeTypeAnnotations,
  removeTypeModifiers,
} from './type';

export interface ModelGeneratorParams {
  targetPath: string;
  solution: string;
  types: Record<string, Type>;
  serviceImports: Record<string, string[]>;
  modelImports: Record<string, string[]>;
}

export function createImportRefsToModelMapper({ solution, types }: ModelGeneratorParams) {
  const mapImportRefToInterface = createImportRefToInterfaceMapper(types);
  const createImportRefToImportReducer = createImportRefToImportReducerCreator(solution, types);

  return (importRefs: string[]) => {
    const namespace = parseNamespace(solution, importRefs[0]);
    const path = relativePathToModel(namespace, namespace);
    const model = new Model({ namespace, path });

    const reduceImportRefToImport = createImportRefToImportReducer(namespace);
    const imports = importRefs.reduce((accumulatedImports, ref) => {
      const interfaceDirect = mapImportRefToInterface(ref);
      if (interfaceDirect && !types[ref].isEnum) model.interfaces.push(interfaceDirect);

      return reduceImportRefToImport(accumulatedImports, ref);
    }, []);

    imports.forEach(_import => {
      if (_import.path === model.path)
        return _import.refs.forEach(ref => {
          if (model.interfaces.some(i => i.ref === ref)) return;

          const interfaceIndirect = mapImportRefToInterface(ref);
          if (interfaceIndirect) model.interfaces.push(interfaceIndirect);
          reduceImportRefToImport(imports, ref);
        });

      model.imports.push(_import);
    });

    sortInterfaces(model.interfaces);

    return model;
  };
}

function sortInterfaces(interfaces: Interface[]) {
  interfaces.sort((a, b) => (a.identifier > b.identifier ? 1 : -1));
}

export function createImportRefToInterfaceMapper(types: Record<string, Type>) {
  const simplifyType = createTypeSimplifier();
  const getIdentifier = (type: string) => removeTypeModifiers(simplifyType(type));

  return (ref: string) => {
    const typeDef = types[ref];
    if (!typeDef) return;

    const identifier = (typeDef.genericArguments ?? []).reduce(
      (acc, t, i) => acc.replace(`T${i}`, t),
      getIdentifier(ref),
    );

    const base = typeDef.baseType ? getIdentifier(typeDef.baseType) : null;
    const _interface = new Interface({ identifier, base, ref });

    typeDef.properties?.forEach(({ name, typeSimple }) => {
      name = strings.camelize(name);
      const optional = typeSimple.endsWith('?') ? '?' : '';
      const type = simplifyType(typeSimple);

      _interface.properties.push(new Property({ name, optional, type }));
    });

    return _interface;
  };
}

export function createImportRefToImportReducerCreator(
  solution: string,
  types: Record<string, Type>,
) {
  return (namespace: string) => {
    const reduceTypesToImport = createTypesToImportsReducer(solution, namespace);

    return (imports: Import[], importRef: string) =>
      reduceTypesToImport(
        imports,
        mergeBaseTypeWithProperties(types[importRef]).reduce((acc: TypeWithEnum[], typeName) => {
          parseGenerics(typeName)
            .toGenerics()
            .forEach(type => acc.push({ type, isEnum: types[type]?.isEnum }));

          return acc;
        }, []),
      );
  };
}

export function mergeBaseTypeWithProperties({ baseType, genericArguments, properties }: Type) {
  const removeGenerics = createGenericRemover(genericArguments);
  const clearTypes = (type: string) => normalizeTypeAnnotations(removeGenerics(type));
  const baseTypes = baseType ? [baseType] : [];
  const propTypes = (properties ?? []).map(({ type }) => type);

  return [...baseTypes, ...propTypes].reduce(flattenUnionTypes, []).map(clearTypes);
}

export function createGenericRemover(genericArguments: string[] | null) {
  if (!genericArguments) return (type: string) => type;

  return (type: string) =>
    genericArguments.includes(type)
      ? ''
      : type.replace(/<([^<>]+)>/, (_, match) => {
          return match
            .split(/,\s*/)
            .filter((t: string) => !genericArguments.includes(t))
            .join(',');
        });
}

export function filterModelRefsToGenerate(
  modelImports: Record<string, string[]>,
  modelsCreated: Model[],
) {
  const created = modelsCreated.map(m => m.path);

  return Object.entries(modelImports).reduce((acc: string[][], [path, refs]) => {
    if (isEnumImport(path)) return acc;
    if (created.includes(path)) return acc;
    acc.push(refs);
    return acc;
  }, []);
}
