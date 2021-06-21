import {
  EntityProp,
  EntityPropContributorCallbacks,
  EntityPropDefaults,
  EntityPropsFactory,
} from '../lib/models/entity-props';
import { PropData } from '../lib/models/props';
import { createExtraPropertyValueResolver, mergeWithDefaultProps } from '../lib/utils/props.util';

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
        x: [(1 as any) as EntityProp, (2 as any) as EntityProp, (3 as any) as EntityProp],
        y: [(1 as any) as EntityProp, (2 as any) as EntityProp, (3 as any) as EntityProp],
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
});
