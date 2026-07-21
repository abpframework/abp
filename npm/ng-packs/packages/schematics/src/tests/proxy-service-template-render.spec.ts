import { readFileSync } from 'fs';
import { join } from 'path';
import { template as lodashTemplate } from 'lodash';
import { describe, expect, test } from 'vitest';

/**
 * Smoke test that actually renders the proxy `.service.ts.template` against
 * representative body configurations and asserts the emitted code matches
 * what the runtime contract requires.
 *
 * This catches template-syntax / control-flow regressions that a string
 * `toContain` check on the template source would silently let through.
 */

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

const TEMPLATE_SRC = readFileSync(TEMPLATE_PATH, 'utf8');

function render(context: Record<string, unknown>): string {
  const compiled = lodashTemplate(TEMPLATE_SRC, {
    imports: {
      camel: (s: string) => s.charAt(0).toLowerCase() + s.slice(1),
      serializeParameters: (params: Array<{ name: string; type: string; default?: string }>) =>
        params.map(p => `${p.name}: ${p.type}`).join(', '),
    },
  });
  return compiled(context);
}

function buildContext(body: Partial<MockBody>) {
  return {
    apiName: 'Default',
    name: 'Sample',
    namespace: 'app',
    imports: [
      { keyword: 'import', specifiers: ['RestService', 'Rest'], path: '@abp/ng.core' },
      { keyword: 'import', specifiers: ['Injectable', 'inject'], path: '@angular/core' },
    ],
    methods: [
      {
        body: makeBody(body),
        signature: {
          name: 'GetSampleAsync',
          parameters: [],
        },
      },
    ],
  };
}

interface MockBody {
  method: string;
  url: string;
  responseType: string;
  responseTypeWithNamespace: string;
  httpResponseType?: string;
  acceptHeader?: string;
  body?: string;
  params: string[];
  dictParamVar?: string;
  requestType: string;
  isBlobMethod(): boolean;
}

function makeBody(overrides: Partial<MockBody>): MockBody {
  return {
    method: 'GET',
    url: "'/api/sample'",
    responseType: 'any',
    responseTypeWithNamespace: 'any',
    httpResponseType: undefined,
    acceptHeader: undefined,
    body: undefined,
    params: [],
    dictParamVar: undefined,
    requestType: 'any',
    isBlobMethod: () => false,
    ...overrides,
  };
}

describe('proxy service template — rendered output', () => {
  test('default JSON body emits no responseType and no Accept header', () => {
    const output = render(buildContext({
      responseType: 'MyDto',
      responseTypeWithNamespace: 'My.Project.MyDto',
    }));

    expect(output).toContain("method: 'GET'");
    expect(output).not.toContain('responseType:');
    expect(output).not.toContain('headers:');
  });

  test('json httpResponseType emits Accept but no responseType (default is json)', () => {
    const output = render(buildContext({
      responseType: 'string',
      responseTypeWithNamespace: 'string',
      httpResponseType: 'json',
      acceptHeader: 'application/json',
    }));

    expect(output).toContain("headers: { Accept: 'application/json' }");
    expect(output).not.toContain('responseType:');
  });

  test('text httpResponseType emits both responseType and Accept', () => {
    const output = render(buildContext({
      responseType: 'string',
      responseTypeWithNamespace: 'string',
      httpResponseType: 'text',
      acceptHeader: 'text/plain',
    }));

    expect(output).toContain("responseType: 'text'");
    expect(output).toContain("headers: { Accept: 'text/plain' }");
  });

  test('blob (IRemoteStreamContent) emits Blob return type + responseType + Accept', () => {
    const output = render(buildContext({
      responseType: 'Volo.Abp.Content.IRemoteStreamContent',
      responseTypeWithNamespace: 'Volo.Abp.Content.IRemoteStreamContent',
      isBlobMethod: () => true,
      httpResponseType: 'blob',
      acceptHeader: 'application/octet-stream',
    }));

    expect(output).toContain("responseType: 'blob'");
    expect(output).toContain('Blob>');
    expect(output).toContain("headers: { Accept: 'application/octet-stream' }");
  });

  test('IRemoteStreamContent[] degradation renders any[] return type and does not reference IRemoteStreamContent', () => {
    const output = render(buildContext({
      responseType: 'any[]',
      responseTypeWithNamespace: '[Volo.Abp.Content.IRemoteStreamContent]',
      isBlobMethod: () => false,
      httpResponseType: undefined,
      acceptHeader: undefined,
    }));

    expect(output).toContain('any[]');
    expect(output).not.toContain('IRemoteStreamContent');
    expect(output).not.toContain("responseType: 'blob'");
  });

  test('arraybuffer httpResponseType emits responseType', () => {
    const output = render(buildContext({
      responseType: 'ArrayBuffer',
      responseTypeWithNamespace: 'ArrayBuffer',
      httpResponseType: 'arraybuffer',
      acceptHeader: 'application/octet-stream',
    }));

    expect(output).toContain("responseType: 'arraybuffer'");
    expect(output).toContain("headers: { Accept: 'application/octet-stream' }");
  });

  test('no acceptHeader → no headers line', () => {
    const output = render(buildContext({
      responseType: 'string',
      responseTypeWithNamespace: 'string',
      httpResponseType: 'text',
      acceptHeader: undefined,
    }));

    expect(output).toContain("responseType: 'text'");
    expect(output).not.toContain('headers:');
  });

  test('rendered service code is valid TypeScript-shaped (closing braces / semicolons)', () => {
    const output = render(buildContext({
      responseType: 'string',
      responseTypeWithNamespace: 'string',
      httpResponseType: 'json',
      acceptHeader: 'application/json',
    }));

    expect(output).toContain('@Injectable({');
    expect(output).toContain('providedIn: \'root\'');
    expect(output).toContain('export class SampleService');
    expect(output).toContain('this.restService.request<any,');
    expect(output.match(/}/g)!.length).toBeGreaterThanOrEqual(3);
  });

  test.each([
    { name: 'string + json Accept', body: { responseType: 'string', responseTypeWithNamespace: 'string', httpResponseType: 'json', acceptHeader: 'application/problem+json' } },
    { name: 'string + text Accept', body: { responseType: 'string', responseTypeWithNamespace: 'string', httpResponseType: 'text', acceptHeader: 'text/csv' } },
    { name: 'blob + pdf Accept', body: { responseType: 'Blob', responseTypeWithNamespace: 'Blob', httpResponseType: 'blob', acceptHeader: 'application/pdf', isBlobMethod: () => true } },
    { name: 'any[] degradation', body: { responseType: 'any[]', responseTypeWithNamespace: '[Volo.Abp.Content.IRemoteStreamContent]' } },
    { name: 'xml Accept only', body: { responseType: 'any', responseTypeWithNamespace: 'any', acceptHeader: 'application/xml' } },
  ])('rendered service compiles cleanly under real ts.Program ($name)', ({ body }) => {
    const ts = require('typescript');
    const ctx = buildContext(body as Partial<MockBody>);
    ctx.methods[0].signature.parameters = [{ name: 'config', type: 'Record<string, any>' } as any];
    const output = render(ctx);

    const abpStub = `
      declare module '@abp/ng.core' {
        export namespace Rest {
          export interface Config {
            apiName?: string;
            observe?: any;
            skipHandleError?: boolean;
            responseType?: string;
            [key: string]: any;
          }
          export type Observe = any;
        }
        export class RestService {
          request<TBody, TResponse>(req: any, config?: any): import('rxjs').Observable<TResponse>;
        }
      }
    `;
    const domStub = 'declare class Blob { constructor(parts?: any[], options?: any); }\n';
    const angularCoreStub = `
      declare module '@angular/core' {
        export function Injectable(opts?: any): ClassDecorator;
        export function inject<T>(token: { new (...args: any[]): T }): T;
        export function inject<T>(token: any): T;
      }
    `;
    const rxjsStub = `
      declare module 'rxjs' {
        export class Observable<T> { subscribe(...args: any[]): unknown; }
      }
    `;

    const ambient = abpStub + angularCoreStub + rxjsStub + domStub;
    const sources: Record<string, string> = {
      '/proxy/sample.service.ts': output,
      '/proxy/ambient.d.ts': ambient,
    };

    const compilerOptions: any = {
      target: ts.ScriptTarget.ES2020,
      module: ts.ModuleKind.ES2020,
      moduleResolution: ts.ModuleResolutionKind.NodeJs,
      experimentalDecorators: true,
      emitDecoratorMetadata: true,
      strict: true,
      noEmit: true,
      skipLibCheck: true,
    };

    const baseHost = ts.createCompilerHost(compilerOptions, true);
    const host: any = {
      ...baseHost,
      getSourceFile: (fileName: string, languageVersion: any, onError: any) => {
        if (sources[fileName]) {
          return ts.createSourceFile(fileName, sources[fileName], languageVersion, true);
        }
        return baseHost.getSourceFile(fileName, languageVersion, onError);
      },
      fileExists: (fileName: string) =>
        sources[fileName] != null || baseHost.fileExists(fileName),
      readFile: (fileName: string) =>
        sources[fileName] ?? baseHost.readFile(fileName),
    };

    const program = ts.createProgram(Object.keys(sources), compilerOptions, host);
    const errors = ts
      .getPreEmitDiagnostics(program)
      .filter((d: any) =>
        d.category === ts.DiagnosticCategory.Error &&
        d.code !== 6053,
      );

    if (errors.length) {
      const messages = errors
        .map((d: any) => {
          const where = d.file
            ? (() => {
                const p = d.file.getLineAndCharacterOfPosition(d.start ?? 0);
                const lineText = d.file.text.split('\n')[p.line];
                return `${d.file.fileName}:${p.line + 1}:${p.character + 1}\n>>> ${lineText}\n>>> ${' '.repeat(p.character)}^`;
              })()
            : '(no file)';
          return `[${where}] TS${d.code}: ${ts.flattenDiagnosticMessageText(d.messageText, '\n')}`;
        })
        .join('\n---\n');
      throw new Error(`Generated proxy did not compile:\n${output}\n=== diagnostics ===\n${messages}`);
    }
    expect(errors).toHaveLength(0);
  });

  test.each([
    {
      name: 'DTO upload — single FormData arg',
      signatureParams: [
        { name: 'input', type: 'FormData' },
        { name: 'config', type: 'Record<string, any>' },
      ],
      bodyOverrides: { method: 'POST', url: "'/api/test/upload-single'", body: 'input' },
      shouldContain: ['input: FormData', 'body: input'],
    },
    {
      name: 'direct upload — FormData arg with custom name',
      signatureParams: [
        { name: 'file', type: 'FormData' },
        { name: 'config', type: 'Record<string, any>' },
      ],
      bodyOverrides: { method: 'POST', url: "'/api/test/upload-direct'", body: 'file' },
      shouldContain: ['file: FormData', 'body: file'],
    },
    {
      name: 'path + upload mixed — id stays in URL, FormData becomes body',
      signatureParams: [
        { name: 'id', type: 'number' },
        { name: 'input', type: 'FormData' },
        { name: 'config', type: 'Record<string, any>' },
      ],
      bodyOverrides: { method: 'POST', url: '`/api/test/upload-with-path/${id}`', body: 'input' },
      shouldContain: ['id: number', 'input: FormData', 'body: input'],
    },
    {
      name: 'query + upload mixed — tag in params, FormData in body',
      signatureParams: [
        { name: 'tag', type: 'string' },
        { name: 'input', type: 'FormData' },
        { name: 'config', type: 'Record<string, any>' },
      ],
      bodyOverrides: {
        method: 'POST',
        url: "'/api/test/upload-with-query'",
        params: ['tag'],
        body: 'input',
      },
      shouldContain: ['tag: string', 'input: FormData', 'params: { tag }', 'body: input'],
    },
  ])('upload action signature collapses to FormData ($name)', ({ signatureParams, bodyOverrides, shouldContain }) => {
    const ts = require('typescript');
    const ctx = buildContext({
      responseType: 'string',
      responseTypeWithNamespace: 'string',
      ...bodyOverrides,
    } as Partial<MockBody>);
    ctx.methods[0].signature.parameters = signatureParams as any;
    const output = render(ctx);

    for (const fragment of shouldContain) {
      expect(output).toContain(fragment);
    }
    expect(output).not.toContain('JSON.stringify');

    const abpStub = `
      declare module '@abp/ng.core' {
        export namespace Rest {
          export interface Config {
            apiName?: string;
            observe?: any;
            skipHandleError?: boolean;
            responseType?: string;
            [key: string]: any;
          }
          export type Observe = any;
        }
        export class RestService {
          request<TBody, TResponse>(req: any, config?: any): import('rxjs').Observable<TResponse>;
        }
      }
    `;
    const angularCoreStub = `
      declare module '@angular/core' {
        export function Injectable(opts?: any): ClassDecorator;
        export function inject<T>(token: { new (...args: any[]): T }): T;
        export function inject<T>(token: any): T;
      }
    `;
    const rxjsStub = `
      declare module 'rxjs' {
        export class Observable<T> { subscribe(...args: any[]): unknown; }
      }
    `;
    const domStub = `
      declare class Blob { constructor(parts?: any[], options?: any); }
      declare class FormData {
        constructor();
        append(name: string, value: string | Blob, fileName?: string): void;
        get(name: string): any;
      }
    `;
    const ambient = abpStub + angularCoreStub + rxjsStub + domStub;
    const sources: Record<string, string> = {
      '/proxy/sample.service.ts': output,
      '/proxy/ambient.d.ts': ambient,
    };
    const compilerOptions: any = {
      target: ts.ScriptTarget.ES2020,
      module: ts.ModuleKind.ES2020,
      moduleResolution: ts.ModuleResolutionKind.NodeJs,
      experimentalDecorators: true,
      emitDecoratorMetadata: true,
      strict: true,
      noEmit: true,
      skipLibCheck: true,
    };
    const baseHost = ts.createCompilerHost(compilerOptions, true);
    const host: any = {
      ...baseHost,
      getSourceFile: (fileName: string, languageVersion: any, onError: any) =>
        sources[fileName]
          ? ts.createSourceFile(fileName, sources[fileName], languageVersion, true)
          : baseHost.getSourceFile(fileName, languageVersion, onError),
      fileExists: (fileName: string) =>
        sources[fileName] != null || baseHost.fileExists(fileName),
      readFile: (fileName: string) =>
        sources[fileName] ?? baseHost.readFile(fileName),
    };
    const program = ts.createProgram(Object.keys(sources), compilerOptions, host);
    const errors = ts
      .getPreEmitDiagnostics(program)
      .filter((d: any) => d.category === ts.DiagnosticCategory.Error && d.code !== 6053);
    if (errors.length) {
      const messages = errors
        .map((d: any) => {
          const where = d.file
            ? (() => {
                const p = d.file.getLineAndCharacterOfPosition(d.start ?? 0);
                const lineText = d.file.text.split('\n')[p.line];
                return `${d.file.fileName}:${p.line + 1}:${p.character + 1}\n>>> ${lineText}\n>>> ${' '.repeat(p.character)}^`;
              })()
            : '(no file)';
          return `[${where}] TS${d.code}: ${ts.flattenDiagnosticMessageText(d.messageText, '\n')}`;
        })
        .join('\n---\n');
      throw new Error(`Upload action proxy did not compile:\n${output}\n=== diagnostics ===\n${messages}`);
    }
    expect(errors).toHaveLength(0);
  });

  test('rendered upload service forwards FormData to restService.request at runtime', () => {
    const ts = require('typescript');
    const ctx = buildContext({
      method: 'POST',
      url: "'/api/upload-runtime'",
      responseType: 'string',
      responseTypeWithNamespace: 'string',
      body: 'input',
    });
    ctx.methods[0].signature.parameters = [
      { name: 'input', type: 'FormData' },
      { name: 'config', type: 'Record<string, any>' },
    ] as any;
    const output = render(ctx);

    const stripped = output
      .replace(/^import .*?;\s*$/gm, '')
      .replace(/@Injectable\(\{[\s\S]*?\}\)\s*\n/g, '')
      .replace(/private restService = inject\(RestService\);/, 'restService;')
      .replace(/this\.restService\.request<[^>]+,\s*[^>]+>/g, 'this.restService.request');

    const transpiled = ts.transpileModule(stripped, {
      compilerOptions: {
        target: ts.ScriptTarget.ES2020,
        module: ts.ModuleKind.CommonJS,
        experimentalDecorators: true,
      },
    }).outputText;

    const restMockCalls: Array<{ body: any; method: string; url: string; headers?: any }> = [];
    const restMock = {
      request: (req: any /* , _config: any */) => {
        restMockCalls.push(req);
        return { subscribe: () => undefined };
      },
    };

    const vm = require('vm');
    const sandbox: Record<string, any> = { exports: {} };
    vm.createContext(sandbox);
    vm.runInContext(transpiled + '\nexports.SampleService = SampleService;', sandbox);
    const ServiceCls = sandbox.exports.SampleService;
    const instance = new ServiceCls();
    instance.restService = restMock;

    const GlobalFormData = (globalThis as any).FormData;
    const formData = typeof GlobalFormData === 'function'
      ? new GlobalFormData()
      : { __isFormData: true, append: () => undefined };
    instance.getSampleAsync(formData, { apiName: 'Default' });

    expect(restMockCalls).toHaveLength(1);
    expect(restMockCalls[0].body).toBe(formData);
    expect(restMockCalls[0].method).toBe('POST');
  });
});
