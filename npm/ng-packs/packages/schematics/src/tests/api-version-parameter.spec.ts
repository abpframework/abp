import { readFileSync } from 'fs';
import { join } from 'path';
import { template as lodashTemplate } from 'lodash';
import * as ts from 'typescript';
import { beforeAll, describe, expect, test } from 'vitest';
import { eBindingSourceId } from '../enums';
import { Action, ParameterInBody } from '../models';
import {
  createActionToBodyMapper,
  createActionToMethodMapper,
  createActionToSignatureMapper,
  serializeParameters,
} from '../utils/service';

const TEMPLATE_PATH = join(
  __dirname,
  '..',
  'commands',
  'api',
  'files-service',
  'proxy',
  '__namespace@dir__',
  '__name@kebab__.service.ts.template',
);

function parameter(overrides: Partial<ParameterInBody>): ParameterInBody {
  return {
    nameOnMethod: overrides.name!,
    jsonName: null,
    type: 'System.String',
    typeSimple: 'string',
    isOptional: false,
    defaultValue: null,
    constraintTypes: null,
    bindingSourceId: eBindingSourceId.Query,
    descriptorName: '',
    ...overrides,
  } as ParameterInBody;
}

function inputDtoParameter(name: string, bindingSourceId = eBindingSourceId.Model) {
  return parameter({ name, nameOnMethod: 'input', bindingSourceId, descriptorName: 'input' });
}

function methodParameter(name: string, typeSimple: string) {
  return {
    name,
    typeAsString: typeSimple,
    type: typeSimple,
    typeSimple,
    isOptional: false,
    defaultValue: null,
  };
}

const inputOnMethod = methodParameter('input', 'GetBookListInput');
const idOnMethod = methodParameter('id', 'string');

function buildAction(overrides: Partial<Action>): Action {
  return {
    uniqueName: 'GetListAsync',
    name: 'GetListAsync',
    httpMethod: 'GET',
    url: 'api/app/book',
    supportedVersions: ['2.0'],
    parametersOnMethod: [],
    parameters: [],
    returnValue: { type: 'System.String', typeSimple: 'string' },
    ...overrides,
  } as Action;
}

const queryVersionWithInputDto = buildAction({
  parametersOnMethod: [inputOnMethod],
  parameters: [
    inputDtoParameter('Filter'),
    inputDtoParameter('MaxResultCount'),
    parameter({ name: 'api-version', nameOnMethod: 'input' }),
  ],
});

const queryVersionWithSimpleParameters = buildAction({
  uniqueName: 'GetAsync',
  url: 'api/app/book/{id}',
  parametersOnMethod: [idOnMethod],
  parameters: [
    parameter({ name: 'id', bindingSourceId: eBindingSourceId.Path }),
    parameter({ name: 'api-version' }),
  ],
});

const queryVersionWithBody = buildAction({
  uniqueName: 'CreateAsync',
  httpMethod: 'POST',
  parametersOnMethod: [methodParameter('input', 'CreateBookDto')],
  parameters: [
    parameter({
      name: 'input',
      type: 'CreateBookDto',
      typeSimple: 'CreateBookDto',
      bindingSourceId: eBindingSourceId.Body,
    }),
    parameter({ name: 'api-version' }),
  ],
});

const queryVersionWithoutParameters = buildAction({
  uniqueName: 'GetCountAsync',
  url: 'api/app/book/count',
  parameters: [parameter({ name: 'api-version' })],
});

const urlSegmentVersionWithInputDto = buildAction({
  uniqueName: 'GetPagedAsync',
  url: 'api/v{apiVersion}/book',
  parametersOnMethod: [inputOnMethod],
  parameters: [
    inputDtoParameter('Filter', eBindingSourceId.Query),
    parameter({
      name: 'apiVersion',
      nameOnMethod: 'input',
      bindingSourceId: eBindingSourceId.Path,
    }),
  ],
});

const urlSegmentVersionWithSimpleParameters = buildAction({
  uniqueName: 'GetByIdAsync',
  url: 'api/v{apiVersion}/book/{id}',
  parametersOnMethod: [idOnMethod],
  parameters: [
    parameter({ name: 'id', bindingSourceId: eBindingSourceId.Path }),
    parameter({ name: 'apiVersion', bindingSourceId: eBindingSourceId.Path }),
  ],
});

const versionWithoutSupportedVersions = buildAction({
  uniqueName: 'GetLegacyAsync',
  url: 'api/app/book/legacy',
  supportedVersions: [],
  parameters: [parameter({ name: 'api-version' })],
});

describe('api version parameter mapping', () => {
  const mapSignature = createActionToSignatureMapper();
  const mapBody = createActionToBodyMapper();

  test('query string version with an input DTO is bound to the apiVersion argument', () => {
    expect(serializeParameters(mapSignature(queryVersionWithInputDto).parameters)).toBe(
      'input: GetBookListInput, apiVersion: string = "2.0", config?: Partial<Rest.Config>',
    );
    expect(mapBody(queryVersionWithInputDto).params).toEqual([
      'filter: input.filter',
      'maxResultCount: input.maxResultCount',
      '["api-version"]: apiVersion',
    ]);
  });

  test('query string version with simple parameters is bound to the apiVersion argument', () => {
    expect(serializeParameters(mapSignature(queryVersionWithSimpleParameters).parameters)).toBe(
      'id: string, apiVersion: string = "2.0", config?: Partial<Rest.Config>',
    );
    const body = mapBody(queryVersionWithSimpleParameters);
    expect(body.url).toBe('`/api/app/book/${id}`');
    expect(body.params).toEqual(['["api-version"]: apiVersion']);
  });

  test('query string version with a body keeps the body bound to the input', () => {
    expect(serializeParameters(mapSignature(queryVersionWithBody).parameters)).toBe(
      'input: CreateBookDto, apiVersion: string = "2.0", config?: Partial<Rest.Config>',
    );
    const body = mapBody(queryVersionWithBody);
    expect(body.body).toBe('input');
    expect(body.params).toEqual(['["api-version"]: apiVersion']);
  });

  test('query string version without other parameters is bound to the apiVersion argument', () => {
    expect(serializeParameters(mapSignature(queryVersionWithoutParameters).parameters)).toBe(
      'apiVersion: string = "2.0", config?: Partial<Rest.Config>',
    );
    expect(mapBody(queryVersionWithoutParameters).params).toEqual(['["api-version"]: apiVersion']);
  });

  test('url segment version with an input DTO is bound to the apiVersion argument', () => {
    expect(serializeParameters(mapSignature(urlSegmentVersionWithInputDto).parameters)).toBe(
      'input: GetBookListInput, apiVersion: string = "2.0", config?: Partial<Rest.Config>',
    );
    const body = mapBody(urlSegmentVersionWithInputDto);
    expect(body.url).toBe('`/api/v${apiVersion}/book`');
    expect(body.params).toEqual(['filter: input.filter']);
  });

  test('uses the latest supported version as the default value', () => {
    const action = buildAction({
      supportedVersions: ['1.0', '2.0', '3.0'],
      parameters: [parameter({ name: 'api-version' })],
    });

    expect(serializeParameters(mapSignature(action).parameters)).toBe(
      'apiVersion: string = "3.0", config?: Partial<Rest.Config>',
    );
  });

  test('falls back to 1.0 when the action has no supported versions', () => {
    expect(serializeParameters(mapSignature(versionWithoutSupportedVersions).parameters)).toBe(
      'apiVersion: string = "1.0", config?: Partial<Rest.Config>',
    );
    expect(mapBody(versionWithoutSupportedVersions).params).toEqual([
      '["api-version"]: apiVersion',
    ]);
  });

  test('leaves actions without a version parameter unchanged', () => {
    const action = buildAction({
      parametersOnMethod: [inputOnMethod],
      parameters: [inputDtoParameter('Filter')],
    });

    expect(serializeParameters(mapSignature(action).parameters)).toBe(
      'input: GetBookListInput, config?: Partial<Rest.Config>',
    );
    expect(mapBody(action).params).toEqual(['filter: input.filter']);
  });
});

interface CapturedRequest {
  method: string;
  url: string;
  params?: Record<string, unknown>;
  body?: unknown;
}

const GENERATED_SERVICE_PATH = '/proxy/book.service.ts';

const VIRTUAL_FILES: Record<string, string> = {
  '/proxy/models.ts': `
    export interface GetBookListInput { filter?: string; maxResultCount?: number; }
    export interface CreateBookDto { name: string; }
  `,
  '/proxy/ambient.d.ts': `
    declare module '@abp/ng.core' {
      export namespace Rest {
        export interface Config { apiName?: string; [key: string]: any; }
      }
      export class RestService {
        request<TBody, TResponse>(req: any, config?: any): import('rxjs').Observable<TResponse>;
      }
    }
    declare module '@angular/core' {
      export function Injectable(opts?: any): ClassDecorator;
      export function inject<T>(token: { new (...args: any[]): T }): T;
    }
    declare module 'rxjs' {
      export class Observable<T> { subscribe(...args: any[]): unknown; }
    }
  `,
};

function renderService(actions: Action[]) {
  const mapMethod = createActionToMethodMapper();
  return lodashTemplate(readFileSync(TEMPLATE_PATH, 'utf8'), {
    imports: {
      camel: (s: string) => s.charAt(0).toLowerCase() + s.slice(1),
      serializeParameters,
    },
  })({
    apiName: 'Default',
    name: 'Book',
    resourceApi: false,
    imports: [
      {
        keyword: 'import type',
        specifiers: ['CreateBookDto', 'GetBookListInput'],
        path: './models',
      },
      { keyword: 'import', specifiers: ['RestService', 'Rest'], path: '@abp/ng.core' },
      { keyword: 'import', specifiers: ['Injectable', 'inject'], path: '@angular/core' },
    ],
    methods: actions.map(mapMethod),
  });
}

function typeCheck(source: string) {
  const sources: Record<string, string> = { ...VIRTUAL_FILES, [GENERATED_SERVICE_PATH]: source };
  const compilerOptions: ts.CompilerOptions = {
    target: ts.ScriptTarget.ES2022,
    module: ts.ModuleKind.ES2022,
    moduleResolution: ts.ModuleResolutionKind.Bundler,
    experimentalDecorators: true,
    strict: true,
    noEmit: true,
    skipLibCheck: true,
  };
  const baseHost = ts.createCompilerHost(compilerOptions, true);
  const host: ts.CompilerHost = {
    ...baseHost,
    getSourceFile: (fileName, languageVersion, onError) =>
      sources[fileName] != null
        ? ts.createSourceFile(fileName, sources[fileName], languageVersion, true)
        : baseHost.getSourceFile(fileName, languageVersion, onError),
    fileExists: fileName => sources[fileName] != null || baseHost.fileExists(fileName),
    readFile: fileName => sources[fileName] ?? baseHost.readFile(fileName),
    directoryExists: directoryName =>
      directoryName === '/proxy' || (baseHost.directoryExists?.(directoryName) ?? false),
  };

  const program = ts.createProgram(Object.keys(sources), compilerOptions, host);
  return ts
    .getPreEmitDiagnostics(program)
    .filter(d => d.category === ts.DiagnosticCategory.Error)
    .map(d => `TS${d.code}: ${ts.flattenDiagnosticMessageText(d.messageText, '\n')}`);
}

// Renders the real service template, type checks it and runs the generated methods against a fake
// RestService, so the test fails on code that does not compile or sends the wrong version.
function createGeneratedService(actions: Action[]) {
  const source = renderService(actions);
  const errors = typeCheck(source);
  const requests: CapturedRequest[] = [];
  if (errors.length) {
    return { errors, requests, service: undefined };
  }

  const restService = { request: (request: CapturedRequest) => requests.push(request) };
  const modules: Record<string, unknown> = {
    '@abp/ng.core': { RestService: class {} },
    '@angular/core': { Injectable: () => (target: unknown) => target, inject: () => restService },
  };
  const { outputText } = ts.transpileModule(source, {
    compilerOptions: {
      target: ts.ScriptTarget.ES2022,
      module: ts.ModuleKind.CommonJS,
      experimentalDecorators: true,
    },
  });
  const module = { exports: {} as Record<string, new () => Record<string, Function>> };
  new Function('module', 'exports', 'require', outputText)(
    module,
    module.exports,
    (name: string) => modules[name],
  );

  return { errors, requests, service: new module.exports.BookService() };
}

describe('api version parameter in the generated service', () => {
  let generated: ReturnType<typeof createGeneratedService>;
  let service: Record<string, Function>;

  beforeAll(() => {
    generated = createGeneratedService([
      queryVersionWithInputDto,
      queryVersionWithSimpleParameters,
      queryVersionWithBody,
      queryVersionWithoutParameters,
      urlSegmentVersionWithInputDto,
      urlSegmentVersionWithSimpleParameters,
      versionWithoutSupportedVersions,
    ]);
    service = generated.service!;
  });

  const input = { filter: 'f', maxResultCount: 5 };
  const lastRequest = () => generated.requests[generated.requests.length - 1];

  test('type checks', () => {
    expect(generated.errors).toEqual([]);
  });

  test.each([
    ['getList', [input], '2.0'],
    ['get', ['1'], '2.0'],
    ['create', [{ name: 'n' }], '2.0'],
    ['getCount', [], '2.0'],
    ['getLegacy', [], '1.0'],
  ])('%s sends the default query string version', (method, args, version) => {
    service[method](...args);
    expect(lastRequest().params!['api-version']).toBe(version);
  });

  test.each([
    ['getList', [input]],
    ['get', ['1']],
    ['create', [{ name: 'n' }]],
    ['getCount', []],
    ['getLegacy', []],
  ])('%s sends the query string version passed by the caller', (method, args) => {
    service[method](...args, '3.0');
    expect(lastRequest().params!['api-version']).toBe('3.0');
  });

  test('getList sends the input DTO properties and keeps the input unchanged', () => {
    service.getList(input);
    expect(lastRequest().params).toEqual({
      filter: 'f',
      maxResultCount: 5,
      'api-version': '2.0',
    });
    expect(input).toEqual({ filter: 'f', maxResultCount: 5 });
  });

  test('create sends the input as the body', () => {
    const dto = { name: 'n' };
    service.create(dto);
    expect(lastRequest()).toMatchObject({ method: 'POST', body: dto });
  });

  test.each([
    ['getPaged', [input], '/api/v2.0/book'],
    ['getById', ['1'], '/api/v2.0/book/1'],
  ])('%s puts the default version into the url', (method, args, url) => {
    service[method](...args);
    expect(lastRequest().url).toBe(url);
  });

  test.each([
    ['getPaged', [input], '/api/v3.0/book'],
    ['getById', ['1'], '/api/v3.0/book/1'],
  ])('%s puts the version passed by the caller into the url', (method, args, url) => {
    service[method](...args, '3.0');
    expect(lastRequest().url).toBe(url);
  });
});
