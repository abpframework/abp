import { strings } from '@angular-devkit/core';
import { Import, Interface, Model, Property, Type, TypeWithEnum } from '../models';
import { sortImports } from './import';
import { parseNamespace } from './namespace';
import { relativePathToModel } from './path';
import { parseGenerics } from './tree';
import {
  createTypeSimplifier,
  createTypesToImportsReducer,
  flattenUnionTypes,
  removeTypeModifiers,
} from './type';

export function createImportRefsToModelMapper(solution: string, types: Record<string, Type>) {
  const mapImportRefToInterface = createImportRefToInterfaceMapper(solution, types);
  const createImportRefToImportReducer = createImportRefToImportReducerCreator(solution, types);

  return (importRefs: string[]) => {
    const namespace = parseNamespace(solution, importRefs[0]);
    const model = new Model({ namespace });

    const reduceImportRefToImport = createImportRefToImportReducer(namespace);
    const imports = importRefs.reduce((accumulatedImports, ref) => {
      const interfaceDirect = mapImportRefToInterface(ref);
      if (interfaceDirect && !types[ref].isEnum) model.interfaces.push(interfaceDirect);

      return reduceImportRefToImport(accumulatedImports, ref);
    }, []);

    sortImports(imports);

    const selfPath = relativePathToModel(namespace, namespace);
    imports.forEach(_import => {
      if (_import.path === selfPath)
        return _import.refs.forEach(ref => {
          if (model.interfaces.some(i => i.ref === ref)) return;

          const interfaceIndirect = mapImportRefToInterface(ref);
          if (interfaceIndirect) model.interfaces.push(interfaceIndirect);
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

export function createImportRefToInterfaceMapper(solution: string, types: Record<string, Type>) {
  const simplifyType = createTypeSimplifier(solution);

  return (ref: string) => {
    const typeDef = types[ref];
    if (!typeDef) return;

    const identifier = (typeDef.genericArguments ?? []).reduce(
      (acc, t, i) => acc.replace(`T${i}`, t),
      simplifyType(ref),
    );

    const base = typeDef.baseType ? simplifyType(typeDef.baseType) : null;
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
  const clearTypes = (type: string) => removeTypeModifiers(removeGenerics(type));
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
