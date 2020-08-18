import { eImportKeyword } from '../enums';
import {
  Action,
  Body,
  Controller,
  Import,
  Method,
  Parameter,
  ReturnValue,
  Service,
  Signature,
} from '../models';
import { parseNamespace } from './namespace';
import { dir } from './text';

export function createControllerToServiceMapper(solution: string, apiName: string) {
  const mapActionToMethod = createActionToMethodMapper(solution);

  return (controller: Controller) => {
    const actions = Object.values(controller.actions);
    const imports = actions.reduce(createActionToImportsReducer(solution), []);
    const methods = actions.map(mapActionToMethod);
    const name = controller.controllerName;
    const namespace = parseNamespace(solution, controller.type);
    return new Service({ apiName, imports, methods, name, namespace });
  };
}

export function createActionToImportsReducer(solution: string) {
  const mapTypeDefToImport = createTypeDefToImportMapper(solution);

  return (imports: Import[], action: Action) => {
    const typeDefs = [action.returnValue, ...action.parametersOnMethod];
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

export function createTypeDefToImportMapper(solution: string) {
  const adaptType = createTypeAdapter(solution);

  return ({ type, typeSimple }: ReturnValue) => {
    if (type.startsWith('System')) return;
    const namespace = parseNamespace(solution, type);
    const path = type.startsWith('Volo.Abp.Application.Dtos')
      ? '@volo/abp.ng.core'
      : `@shared/models/${dir(namespace)}`;
    const specifier = adaptType(typeSimple.split('<')[0]);
    return new Import({ keyword: eImportKeyword.Type, path, specifiers: [specifier] });
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

    return typeSimple
      .replace(/>+$/, '')
      .split('<')
      .reduceRight((acc, type) => {
        type = type.replace(voloRegex, '');
        type = type.replace(solutionRegex, '');
        type = type.replace(optionalRegex, '');
        type = type.split('.').pop()!;
        return acc ? `${type}<${acc}>` : type;
      }, '');
  };
}
