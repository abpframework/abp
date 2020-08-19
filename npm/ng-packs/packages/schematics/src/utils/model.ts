import { strings } from '@angular-devkit/core';
import { Interface, Model, Property, Type } from '../models';
import { parseNamespace } from './namespace';
import { createTypeSimplifier } from './type';

export function createImportRefsToModelMapper(solution: string, types: Record<string, Type>) {
  const simplifyType = createTypeSimplifier(solution);

  return (importRefs: string[]) => {
    const model = new Model({
      namespace: parseNamespace(solution, importRefs[0]),
    });

    importRefs.forEach(ref => {
      const typeDef = types[ref];
      let identifier = simplifyType(ref);
      (typeDef.genericArguments ?? []).forEach((t, i) => {
        identifier = identifier.replace(`T${i}`, t);
      });

      const base = typeDef.baseType ? simplifyType(typeDef.baseType) : null;
      const _interface = new Interface({ identifier, base });

      typeDef.properties?.forEach(({ name, typeSimple }) => {
        name = strings.camelize(name);
        const type = simplifyType(typeSimple);
        const optional = typeSimple.endsWith('?') ? '?' : '';

        _interface.properties.push(new Property({ name, type, optional }));
      });

      console.log(_interface);

      model.interfaces.push(_interface);
    });

    return model;
  };
}
