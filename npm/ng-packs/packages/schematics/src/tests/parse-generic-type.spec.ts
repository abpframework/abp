import { parseBaseTypeWithGenericTypes } from '../utils/model';
import { parseGenerics   } from '../utils/tree';

import {test} from '@jest/globals';

const cases: Array<[string, string[]]> = [
  [
    'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto',
    ['Volo.Abp.Application.Dtos.AuditedEntityWithUserDto'],
  ],
  [
    'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<Volo.Abp.Identity.IdentityUserDto>',
    ['Volo.Abp.Application.Dtos.AuditedEntityWithUserDto', 'Volo.Abp.Identity.IdentityUserDto'],
  ],
  [
    'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<string,Volo.Abp.Identity.IdentityUserDto>',
    [
      'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto',
      'string',
      'Volo.Abp.Identity.IdentityUserDto'
    ],
  ],
  [
    'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<string,Volo.Abp.Identity.IdentityUserDto<number>>',
    [
      'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto',
      'string',
      'Volo.Abp.Identity.IdentityUserDto',
      'number',
    ],
  ],
  [
    'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<string,Volo.Abp.Identity.IdentityUserDto<Volo.Abp.Core.Dummy<System.String>>>',
    [
      'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto',
      'string',
      'Volo.Abp.Identity.IdentityUserDto',
      'Volo.Abp.Core.Dummy',
      'System.String',
    ],
  ],
  [
    'AuditedEntityWithUserDto',
    ['AuditedEntityWithUserDto'],
  ],
];

test.each(cases)('should parse %s', (inputStr, expected) => {
    const parsed = parseBaseTypeWithGenericTypes(inputStr);
    expect(parsed).toEqual(expected);
})

describe('parseGenerics', () => {

  it('should work with simple type', function() {
    const node = parseGenerics('System.String');
    expect(node.data).toEqual('System.String');
    expect(node.index).toBe(0);
    expect(node.parent).toBe(null);
  });
  it('should work with simple  Array type', function() {
    const node = parseGenerics('System.String[]');
    expect(node.data).toEqual('System.String[]');
    expect(node.index).toBe(0);
    expect(node.parent).toBe(null);
  });

  it('should work with simple', function() {
    const node = parseGenerics('Volo.Abp.Application.Dtos.AuditedEntityWithUserDto');
    expect(node.data).toEqual('Volo.Abp.Application.Dtos.AuditedEntityWithUserDto');
    expect(node.index).toBe(0);
    expect(node.parent).toBe(null);
  });

  it('should work with `Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<Volo.Abp.Identity.IdentityUserDto>`', function() {
    const type = 'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<Volo.Abp.Identity.IdentityUserDto>';
    const node = parseGenerics(type);

    expect(node.data).toEqual('Volo.Abp.Application.Dtos.AuditedEntityWithUserDto');
    expect(node.children.length).toBe(1);
    const child = node.children[0];
    expect(node.children[0].data).toEqual('Volo.Abp.Identity.IdentityUserDto');
    expect(child.parent).toBe(node);
  });


  it('should work with `Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<System.string,Volo.Abp.Identity.IdentityUserDto>`', function() {
    const type = 'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<System.string,Volo.Abp.Identity.IdentityUserDto>';
    const node = parseGenerics(type);

    expect(node.data).toEqual('Volo.Abp.Application.Dtos.AuditedEntityWithUserDto');
    expect(node.children.length).toBe(2);
    expect(node.children[0].data).toEqual('System.string');
    expect(node.children[0].index).toBe(0)
    expect(node.children[1].data).toEqual('Volo.Abp.Identity.IdentityUserDto');
    expect(node.children[1].index).toBe(1);

  });
  it('should Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<System.string,Volo.Abp.Identity.IdentityUserDto<System.Int>>', function() {
    const type = 'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<System.string,Volo.Abp.Identity.IdentityUserDto<System.Int>>';
    const node = parseGenerics(type);
    expect(node.data).toEqual('Volo.Abp.Application.Dtos.AuditedEntityWithUserDto');
    expect(node.children.length).toBe(2);
    expect(node.children[0].data).toEqual('System.string');
    expect((node.children[0]).parent).toBe(node);

    expect(node.children[1].data).toEqual('Volo.Abp.Identity.IdentityUserDto');
    expect(node.children[1].children.length).toBe(1);
    expect(node.children[1].children[0].data).toEqual('System.Int');
    expect(node.children[1].children[0].parent).toBe(node.children[1])
    expect(node.children[1].children[0].index).toBe(0)

  });

  it('should Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<Volo.Abp.Identity.IdentityUserDto<System.Int>,System.string>', function() {
    const type = 'Volo.Abp.Application.Dtos.AuditedEntityWithUserDto<Volo.Abp.Identity.IdentityUserDto<System.Int>,System.string>';
    const node = parseGenerics(type);
    expect(node.data).toEqual('Volo.Abp.Application.Dtos.AuditedEntityWithUserDto');
    expect(node.children.length).toBe(2);
    expect(node.children[0].data).toEqual('Volo.Abp.Identity.IdentityUserDto');
    expect(node.children[0].children.length).toBe(1);
    expect(node.children[0].children[0].data).toEqual('System.Int');
    expect(node.children[1].data).toEqual('System.string');

  });
});



