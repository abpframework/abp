import { readFileSync } from 'fs';
import { join } from 'path';
import { describe, expect, test } from 'vitest';
import { eBindingSourceId } from '../enums';
import { Action } from '../models';
import { createActionToBodyMapper } from '../utils/service';

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

function buildAction(overrides: Partial<Action>): Action {
  return {
    uniqueName: 'GetStatusAsync',
    name: 'GetStatus',
    httpMethod: 'GET',
    url: 'api/app/test-service/status',
    supportedVersions: [],
    parametersOnMethod: [],
    parameters: [],
    returnValue: { type: 'System.String', typeSimple: 'string' },
    ...overrides,
  } as Action;
}

describe('createActionToBodyMapper — string return value', () => {
  const mapBody = createActionToBodyMapper();

  test('without contentTypes falls back to text mode (legacy behavior)', () => {
    const body = mapBody(buildAction({}));

    expect(body.responseType).toBe('string');
    expect(body.httpResponseType).toBe('text');
    expect(body.acceptHeader).toBeUndefined();
  });

  test('with contentTypes containing application/json picks json + Accept: application/json', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['application/json', 'text/plain'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
    expect(body.acceptHeader).toBe('application/json');
  });

  test('with only text/* contentTypes picks text + Accept: text/plain', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['text/plain', 'text/csv'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('text');
    expect(body.acceptHeader).toBe('text/plain');
  });
});

describe('createActionToBodyMapper — IRemoteStreamContent return value', () => {
  const mapBody = createActionToBodyMapper();

  test('always picks blob + Accept: application/octet-stream regardless of contentTypes', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'Volo.Abp.Content.IRemoteStreamContent',
          typeSimple: 'Volo.Abp.Content.IRemoteStreamContent',
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
    expect(body.acceptHeader).toBe('application/octet-stream');
    expect(body.isBlobMethod()).toBe(true);
  });

  test('binary-only contentTypes picks blob and echoes back the actual binary media type', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.Byte[]',
          typeSimple: 'byte[]',
          contentTypes: ['application/pdf'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
    expect(body.acceptHeader).toBe('application/pdf');
  });
});

describe('createActionToBodyMapper — other return values', () => {
  const mapBody = createActionToBodyMapper();

  test('object return without contentTypes has no httpResponseType / acceptHeader (defaults to json)', () => {
    const body = mapBody(
      buildAction({
        returnValue: { type: 'My.Project.UserDto', typeSimple: 'My.Project.UserDto' },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBeUndefined();
    expect(body.acceptHeader).toBeUndefined();
  });

  test('void return has no httpResponseType / acceptHeader', () => {
    const body = mapBody(
      buildAction({
        returnValue: { type: 'System.Void', typeSimple: 'void' },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBeUndefined();
    expect(body.acceptHeader).toBeUndefined();
  });

  test('registers a query parameter via the binding source', () => {
    const body = mapBody(
      buildAction({
        parameters: [
          {
            nameOnMethod: 'id',
            name: 'id',
            jsonName: null,
            type: 'System.Guid',
            typeSimple: 'string',
            isOptional: false,
            defaultValue: null,
            constraintTypes: null,
            bindingSourceId: eBindingSourceId.Query,
            descriptorName: '',
          },
        ],
      }),
    );

    expect(body.params).toEqual(['id']);
  });
});

describe('createActionToBodyMapper — IsRemoteStream backend flag', () => {
  const mapBody = createActionToBodyMapper();

  test('isRemoteStream=true forces blob even if Type is a custom subclass name', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'My.Project.CustomStreamContent',
          typeSimple: 'My.Project.CustomStreamContent',
          isRemoteStream: true,
          contentTypes: ['text/plain', 'application/json'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
    expect(body.acceptHeader).toBe('application/octet-stream');
    expect(body.isBlobMethod()).toBe(true);
  });

  test('[Volo.Abp.Content.IRemoteStreamContent] (real ABP square-bracket form) must NOT pick blob and degrades responseType to any[]', () => {
    // ABP serialises collections as `[T]` (not `T[]`) — pin the on-the-wire shape.
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.Collections.Generic.IList<Volo.Abp.Content.IRemoteStreamContent>',
          typeSimple: '[Volo.Abp.Content.IRemoteStreamContent]',
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBeUndefined();
    expect(body.acceptHeader).toBeUndefined();
    expect(body.isBlobMethod()).toBe(false);
    expect(body.responseType).toBe('any[]');
  });

  test('IRemoteStreamContent[] array return must NOT pick blob (server falls back to JSON metadata)', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'Volo.Abp.Content.IRemoteStreamContent[]',
          typeSimple: 'Volo.Abp.Content.IRemoteStreamContent[]',
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBeUndefined();
    expect(body.acceptHeader).toBeUndefined();
    expect(body.isBlobMethod()).toBe(false);
    expect(body.responseType).toBe('any[]');
  });

  test('isRemoteStream=false with stream-content type-name still detected by type name (legacy)', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'Volo.Abp.Content.IRemoteStreamContent',
          typeSimple: 'Volo.Abp.Content.IRemoteStreamContent',
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
    expect(body.acceptHeader).toBe('application/octet-stream');
  });
});

describe('createActionToBodyMapper — +json suffix detection', () => {
  const mapBody = createActionToBodyMapper();

  test('application/problem+json echoes back as Accept (decoder stays json)', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['application/problem+json'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
    expect(body.acceptHeader).toBe('application/problem+json');
  });

  test('text/json echoes back as text/json (decoder stays json)', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['text/json'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
    expect(body.acceptHeader).toBe('text/json');
  });

  test('application/vnd.api+json echoes back as Accept (decoder stays json)', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['application/vnd.api+json'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
    expect(body.acceptHeader).toBe('application/vnd.api+json');
  });
});

describe('createActionToBodyMapper — expanded binary whitelist', () => {
  const mapBody = createActionToBodyMapper();

  test.each([
    ['application/wasm'],
    ['font/woff2'],
    ['application/vnd.openxmlformats-officedocument.wordprocessingml.document'],
    ['application/vnd.ms-excel'],
    ['application/vnd.oasis.opendocument.spreadsheet'],
    ['application/x-msdownload'],
    ['application/rtf'],
    ['application/x-rar-compressed'],
    ['application/x-bzip2'],
    ['application/x-iso9660-image'],
    ['application/java-archive'],
    ['application/epub+zip'],
    ['model/gltf-binary'],
  ])('"%s" picked as blob', mediaType => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.Byte[]',
          typeSimple: 'byte[]',
          contentTypes: [mediaType],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
  });
});

describe('createActionToBodyMapper — contentTypes precedence and edge cases', () => {
  const mapBody = createActionToBodyMapper();

  test('isBlobMethod() type detection wins over json in contentTypes', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'Volo.Abp.Content.RemoteStreamContent',
          typeSimple: 'Volo.Abp.Content.RemoteStreamContent',
          contentTypes: ['application/json', 'text/plain'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
    expect(body.acceptHeader).toBe('application/octet-stream');
  });

  test('IRemoteStreamContent[] array falls through to defaults (server returns JSON metadata, not binary)', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'Volo.Abp.Content.IRemoteStreamContent[]',
          typeSimple: 'Volo.Abp.Content.IRemoteStreamContent[]',
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBeUndefined();
    expect(body.acceptHeader).toBeUndefined();
  });

  test('case-insensitive json detection (APPLICATION/JSON)', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['APPLICATION/JSON'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
    expect(body.acceptHeader).toBe('application/json');
  });

  test('image/* contentTypes alone picks blob and echoes back the first image type', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.Byte[]',
          typeSimple: 'byte[]',
          contentTypes: ['image/png', 'image/jpeg'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
    expect(body.acceptHeader).toBe('image/png');
  });

  test('video/* and audio/* picked as blob', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.Byte[]',
          typeSimple: 'byte[]',
          contentTypes: ['video/mp4', 'audio/mpeg'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
  });

  test('application/pdf contentTypes picked as blob', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.Byte[]',
          typeSimple: 'byte[]',
          contentTypes: ['application/pdf'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
  });

  test('empty contentTypes falls through to legacy string→text behavior', () => {
    const body = mapBody(
      buildAction({
        returnValue: { type: 'System.String', typeSimple: 'string', contentTypes: [] },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('text');
    expect(body.acceptHeader).toBeUndefined();
  });

  test('mixed text/* and application/json picks json and echoes the first json-shaped media type', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['text/json', 'text/plain', 'application/json'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
    expect(body.acceptHeader).toBe('text/json');
  });

  test('contentTypes with json-suffix variants still picks json', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['application/json; charset=utf-8'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
  });

  test('non-string non-blob type with contentTypes containing json defaults appropriately', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'My.Project.UserDto',
          typeSimple: 'My.Project.UserDto',
          contentTypes: ['application/json'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
    expect(body.acceptHeader).toBe('application/json');
  });

  test('json contentType with charset parameter is normalized', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['application/json; charset=utf-8'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
    expect(body.acceptHeader).toBe('application/json');
  });

  test('text contentType with charset parameter is normalized', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['text/plain ; charset=utf-8 '],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('text');
    expect(body.acceptHeader).toBe('text/plain');
  });

  test('mixed text/plain (with charset) and application/json picks json after normalize', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['text/plain; charset=utf-8', 'application/json; charset=utf-8'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('json');
  });

  test('text/csv only (custom text format) picks text and echoes the content type back', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'System.String',
          typeSimple: 'string',
          contentTypes: ['text/csv'],
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('text');
    expect(body.acceptHeader).toBe('text/csv');
  });
});

describe('createActionToBodyMapper — backward compatibility', () => {
  const mapBody = createActionToBodyMapper();

  test('legacy api-definition without contentTypes works (string)', () => {
    const body = mapBody(
      buildAction({
        returnValue: { type: 'System.String', typeSimple: 'string' } as Partial<Action>['returnValue'],
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('text');
    expect(body.acceptHeader).toBeUndefined();
  });

  test('legacy api-definition without contentTypes works (IRemoteStreamContent)', () => {
    const body = mapBody(
      buildAction({
        returnValue: {
          type: 'Volo.Abp.Content.IRemoteStreamContent',
          typeSimple: 'Volo.Abp.Content.IRemoteStreamContent',
        },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBe('blob');
    expect(body.acceptHeader).toBe('application/octet-stream');
  });

  test('legacy api-definition without contentTypes works (object)', () => {
    const body = mapBody(
      buildAction({
        returnValue: { type: 'My.Project.UserDto', typeSimple: 'My.Project.UserDto' },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBeUndefined();
    expect(body.acceptHeader).toBeUndefined();
  });

  test('void / no return preserves legacy behavior', () => {
    const body = mapBody(
      buildAction({
        returnValue: { type: 'System.Void', typeSimple: 'void' },
      } as Partial<Action>),
    );

    expect(body.httpResponseType).toBeUndefined();
    expect(body.acceptHeader).toBeUndefined();
  });
});

describe('proxy service template emission', () => {
  const template = readFileSync(TEMPLATE_PATH, 'utf8');

  test('reads body.httpResponseType and body.acceptHeader from body', () => {
    expect(template).toContain('body.httpResponseType');
    expect(template).toContain('body.acceptHeader');
  });

  test('emits Accept header conditional', () => {
    expect(template).toMatch(/headers:\s*\{\s*Accept:/);
  });

  test('emits responseType only for non-json httpResponseType', () => {
    expect(template).toMatch(/httpResponseType\s*&&\s*httpResponseType\s*!==\s*'json'/);
  });
});
