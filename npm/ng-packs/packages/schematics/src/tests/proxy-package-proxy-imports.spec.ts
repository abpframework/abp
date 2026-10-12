import { describe, expect, it } from 'vitest';
import { Action, Controller, Type } from '../models/api-definition';
import { createImportRefsToModelReducer, ModelGeneratorParams } from '../utils/model';
import { createControllerToServiceMapper } from '../utils/service';
import { createTypesToImportsReducer } from '../utils/type';

// DTOs listed in VOLO_PACKAGE_PROXY_IMPORTS (e.g. EntityChangeDto) are not generated locally,
// they must be imported from their package proxy (e.g. @volo/abp.ng.audit-logging/proxy).
describe('proxy generation - package proxy imports', () => {
  const solution = 'Acme.BookStore';
  const auditLoggingProxy = '@volo/abp.ng.audit-logging/proxy';

  const dto = (baseType: string | null, properties: [string, string][] = []): Type => ({
    baseType,
    isEnum: false,
    enumNames: null,
    enumValues: null,
    genericArguments: null,
    properties: properties.map(([name, type]) => ({
      name,
      jsonName: null,
      type,
      typeSimple: type,
      isRequired: false,
      isNullable: false,
    })),
  });

  const types: Record<string, Type> = {
    'Volo.Abp.AuditLogging.AuditLogDto': dto(
      'Volo.Abp.Application.Dtos.ExtensibleEntityDto<System.Guid>',
      [['UserName', 'System.String']],
    ),
    'Volo.Abp.AuditLogging.EntityChangeDto': dto(
      'Volo.Abp.Application.Dtos.ExtensibleEntityDto<System.Guid>',
      [['AuditLogId', 'System.Guid']],
    ),
    'Volo.Abp.AuditLogging.EntityChangeWithUsernameDto': dto(null, [
      ['EntityChange', 'Volo.Abp.AuditLogging.EntityChangeDto'],
      ['UserName', 'System.String'],
    ]),
    'Volo.Abp.AuditLogging.EntityChangePagedResultDto': dto(
      'Volo.Abp.Application.Dtos.PagedResultDto<Volo.Abp.AuditLogging.EntityChangeDto>',
    ),
    'Volo.Abp.AuditLogging.LocalListBase<T0>': {
      ...dto(null, [['Items', '[T]']]),
      genericArguments: ['T'],
    },
    'Volo.Abp.AuditLogging.EntityChangeListDto': dto(
      'Volo.Abp.AuditLogging.LocalListBase<Volo.Abp.AuditLogging.EntityChangeDto>',
      [['Total', 'System.Int32']],
    ),
  };

  const params: ModelGeneratorParams = {
    targetPath: 'src/app/proxy',
    solution,
    types,
    serviceImports: {},
    modelImports: {},
  };

  const generateModel = (ref: string) => {
    const models = createImportRefsToModelReducer(params)([], [ref]);
    const model = models.find(m => m.namespace === 'Volo.Abp.AuditLogging');
    expect(model).toBeDefined();
    return model!;
  };

  it('imports a same-namespace package DTO used by a property from its package proxy', () => {
    const model = generateModel('Volo.Abp.AuditLogging.EntityChangeWithUsernameDto');

    expect(model.interfaces.map(i => i.identifier)).toEqual(['EntityChangeWithUsernameDto']);
    expect(model.imports.find(i => i.path === auditLoggingProxy)?.specifiers).toEqual([
      'EntityChangeDto',
    ]);
    expect(model.imports.some(i => i.path === './models')).toBe(false);
  });

  it('imports a package DTO used as a generic argument of an external base type', () => {
    const model = generateModel('Volo.Abp.AuditLogging.EntityChangePagedResultDto');

    expect(model.imports.find(i => i.path === auditLoggingProxy)?.specifiers).toEqual([
      'EntityChangeDto',
    ]);
    expect(model.imports.some(i => i.path === './models')).toBe(false);
  });

  it('imports a package DTO used as a generic argument of a same-namespace base type', () => {
    const model = generateModel('Volo.Abp.AuditLogging.EntityChangeListDto');

    expect(model.imports.find(i => i.path === auditLoggingProxy)?.specifiers).toEqual([
      'EntityChangeDto',
    ]);
    expect(model.imports.some(i => i.path === './models')).toBe(false);
  });

  it('imports package DTOs from the package proxy and other DTOs from local models', () => {
    const reduceTypesToImports = createTypesToImportsReducer(solution, 'EntityHistory');
    const imports = reduceTypesToImports(
      [],
      [
        { type: 'Volo.Abp.AuditLogging.EntityChangeDto', isEnum: false },
        { type: 'Volo.Abp.AuditLogging.EntityChangeWithUsernameDto', isEnum: false },
      ],
    );

    expect(imports.map(i => ({ path: i.path, specifiers: i.specifiers }))).toEqual([
      { path: auditLoggingProxy, specifiers: ['EntityChangeDto'] },
      { path: '../volo/abp/audit-logging/models', specifiers: ['EntityChangeWithUsernameDto'] },
    ]);
  });

  it('imports package DTOs returned by a service from the package proxy', () => {
    const action = (name: string, type: string, typeSimple: string) =>
      ({
        uniqueName: name,
        name,
        httpMethod: 'GET',
        url: `api/app/entity-history/${name}`,
        supportedVersions: [],
        parametersOnMethod: [],
        parameters: [],
        returnValue: { type, typeSimple },
      }) as unknown as Action;

    const controller = {
      controllerName: 'EntityHistory',
      type: 'Acme.BookStore.EntityHistory.EntityHistoryAppService',
      isRemoteService: true,
      isIntegrationService: false,
      interfaces: [],
      actions: {
        GetAuditLogsAsync: action(
          'GetAuditLogsAsync',
          'Volo.Abp.Application.Dtos.PagedResultDto<Volo.Abp.AuditLogging.AuditLogDto>',
          'Volo.Abp.Application.Dtos.PagedResultDto<Volo.Abp.AuditLogging.AuditLogDto>',
        ),
        GetEntityChangesAsync: action(
          'GetEntityChangesAsync',
          'System.Collections.Generic.List<Volo.Abp.AuditLogging.EntityChangeDto>',
          '[Volo.Abp.AuditLogging.EntityChangeDto]',
        ),
        GetHistoryAsync: action(
          'GetHistoryAsync',
          'System.Collections.Generic.List<Volo.Abp.AuditLogging.EntityChangeWithUsernameDto>',
          '[Volo.Abp.AuditLogging.EntityChangeWithUsernameDto]',
        ),
      },
    } as Controller;

    const service = createControllerToServiceMapper({
      targetPath: 'src/app/proxy',
      solution,
      types,
      apiName: 'Default',
      controllers: [controller],
      serviceImports: {},
    })(controller);

    expect(service.imports.find(i => i.path === auditLoggingProxy)?.specifiers).toEqual([
      'AuditLogDto',
      'EntityChangeDto',
    ]);
    expect(
      service.imports.find(i => i.path === '../volo/abp/audit-logging/models')?.specifiers,
    ).toEqual(['EntityChangeWithUsernameDto']);
  });
});
