import { strings } from '@angular-devkit/core';
import { Interface, Model, Property, Type } from '../models';
import { parseNamespace } from './namespace';
import { createTypeSimplifier } from './type';

export function createImportRefsToModelMapper(solution: string, types: Record<string, Type>) {
  const mapImportRefToInterface = createImportRefToInterfaceMapper(solution, types);

  return (importRefs: string[]) => {
    const model = new Model({
      namespace: parseNamespace(solution, importRefs[0]),
    });

    importRefs.forEach(ref => {
      model.interfaces.push(mapImportRefToInterface(ref));
    });

    model.interfaces.sort((a, b) => (a.identifier > b.identifier ? 1 : -1));

    return model;
  };
}

export function createImportRefToInterfaceMapper(solution: string, types: Record<string, Type>) {
  const simplifyType = createTypeSimplifier(solution);

  return (ref: string) => {
    const typeDef = types[ref];
    let identifier = simplifyType(ref);
    (typeDef.genericArguments ?? []).forEach((t, i) => {
      identifier = identifier.replace(`T${i}`, t);
    });

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
