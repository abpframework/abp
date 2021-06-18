import { VOLO_REGEX } from '../constants';
import { Interface, Model, Property, PropertyDef, Type, TypeWithEnum } from '../models';
import {
  extractGenerics,
  generateRefWithPlaceholders,
  GenericsCollector,
  replacePlaceholdersWithGenerics,
} from './generics';
import { parseNamespace } from './namespace';
import { relativePathToModel } from './path';
import { camel } from './text';
import { parseGenerics } from './tree';
import {
  createTypeParser,
  createTypeSimplifier,
  createTypesToImportsReducer,
  extendsSelf,
  removeTypeModifiers,
} from './type';
const shouldQuote = require('should-quote');

export interface ModelGeneratorParams {
  targetPath: string;
  solution: string;
  types: Record<string, Type>;
  serviceImports: Record<string, string[]>;
  modelImports: Record<string, string[]>;
}

export function createImportRefsToModelReducer(params: ModelGeneratorParams) {
  const reduceImportRefsToInterfaces = createImportRefToInterfaceReducerCreator(params);
  const createRefToImportReducer = createRefToImportReducerCreator(params);
  const { solution, types } = params;

  return (models: Model[], importRefs: string[]) => {
    const enums: string[] = [];
    const interfaces = importRefs.reduce(reduceImportRefsToInterfaces, []);

    sortInterfaces(interfaces);

    interfaces.forEach(_interface => {
      if (VOLO_REGEX.test(_interface.ref)) return;

      if (types[_interface.ref]!.isEnum) {
        if (!enums.includes(_interface.ref)) enums.push(_interface.ref);
        return;
      }

      const index = models.findIndex(m => m.namespace === _interface.namespace);
      if (index > -1) {
        if (models[index].interfaces.some(i => i.identifier === _interface.identifier)) return;

        models[index].interfaces.push(_interface);
      } else {
        const { namespace } = _interface;

        models.push(
          new Model({
            interfaces: [_interface],
            namespace,
            path: relativePathToModel(namespace, namespace),
          }),
        );
      }
    });

    models.forEach(model => {
      const toBeImported: TypeWithEnum[] = [];

      model.interfaces.forEach(_interface => {
        const { baseType } = types[_interface.ref];

        if (baseType && parseNamespace(solution, baseType) !== model.namespace)
          toBeImported.push({
            type: baseType.split('<')[0],
            isEnum: false,
          });

        [..._interface.properties, ..._interface.generics].forEach(prop => {
          prop.refs.forEach(ref => {
            const propType = types[ref];
            if (!propType) return;
            if (propType.isEnum) toBeImported.push({ type: ref, isEnum: true });
            else if (parseNamespace(solution, ref) !== model.namespace)
              toBeImported.push({ type: ref, isEnum: false });
          });
        });
      });

      if (!toBeImported.length) return;

      const reduceRefToImport = createRefToImportReducer(model.namespace);
      reduceRefToImport(model.imports, toBeImported);
    });

    return models;
  };
}

function sortInterfaces(interfaces: Interface[]) {
  interfaces.sort((a, b) => (a.identifier > b.identifier ? 1 : -1));
}

export function createImportRefToInterfaceReducerCreator(params: ModelGeneratorParams) {
  const { solution, types } = params;
  const parseType = createTypeParser(removeTypeModifiers);
  const simplifyType = createTypeSimplifier();
  const getIdentifier = (type: string) => removeTypeModifiers(simplifyType(type));
  const genericsCollector = new GenericsCollector(getIdentifier);

  return reduceRefsToInterfaces;

  function reduceRefsToInterfaces(interfaces: Interface[], ref: string): Interface[] {
    const typeDef = types[ref];
    if (!typeDef) return interfaces;

    const namespace = parseNamespace(solution, ref);

    let { baseType: base, genericArguments } = typeDef;
    genericArguments = genericArguments || [];
    let identifier = getIdentifier(ref);
    identifier = replacePlaceholdersWithGenerics(identifier, genericArguments, genericsCollector);

    if (base) {
      if (extendsSelf(ref, base)) {
        genericsCollector.collect(extractGenerics(base).generics, genericArguments);
        return reduceRefsToInterfaces(interfaces, generateRefWithPlaceholders(base));
      } else {
        base = getIdentifier(base);
      }
    }

    const { generics } = genericsCollector;
    const _interface = new Interface({ identifier, base, namespace, ref, generics });
    genericsCollector.reset();

    typeDef.properties?.forEach(prop => {
      let name = prop.jsonName || camel(prop.name);
      name = shouldQuote(name) ? `'${name}'` : name;
      const type = simplifyType(prop.typeSimple);
      const refs = parseType(prop.type).reduce(
        (acc: string[], r) => acc.concat(parseGenerics(r).toGenerics()),
        [],
      );
      const property = new Property({ name, type, refs });
      property.setOptional(isOptionalProperty(prop));

      _interface.properties.push(property);
    });

    interfaces.push(_interface);

    return [..._interface.properties, ..._interface.generics]
      .reduce<string[]>((refs, prop) => {
        prop.refs.forEach(type => {
          if (types[type]?.isEnum) return;
          if (interfaces.some(i => i.ref === type)) return;
          refs.push(type);
        });

        return refs;
      }, [])
      .concat(base ? parseGenerics(typeDef.baseType!).toGenerics() : [])
      .reduce(reduceRefsToInterfaces, interfaces);
  }
}

export function createRefToImportReducerCreator(params: ModelGeneratorParams) {
  const { solution } = params;
  return (namespace: string) => createTypesToImportsReducer(solution, namespace);
}

function isOptionalProperty(prop: PropertyDef) {
  return (
    prop.typeSimple.endsWith('?') || (prop.typeSimple === 'string' && prop.isRequired === false)
  );
}
