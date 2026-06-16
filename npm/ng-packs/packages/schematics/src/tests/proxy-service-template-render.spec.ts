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
});
