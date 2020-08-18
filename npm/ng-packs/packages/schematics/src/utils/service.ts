import { eImportKeyword } from '../enums';
import {
  Action,
  Body,
  Controller,
  Import,
  Method,
  Parameter,
  Service,
  Signature,
  TypeDef,
} from '../models';
import { parseNamespace } from './namespace';
import { relativePathFromServiceToModel } from './path';

export function serializeParameters(parameters: Parameter[]) {
  return parameters.map(p => p.name + p.optional + ': ' + p.type + p.default, '').join(', ');
}

export function createControllerToServiceMapper(solution: string, apiName: string) {
  const mapActionToMethod = createActionToMethodMapper(solution);

  return (controller: Controller) => {
    const name = controller.controllerName;
    const namespace = parseNamespace(solution, controller.type);
    const actions = Object.values(controller.actions);
    const imports = actions.reduce(createActionToImportsReducer(solution, namespace), []);
    const methods = actions.map(mapActionToMethod);
    return new Service({ apiName, imports, methods, name, namespace });
  };
}

export function createActionToImportsReducer(solution: string, namespace: string) {
  const mapTypeDefToImport = createTypeDefToImportMapper(solution, namespace);

  return (imports: Import[], action: Action) => {
    const typeDefs = getTypeDefsFromAction(action);

    typeDefs.forEach(typeDef => {
      const def = mapTypeDefToImport(typeDef);
      if (!def) return;

      const existingImport = imports.find(
        ({ keyword, path }) => keyword === def.keyword && path === def.path,
      );
      if (!existingImport) return imports.push(def);

      existingImport.specifiers = [
        ...new Set([...existingImport.specifiers, ...def.specifiers]),
      ].sort();
    });
    return imports;
  };
}

export function createTypeDefToImportMapper(solution: string, namespace: string) {
  const adaptType = createTypeAdapter(solution);

  return ({ type, typeSimple }: TypeDef) => {
    if (type.startsWith('System')) return;
    const modelNamespace = parseNamespace(solution, type);
    const path = type.startsWith('Volo.Abp.Application.Dtos')
      ? '@abp/ng.core'
      : relativePathFromServiceToModel(namespace, modelNamespace);
    const specifiers = [adaptType(typeSimple.split('<')[0])];
    return new Import({ keyword: eImportKeyword.Type, path, specifiers });
  };
}

export function createActionToMethodMapper(solution: string) {
  const mapActionToBody = createActionToBodyMapper(solution);
  const mapActionToSignature = createActionToSignatureMapper(solution);

  return (action: Action) => {
    const body = mapActionToBody(action);
    const signature = mapActionToSignature(action);
    return new Method({ body, signature });
  };
}

export function createActionToBodyMapper(solution: string) {
  const adaptType = createTypeAdapter(solution);

  return ({ httpMethod, parameters, returnValue, url }: Action) => {
    const responseType = adaptType(returnValue.typeSimple);
    const body = new Body({ method: httpMethod, responseType, url });

    parameters.forEach(body.registerActionParameter);

    return body;
  };
}

export function createActionToSignatureMapper(solution: string) {
  const adaptType = createTypeAdapter(solution);

  return (action: Action) => {
    const signature = new Signature({ name: getMethodNameFromAction(action) });

    signature.parameters = action.parametersOnMethod.map(p => {
      const type = adaptType(p.typeSimple);
      const parameter = new Parameter({ name: p.name, type });
      if (p.defaultValue) parameter.default = ` = ${p.defaultValue}`;
      else if (p.isOptional) parameter.optional = '?';
      return parameter;
    });

    return signature;
  };
}

function getMethodNameFromAction(action: Action): string {
  return action.uniqueName.split('Async')[0];
}

function createTypeAdapter(solution: string) {
  const optionalRegex = /\?/g;
  const solutionRegex = new RegExp(solution.replace(/\./g, `\.`) + `\.`);
  const voloRegex = /^Volo\.(Abp\.?)(Application\.?)/;

  return (typeSimple: string) => {
    if (typeSimple === 'System.Void') return 'void';

    return parseGenerics(typeSimple).reduceRight((acc, type) => {
      type = type.replace(voloRegex, '');
      type = type.replace(solutionRegex, '');
      type = type.replace(optionalRegex, '');
      type = type.split('.').pop()!;
      return acc ? `${type}<${acc}>` : type;
    }, '');
  };
}

function parseGenerics(type: string) {
  return type.replace(/>+$/, '').split('<');
}

function getTypeDefsFromAction({ parametersOnMethod, returnValue }: Action) {
  const typeDefs: TypeDef[] = [];

  [returnValue, ...parametersOnMethod].forEach(({ type, typeSimple }) => {
    const types = parseGenerics(type);
    const simpleTypes = parseGenerics(typeSimple);

    types.forEach((type, i) => typeDefs.push({ type, typeSimple: simpleTypes[i] }));
  });

  return typeDefs;
}
