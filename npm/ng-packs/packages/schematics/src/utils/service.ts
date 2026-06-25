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
import { extractGenerics } from './generics';
import { isCollectionType } from './methods';
import {
  createTypeAdapter,
  createTypeParser,
  createTypesToImportsReducer,
  getTypeForEnumList,
  removeTypeModifiers,
} from './type';
import { eBindingSourceId } from '../enums';
import { camelizeHyphen } from './text';
import { VOLO_REMOTE_STREAM_CONTENT } from '../constants';

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
    const typeWithoutIRemoteStreamContent = getTypesWithoutIRemoteStreamContent(types);
    const imports = actions.reduce(
      createActionToImportsReducer(solution, typeWithoutIRemoteStreamContent, namespace),
      [],
    );
    imports.push(new Import({ path: '@abp/ng.core', specifiers: ['RestService', 'Rest'] }));
    imports.push(new Import({ path: '@angular/core', specifiers: ['Injectable', 'inject'] }));
    sortImports(imports);
    const methods = actions.map(mapActionToMethod);
    sortMethods(methods);
    return new Service({ apiName, imports, methods, name, namespace });
  };
}

function getTypesWithoutIRemoteStreamContent(types: Record<string, Type>) {
  const newType = { ...types };
  VOLO_REMOTE_STREAM_CONTENT.forEach(fileType => {
    delete newType[fileType];
  });
  return newType;
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
    let responseType = adaptType(returnValue.typeSimple);
    if (responseType.includes('enum')) {
      const type = returnValue.typeSimple.replace('enum', returnValue.type);

      if (responseType === 'enum') {
        responseType = adaptType(type);
      }

      if (responseType === 'enum[]') {
        const normalizedType = getTypeForEnumList(type);
        responseType = adaptType(normalizedType);
      }
    }
    if (isRemoteStreamContentArray(returnValue.typeSimple)) {
      responseType = 'any[]';
    }
    const responseTypeWithNamespace = returnValue.typeSimple;
    const { httpResponseType, acceptHeader } = resolveHttpResponseAndAccept(
      responseType,
      responseTypeWithNamespace,
      returnValue.contentTypes,
      returnValue.isRemoteStream,
    );
    const body = new Body({
      method: httpMethod,
      responseType,
      url,
      responseTypeWithNamespace,
      httpResponseType,
      acceptHeader,
    });

    const uploadMethodArgNames = new Set(
      parameters
        .filter(p => p.bindingSourceId === eBindingSourceId.FormFile)
        .map(p => p.nameOnMethod),
    );
    if (uploadMethodArgNames.size > 0) {
      body.body = camelizeHyphen([...uploadMethodArgNames][0]);
      parameters
        .filter(p => {
          if (uploadMethodArgNames.has(p.nameOnMethod)) {
            return false;
          }
          return (
            p.bindingSourceId !== eBindingSourceId.Form &&
            p.bindingSourceId !== eBindingSourceId.FormFile
          );
        })
        .forEach(body.registerActionParameter);
    } else {
      parameters.forEach(body.registerActionParameter);
    }

    return body;
  };
}

function normalizeMediaType(mediaType: string): string {
  const semi = mediaType.indexOf(';');
  return (semi < 0 ? mediaType : mediaType.slice(0, semi)).trim().toLowerCase();
}

function isJsonMediaType(normalized: string): boolean {
  return normalized === 'application/json' || normalized === 'text/json' || normalized.endsWith('+json');
}

function isBinaryMediaType(mediaType: string): boolean {
  const m = normalizeMediaType(mediaType);
  if (
    m === 'application/octet-stream' ||
    m === 'application/pdf' ||
    m === 'application/zip' ||
    m === 'application/x-zip-compressed' ||
    m === 'application/gzip' ||
    m === 'application/x-tar' ||
    m === 'application/x-7z-compressed' ||
    m === 'application/wasm' ||
    m === 'application/x-msdownload' ||
    m === 'application/x-msdos-program' ||
    m === 'application/rtf' ||
    m === 'application/x-rar-compressed' ||
    m === 'application/x-rar' ||
    m === 'application/x-bzip2' ||
    m === 'application/x-iso9660-image' ||
    m === 'application/x-apple-diskimage' ||
    m === 'application/java-archive' ||
    m === 'application/epub+zip' ||
    m === 'model/gltf-binary'
  ) {
    return true;
  }
  if (m.startsWith('image/') || m.startsWith('video/') || m.startsWith('audio/') || m.startsWith('font/')) {
    return true;
  }
  // Office / OpenDocument / generic vnd.* (excluding +json / +xml which are structured text)
  if (
    m.startsWith('application/vnd.openxmlformats-') ||
    m.startsWith('application/vnd.ms-') ||
    m.startsWith('application/vnd.oasis.opendocument.')
  ) {
    return true;
  }
  if (m.startsWith('application/vnd.') && !m.endsWith('+json') && !m.endsWith('+xml')) {
    return true;
  }
  return false;
}

function resolveHttpResponseAndAccept(
  responseType: string,
  responseTypeWithNamespace: string,
  contentTypes: string[] | undefined,
  isRemoteStreamFlag: boolean | undefined,
): { httpResponseType?: 'json' | 'text' | 'blob' | 'arraybuffer'; acceptHeader?: string } {
  if (isRemoteStreamFlag || isRemoteStreamContent(responseTypeWithNamespace)) {
    return { httpResponseType: 'blob', acceptHeader: 'application/octet-stream' };
  }

  if (contentTypes && contentTypes.length > 0) {
    const normalized = contentTypes.map(normalizeMediaType);

    const firstJsonShaped = normalized.find(isJsonMediaType);
    if (firstJsonShaped) {
      return { httpResponseType: 'json', acceptHeader: firstJsonShaped };
    }
    if (normalized.every(ct => ct.startsWith('text/'))) {
      return { httpResponseType: 'text', acceptHeader: normalized[0] };
    }
    if (normalized.every(isBinaryMediaType)) {
      return { httpResponseType: 'blob', acceptHeader: normalized[0] };
    }
    return { acceptHeader: normalized[0] };
  }

  if (responseType === 'string') {
    return { httpResponseType: 'text' };
  }

  return {};
}

export function createActionToSignatureMapper() {
  const adaptType = createTypeAdapter();

  return (action: Action) => {
    const signature = new Signature({ name: getMethodNameFromAction(action) });
    const versionParameter = getVersionParameter(action);
    const restConfig = new Property({ name: 'config', type: 'Partial<Rest.Config>' });
    restConfig.setOptional(true);
    const parameters = [
      ...action.parametersOnMethod,
      ...(versionParameter ? [versionParameter] : []),
    ];

    const uploadMethodArgNames = new Set(
      (action.parameters ?? [])
        .filter(p => p.bindingSourceId === eBindingSourceId.FormFile)
        .map(p => p.nameOnMethod),
    );

    signature.parameters = parameters.map(p => {
      if (uploadMethodArgNames.has(p.name)) {
        return new Property({ name: p.name, type: 'FormData' });
      }
      const isFormData = isRemoteStreamContent(p.type);
      const isFormArray = isRemoteStreamContentArray(p.type);
      if (isFormData || isFormArray) {
        return new Property({ name: p.name, type: 'FormData' });
      }

      let type = adaptType(p.typeSimple);
      if (p.typeSimple === 'enum' || p.typeSimple === '[enum]' || p.typeSimple === 'enum?' || p.typeSimple === '[enum]?') {
        type = adaptType(p.type);
      }

      // Array params are only forwarded to the HTTP client and never mutated, so declare them
      // `readonly` to also accept readonly arrays from callers without a type error.
      if (type.endsWith('[]')) {
        type = `readonly ${type}`;
      }

      const parameter = new Property({ name: p.name, type });
      parameter.setDefault(p.defaultValue);
      parameter.setOptional(p.isOptional);
      return parameter;
    });
    signature.parameters.push(restConfig);

    return signature;
  };
}

export function isRemoteStreamContent(type: string) {
  return VOLO_REMOTE_STREAM_CONTENT.some(x => x === type);
}

export function isRemoteStreamContentArray(type: string) {
  if (VOLO_REMOTE_STREAM_CONTENT.map(x => `${x}[]`).some(x => x === type)) {
    return true;
  }

  // ABP serialises collections as `[T]` (see ApiTypeNameHelper.GetSimpleTypeName).
  if (type.startsWith('[') && type.endsWith(']')) {
    const inner = type.slice(1, -1);
    if (VOLO_REMOTE_STREAM_CONTENT.includes(inner)) {
      return true;
    }
  }

  if (isCollectionType(type)) {
    const { generics } = extractGenerics(type);
    if (generics.length > 0 && VOLO_REMOTE_STREAM_CONTENT.includes(generics[0])) {
      return true;
    }
  }

  return false;
}

function getMethodNameFromAction(action: Action): string {
  return action.uniqueName.split('Async')[0];
}

function getVersionParameter(action: Action) {
  const versionParameter = action.parameters.find(
    p =>
      (p.name == 'apiVersion' && p.bindingSourceId == eBindingSourceId.Path) ||
      (p.name == 'api-version' && p.bindingSourceId == eBindingSourceId.Query),
  );
  const bestVersion = findBestApiVersion(action);
  return versionParameter && bestVersion
    ? {
        ...versionParameter,
        name: camelizeHyphen(versionParameter.name),
        defaultValue: `"${bestVersion}"`,
      }
    : null;
}

// Implementation of https://github.com/abpframework/abp/commit/c3f77c1229508279015054a9b4f5586404a88a14#diff-a4dbf6be9a1aa21d8294f11047774949363ee6b601980bf3225e8046c0748c9eR101
function findBestApiVersion(action: Action) {
  /*
  TODO: Implement  configuredVersion when js proxies implemented
  let configuredVersion = null;
   if (action.supportedVersions.includes(configuredVersion)) {
    return configuredVersion;
  }
  */

  if (!action.supportedVersions?.length) {
    // TODO: return configuredVersion if exists or '1.0'
    return '1.0';
  }
  //TODO: Ensure to get the latest version!
  return action.supportedVersions[action.supportedVersions.length - 1];
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
              if (types[type]) {
                acc.push({ type, isEnum: types[type].isEnum });
              }
            }),
        );

        return acc;
      }, []),
    );
}
