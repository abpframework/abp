import {
  Action,
  Body,
  Controller,
  Import,
  Method,
  Property,
  Service,
  ServiceGeneratorParams,
  Signature,
  Type,
  TypeWithEnum,
} from '../models';
import { sortImports } from './import';
import { parseNamespace } from './namespace';
import { parseGenerics } from './tree';
import {
  createTypeAdapter,
  createTypeParser,
  createTypesToImportsReducer,
  removeTypeModifiers,
} from './type';

export function serializeParameters(parameters: Property[]) {
  return parameters.map(p => p.name + p.optional + ': ' + p.type + p.default, '').join(', ');
}

export function createControllerToServiceMapper({
  solution,
  types,
  apiName,
}: ServiceGeneratorParams) {
  const mapActionToMethod = createActionToMethodMapper();

  return (controller: Controller) => {
    const name = controller.controllerName;
    const namespace = parseNamespace(solution, controller.type);
    const actions = Object.values(controller.actions);
    const imports = actions.reduce(createActionToImportsReducer(solution, types, namespace), []);
    imports.push(new Import({ path: '@abp/ng.core', specifiers: ['RestService'] }));
    imports.push(new Import({ path: '@angular/core', specifiers: ['Injectable'] }));
    sortImports(imports);
    const methods = actions.map(mapActionToMethod);
    sortMethods(methods);
    return new Service({ apiName, imports, methods, name, namespace });
  };
}

function sortMethods(methods: Method[]) {
  methods.sort((a, b) => (a.signature.name > b.signature.name ? 1 : -1));
}

export function createActionToMethodMapper() {
  const mapActionToBody = createActionToBodyMapper();
  const mapActionToSignature = createActionToSignatureMapper();

  return (action: Action) => {
    const body = mapActionToBody(action);
    const signature = mapActionToSignature(action);
    return new Method({ body, signature });
  };
}

export function createActionToBodyMapper() {
  const adaptType = createTypeAdapter();

  return ({ httpMethod, parameters, returnValue, url }: Action) => {
    const responseType = adaptType(returnValue.typeSimple);
    const body = new Body({ method: httpMethod, responseType, url });

    parameters.forEach(body.registerActionParameter);

    return body;
  };
}

export function createActionToSignatureMapper() {
  const adaptType = createTypeAdapter();

  return (action: Action) => {
    const signature = new Signature({ name: getMethodNameFromAction(action) });

    signature.parameters = action.parametersOnMethod.map(p => {
      const type = adaptType(p.typeSimple);
      const parameter = new Property({ name: p.name, type });
      parameter.setDefault(p.defaultValue);
      parameter.setOptional(p.isOptional);
      return parameter;
    });

    return signature;
  };
}

function getMethodNameFromAction(action: Action): string {
  return action.uniqueName.split('Async')[0];
}

function createActionToImportsReducer(
  solution: string,
  types: Record<string, Type>,
  namespace: string,
) {
  const mapTypesToImports = createTypesToImportsReducer(solution, namespace);
  const parseType = createTypeParser(removeTypeModifiers);

  return (imports: Import[], { parametersOnMethod, returnValue }: Action) =>
    mapTypesToImports(
      imports,
      [returnValue, ...parametersOnMethod].reduce((acc: TypeWithEnum[], param) => {
        parseType(param.type).forEach(paramType =>
          parseGenerics(paramType)
            .toGenerics()
            .forEach(type => {
              if (types[type]) acc.push({ type, isEnum: types[type].isEnum });
            }),
        );

        return acc;
      }, []),
    );
}
