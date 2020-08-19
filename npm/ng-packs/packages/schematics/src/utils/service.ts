import { Action, Body, Controller, Import, Method, Property, Service, Signature } from '../models';
import { parseNamespace } from './namespace';
import { parseGenerics } from './tree';
import { createTypeAdapter, createTypesToImportsReducer } from './type';

export function serializeParameters(parameters: Property[]) {
  return parameters.map(p => p.name + p.optional + ': ' + p.type + p.default, '').join(', ');
}

export function createControllerToServiceMapper(solution: string, apiName: string) {
  const mapActionToMethod = createActionToMethodMapper(solution);

  return (controller: Controller) => {
    const name = controller.controllerName;
    const namespace = parseNamespace(solution, controller.type);
    const actions = Object.values(controller.actions);
    const imports = actions.reduce(createActionToImportsReducer(solution, namespace), []);
    imports.push(new Import({ path: '@abp/ng.core', specifiers: ['RestService'] }));
    imports.push(new Import({ path: '@angular/core', specifiers: ['Injectable'] }));
    sortImports(imports);
    const methods = actions.map(mapActionToMethod);
    sortMethods(methods);
    return new Service({ apiName, imports, methods, name, namespace });
  };
}

function sortImports(imports: Import[]) {
  imports.sort((a, b) =>
    removeRelative(a) > removeRelative(b) ? 1 : a.keyword > b.keyword ? 1 : -1,
  );
}

function removeRelative(importDef: Import) {
  return importDef.path.replace(/\.\.\//g, '');
}

function sortMethods(methods: Method[]) {
  methods.sort((a, b) => (a.signature.name > b.signature.name ? 1 : -1));
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
      const parameter = new Property({ name: p.name, type });
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

function createActionToImportsReducer(solution: string, namespace: string) {
  const mapTypesToImports = createTypesToImportsReducer(solution, namespace);

  return (imports: Import[], { parametersOnMethod, returnValue }: Action) =>
    mapTypesToImports(
      imports,
      [returnValue, ...parametersOnMethod].reduce((types: string[], { type }) => {
        parseGenerics(type)
          .toGenerics()
          .forEach(t => types.push(t));

        return types;
      }, []),
    );
}
