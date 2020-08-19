import { strings } from '@angular-devkit/core';
import { Import, Interface, Model, Property, Type } from '../models';
import { sortImports } from './import';
import { parseNamespace } from './namespace';
import { relativePathToModel } from './path';
import { parseGenerics } from './tree';
import { createTypeSimplifier, createTypesToImportsReducer } from './type';

export function createImportRefsToModelMapper(solution: string, types: Record<string, Type>) {
  const mapImportRefToInterface = createImportRefToInterfaceMapper(solution, types);
  const createImportRefToImportReducer = createImportRefToImportReducerCreator(solution, types);

  return (importRefs: string[]) => {
    const namespace = parseNamespace(solution, importRefs[0]);
    const model = new Model({ namespace });

    const reduceImportRefToImport = createImportRefToImportReducer(namespace);
    const imports = importRefs.reduce((accumulatedImports, ref) => {
      model.interfaces.push(mapImportRefToInterface(ref));
      return reduceImportRefToImport(accumulatedImports, ref);
    }, []);

    sortImports(imports);
    const selfPath = relativePathToModel(namespace, namespace);
    imports.forEach(i => {
      if (i.path === selfPath) return;
      model.imports.push(i);
    });

    model.interfaces.sort((a, b) => (a.identifier > b.identifier ? 1 : -1));

    return model;
  };
}

export function createImportRefToInterfaceMapper(solution: string, types: Record<string, Type>) {
  const simplifyType = createTypeSimplifier(solution);

  return (ref: string) => {
    const typeDef = types[ref];
    const identifier = (typeDef.genericArguments ?? []).reduce(
      (acc, t, i) => acc.replace(`T${i}`, t),
      simplifyType(ref),
    );

    const base = typeDef.baseType ? simplifyType(typeDef.baseType) : null;
    const _interface = new Interface({ identifier, base });

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
        mergeBaseTypeWithProperties(types[importRef]).reduce((typeNames: string[], type) => {
          parseGenerics(type)
            .toGenerics()
            .forEach(t => typeNames.push(t));

          return typeNames;
        }, []),
      );
  };
}

export function mergeBaseTypeWithProperties({ baseType, genericArguments, properties }: Type) {
  const removeGenerics = createGenericRemover(genericArguments);
  const baseTypes = baseType ? [baseType] : [];
  const propTypes = (properties ?? []).map(({ type }) => type);

  return [...baseTypes, ...propTypes].map(removeGenerics);
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
