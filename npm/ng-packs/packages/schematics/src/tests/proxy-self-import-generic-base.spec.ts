import { describe, expect, it } from 'vitest';
import { createImportRefsToModelReducer } from '../utils/model';
import { Type } from '../models/api-definition';
import { ModelGeneratorParams } from '../utils/model';

// Reproduces #25080: when a DTO inherits from a generic base type (e.g. PagedResultDto<T>)
// whose type argument T lives in the SAME namespace, the proxy generator used to emit an
// invalid self-import (`import type { TransactionDto } from './models'`) inside models.ts,
// breaking the Angular/TypeScript build.
describe('proxy generation - self-import for generic base type argument (#25080)', () => {
  const solution = 'MyApp';

  // TransactionPagedResultDto : PagedResultDto<TransactionDto>, both in MyApp.Transactions.
  const types: Record<string, Type> = {
    'MyApp.Transactions.TransactionDto': {
      baseType: null,
      isEnum: false,
      enumNames: null,
      enumValues: null,
      genericArguments: null,
      properties: [
        {
          name: 'Id',
          jsonName: null,
          type: 'System.Guid',
          typeSimple: 'System.Guid',
          isRequired: false,
          isNullable: false,
        },
      ],
    },
    'MyApp.Transactions.TransactionPagedResultDto': {
      baseType: 'Volo.Abp.Application.Dtos.PagedResultDto<MyApp.Transactions.TransactionDto>',
      isEnum: false,
      enumNames: null,
      enumValues: null,
      genericArguments: null,
      properties: [
        {
          name: 'AmountSummary',
          jsonName: null,
          type: 'System.Int32',
          typeSimple: 'number',
          isRequired: false,
          isNullable: false,
        },
      ],
    },
  };

  const params: ModelGeneratorParams = {
    targetPath: 'src/app/proxy',
    solution,
    types,
    serviceImports: {},
    modelImports: {},
  };

  it('does not emit a self-import for a same-namespace generic base argument', () => {
    const reduce = createImportRefsToModelReducer(params);
    const models = reduce([], ['MyApp.Transactions.TransactionPagedResultDto']);

    const model = models.find(m => m.namespace === 'Transactions');
    expect(model).toBeDefined();

    // The buggy behavior produced an import with path './models' (a self-import).
    const selfImports = model!.imports.filter(i => i.path === './models');
    expect(selfImports).toEqual([]);

    // Sanity: TransactionDto must NOT be imported at all, since it is declared in this same file.
    const importsTransactionDto = model!.imports.some(i =>
      i.specifiers.some(s => s === 'TransactionDto'),
    );
    expect(importsTransactionDto).toBe(false);

    // The framework base type, however, should still be imported from @abp/ng.core.
    const importsPagedResultDto = model!.imports.some(
      i => i.path === '@abp/ng.core' && i.specifiers.includes('PagedResultDto'),
    );
    expect(importsPagedResultDto).toBe(true);
  });
});

describe('proxy generation - generic argument of a same-namespace base type', () => {
  const solution = 'MyApp';

  // OrderResultDto : ResultBase<CustomerDto>, where ResultBase<T> lives in MyApp.Orders and
  // CustomerDto lives in MyApp.Customers.
  const types: Record<string, Type> = {
    'MyApp.Customers.CustomerDto': {
      baseType: null,
      isEnum: false,
      enumNames: null,
      enumValues: null,
      genericArguments: null,
      properties: [
        {
          name: 'Name',
          jsonName: null,
          type: 'System.String',
          typeSimple: 'string',
          isRequired: false,
          isNullable: false,
        },
      ],
    },
    'MyApp.Orders.ResultBase<T0>': {
      baseType: null,
      isEnum: false,
      enumNames: null,
      enumValues: null,
      genericArguments: ['T'],
      properties: [
        {
          name: 'Items',
          jsonName: null,
          type: '[T]',
          typeSimple: '[T]',
          isRequired: false,
          isNullable: false,
        },
      ],
    },
    'MyApp.Orders.OrderResultDto': {
      baseType: 'MyApp.Orders.ResultBase<MyApp.Customers.CustomerDto>',
      isEnum: false,
      enumNames: null,
      enumValues: null,
      genericArguments: null,
      properties: [],
    },
  };

  const params: ModelGeneratorParams = {
    targetPath: 'src/app/proxy',
    solution,
    types,
    serviceImports: {},
    modelImports: {},
  };

  it('imports a generic argument that lives in another namespace', () => {
    const reduce = createImportRefsToModelReducer(params);
    const models = reduce([], ['MyApp.Orders.OrderResultDto']);

    const model = models.find(m => m.namespace === 'Orders');
    expect(model).toBeDefined();

    expect(model!.imports.find(i => i.path === '../customers/models')?.specifiers).toEqual([
      'CustomerDto',
    ]);
    expect(model!.imports.some(i => i.path === './models')).toBe(false);
  });
});
