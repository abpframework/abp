import { describe, expect, it } from 'vitest';
import { createActionToSignatureMapper } from '../utils/service';
import { Action } from '../models/api-definition';

// Real signature-generation pipeline for #25668: array parameters should be declared `readonly`.
const buildSignature = (name: string, typeSimple: string, type: string) =>
  createActionToSignatureMapper()({
    uniqueName: 'GetAsync',
    name: 'GetAsync',
    httpMethod: 'GET',
    url: 'api/app/my',
    supportedVersions: [],
    parametersOnMethod: [{ name, typeAsString: '', type, typeSimple, isOptional: false, defaultValue: null }],
    parameters: [],
    returnValue: { type: 'System.Void', typeSimple: 'void' },
  } as unknown as Action);

const paramType = (name: string, typeSimple: string, type: string) =>
  (buildSignature(name, typeSimple, type).parameters.find(p => p.name === name) as { type: string })
    .type;

describe('proxy generation - readonly array params', () => {
  it('declares a primitive array parameter as readonly', () => {
    expect(paramType('ids', '[string]', 'System.String[]')).toBe('readonly string[]');
  });

  it('declares a complex array parameter as readonly', () => {
    expect(paramType('items', '[Volo.Abp.Dto]', 'Volo.Abp.Dto[]')).toBe('readonly Dto[]');
  });

  it('leaves a non-array parameter unchanged', () => {
    expect(paramType('name', 'string', 'System.String')).toBe('string');
  });
});
