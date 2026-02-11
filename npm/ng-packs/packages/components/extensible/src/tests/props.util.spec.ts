import { ConfigStateService, PermissionService } from '@abp/ng.core';
import { Injector } from '@angular/core';
import {
  checkPolicies,
  createExtraPropertyValueResolver,
  mergeWithDefaultProps,
} from '../lib/utils/props.util';
import { ObjectExtensions } from '../lib/models/object-extensions';
import {
  EntityProp,
  EntityPropContributorCallbacks,
  EntityPropDefaults,
  EntityPropsFactory,
} from '../lib/models/entity-props';
import { PropData } from '../lib/models/props';

class MockPropData<R = any> extends PropData<R> {
  getInjected: PropData<R>['getInjected'];

  constructor(public readonly record: R) {
    super();
  }
}

describe('Entity Prop Utils', () => {
  describe('#createExtraPropertyValueResolver', () => {
    it('should return a resolver that resolves an observable value from extraProperties', async () => {
      const valueResolver = createExtraPropertyValueResolver('foo');
      const propData = new MockPropData({ extraProperties: { foo: 'bar' } });

      const bar = await valueResolver(propData).toPromise();
      expect(bar).toBe('bar');
    });
  });

  describe('#mergeEntityProps', () => {
    let entityProps: EntityPropsFactory;

    beforeEach(() => {
      entityProps = new EntityPropsFactory();
    });

    it('should merge default props with prop contributors', () => {
      const defaults: EntityPropDefaults = {
        x: [1 as any as EntityProp, 2 as any as EntityProp, 3 as any as EntityProp],
        y: [1 as any as EntityProp, 2 as any as EntityProp, 3 as any as EntityProp],
      };

      const contributors1: EntityPropContributorCallbacks = {
        x: [
          propList => {
            const x2 = propList.dropByIndex(1); // 1 <-> 3
            propList.addHead(x2.value); // 2 <-> 1 <-> 3
          },
          propList => {
            propList.dropTail(); // 2 <-> 1
          },
        ],
      };

      const contributors2: EntityPropContributorCallbacks = {
        y: [
          propList => {
            const y2 = propList.dropByIndex(1); // 1 <-> 3
            propList.addTail(y2.value); // 1 <-> 3 <-> 2
          },
          propList => {
            propList.dropHead(); // 3 <-> 2
          },
        ],
      };

      mergeWithDefaultProps(entityProps, defaults, contributors1, contributors2);

      expect(entityProps.get('x').props.toString()).toBe('2 <-> 1');
      expect(entityProps.get('y').props.toString()).toBe('3 <-> 2');
    });
  });

  describe('#checkPolicies', () => {
    let injector: Injector;
    let configState: ConfigStateService;
    let permissionService: PermissionService;

    beforeEach(() => {
      jest.clearAllMocks();

      configState = {
        getGlobalFeatureIsEnabled: jest.fn().mockReturnValue(false),
        getFeatureIsEnabled: jest.fn().mockReturnValue(false),
      } as any;

      permissionService = {
        getGrantedPolicy: jest.fn().mockReturnValue(false),
      } as any;

      injector = {
        get: jest.fn().mockImplementation(service => {
          if (service === ConfigStateService) {
            return configState;
          }
          if (service === PermissionService) {
            return permissionService;
          }
          return null;
        }),
      } as any;
    });

    it('should keep property when all permissions and features are satisfied', () => {
      (permissionService.getGrantedPolicy as jest.Mock).mockReturnValue(true);
      (configState.getGlobalFeatureIsEnabled as jest.Mock).mockReturnValue(true);
      (configState.getFeatureIsEnabled as jest.Mock).mockReturnValue(true);

      const properties: ObjectExtensions.EntityExtensionProperties = {
        property1: {
          policy: {
            permissions: { permissionNames: ['Permission1', 'Permission2'], requiresAll: true },
            globalFeatures: { features: ['GlobalFeature1'], requiresAll: true },
            features: { features: ['Feature1'], requiresAll: true },
          },
          displayName: undefined,
          api: undefined,
          ui: undefined,
          attributes: [],
          configuration: undefined,
          defaultValue: undefined,
        },
      };

      checkPolicies(injector, properties);

      expect(properties.property1).toBeDefined();
      expect(permissionService.getGrantedPolicy).toHaveBeenCalledWith('Permission1');
      expect(permissionService.getGrantedPolicy).toHaveBeenCalledWith('Permission2');
      expect(configState.getGlobalFeatureIsEnabled).toHaveBeenCalledWith('GlobalFeature1');
      expect(configState.getFeatureIsEnabled).toHaveBeenCalledWith('Feature1');
    });

    it('should keep property when no permissions and features are satisfied', () => {
      (permissionService.getGrantedPolicy as jest.Mock).mockReturnValue(false);
      (configState.getGlobalFeatureIsEnabled as jest.Mock).mockReturnValue(false);
      (configState.getFeatureIsEnabled as jest.Mock).mockReturnValue(false);

      const properties: ObjectExtensions.EntityExtensionProperties = {
        property1: {
          policy: {
            permissions: { permissionNames: ['Permission1', 'Permission2'], requiresAll: true },
            globalFeatures: { features: ['GlobalFeature1'], requiresAll: true },
            features: { features: ['Feature1'], requiresAll: true },
          },
          displayName: undefined,
          api: undefined,
          ui: undefined,
          attributes: [],
          configuration: undefined,
          defaultValue: undefined,
        },
      };

      checkPolicies(injector, properties);

      expect(properties.property1).toBeUndefined();
      expect(permissionService.getGrantedPolicy).toHaveBeenCalledWith('Permission1');
      expect(permissionService.getGrantedPolicy).not.toHaveBeenCalledWith('Permission2');
      expect(configState.getGlobalFeatureIsEnabled).not.toHaveBeenCalledWith('GlobalFeature1');
      expect(configState.getFeatureIsEnabled).not.toHaveBeenCalledWith('Feature1');
    });

    it('should delete property when only some permissions are granted', () => {
      (permissionService.getGrantedPolicy as jest.Mock).mockImplementation(
        permission => permission === 'Permission1',
      );
      (configState.getGlobalFeatureIsEnabled as jest.Mock).mockReturnValue(true);
      (configState.getFeatureIsEnabled as jest.Mock).mockReturnValue(true);

      const properties: ObjectExtensions.EntityExtensionProperties = {
        property1: {
          policy: {
            permissions: { permissionNames: ['Permission1', 'Permission2'], requiresAll: true },
            globalFeatures: { features: ['GlobalFeature1'], requiresAll: true },
            features: { features: ['Feature1'], requiresAll: true },
          },
          displayName: undefined,
          api: undefined,
          ui: undefined,
          attributes: [],
          configuration: undefined,
          defaultValue: undefined,
        },
      };

      checkPolicies(injector, properties);

      expect(properties.property1).toBeUndefined();
      expect(permissionService.getGrantedPolicy).toHaveBeenCalledWith('Permission1');
      expect(permissionService.getGrantedPolicy).toHaveBeenCalledWith('Permission2');
    });

    it('should delete property when some global features are disabled', () => {
      (permissionService.getGrantedPolicy as jest.Mock).mockReturnValue(true);
      (configState.getGlobalFeatureIsEnabled as jest.Mock).mockImplementation(feature =>
        feature === 'GlobalFeature2' ? false : true,
      );
      (configState.getFeatureIsEnabled as jest.Mock).mockReturnValue(true);

      const properties: ObjectExtensions.EntityExtensionProperties = {
        property1: {
          policy: {
            permissions: { permissionNames: ['Permission1'], requiresAll: true },
            globalFeatures: { features: ['GlobalFeature1', 'GlobalFeature2'], requiresAll: true },
            features: { features: ['Feature1'], requiresAll: true },
          },
          displayName: undefined,
          api: undefined,
          ui: undefined,
          attributes: [],
          configuration: undefined,
          defaultValue: undefined,
        },
      };

      checkPolicies(injector, properties);

      expect(properties.property1).toBeUndefined();
      expect(configState.getGlobalFeatureIsEnabled).toHaveBeenCalledWith('GlobalFeature1');
      expect(configState.getGlobalFeatureIsEnabled).toHaveBeenCalledWith('GlobalFeature2');
    });

    it('should keep property when all permissions are granted but only some features are required', () => {
      (permissionService.getGrantedPolicy as jest.Mock).mockImplementation(
        permission => permission === 'Permission1' || permission === 'Permission2',
      );
      (configState.getFeatureIsEnabled as jest.Mock).mockImplementation(
        feature => feature === 'Feature1',
      );
      (configState.getGlobalFeatureIsEnabled as jest.Mock).mockReturnValue(true);

      const properties: ObjectExtensions.EntityExtensionProperties = {
        property1: {
          policy: {
            permissions: { permissionNames: ['Permission1', 'Permission2'], requiresAll: false },
            features: {
              features: ['Feature1', 'Feature2', 'Feature3'],
              requiresAll: false,
            },
            globalFeatures: { features: ['GlobalFeature1'], requiresAll: true },
          },
          displayName: undefined,
          api: undefined,
          ui: undefined,
          attributes: [],
          configuration: undefined,
          defaultValue: undefined,
        },
      };

      checkPolicies(injector, properties);

      expect(properties.property1).toBeDefined();

      expect(configState.getFeatureIsEnabled).toHaveBeenCalledWith('Feature1');
      expect(configState.getFeatureIsEnabled).not.toHaveBeenCalledWith('Feature2');
      expect(configState.getFeatureIsEnabled).not.toHaveBeenCalledWith('Feature3');

      expect(configState.getGlobalFeatureIsEnabled).toHaveBeenCalledWith('GlobalFeature1');
    });
  });
});
