import { LinkedList } from '@abp/utils';
import { NEVER } from 'rxjs';
import { ePropType } from '../lib/enums/props.enum';
import {
  EntityProp,
  EntityPropContributorCallback,
  EntityPropList,
  EntityProps,
  EntityPropsFactory,
} from '../lib/models/entity-props';
import { PropData } from '../lib/models/props';

describe('EntityPropList', () => {
  it('should inherit from LinkedList', () => {
    expect(new EntityPropList() instanceof LinkedList).toBe(true);
  });
});

describe('EntityProps', () => {
  const add1toTail: EntityPropContributorCallback = propList => propList.addTail(1 as any);
  const add2toTail: EntityPropContributorCallback = propList => propList.addTail(2 as any);
  const add3toTail: EntityPropContributorCallback = propList => propList.addTail(3 as any);
  const dropIndex1: EntityPropContributorCallback = propList => propList.dropByIndex(1);

  describe('#props', () => {
    test.each`
      callbackList                                        | expected
      ${[]}                                               | ${''}
      ${[add1toTail, add2toTail, add3toTail]}             | ${'1 <-> 2 <-> 3'}
      ${[add3toTail, add2toTail, add1toTail]}             | ${'3 <-> 2 <-> 1'}
      ${[add1toTail, add2toTail, add3toTail, dropIndex1]} | ${'1 <-> 3'}
    `(
      'should return $expected when given callbackList is $callbackList',
      ({ callbackList, expected }) => {
        const creator = new EntityProps(callbackList);
        expect(creator.props.toString()).toBe(expected);
      },
    );
  });

  describe('#addContributor', () => {
    const creator = new EntityProps([]);

    test.each`
      callbackList                            | callback      | expected
      ${[]}                                   | ${add1toTail} | ${'1'}
      ${[add1toTail]}                         | ${add2toTail} | ${'1 <-> 2'}
      ${[add1toTail, add2toTail]}             | ${add3toTail} | ${'1 <-> 2 <-> 3'}
      ${[add1toTail, add2toTail, add3toTail]} | ${dropIndex1} | ${'1 <-> 3'}
    `(
      'should set props to $expected when callbackList is $callbackList and given callback is $callback',
      ({ callback, expected }) => {
        creator.addContributor(callback);
        expect(creator.props.toString()).toBe(expected);
      },
    );
  });
});

describe('EntityPropsFactory', () => {
  describe('#get', () => {
    it('should create and return an EntityProps instance', () => {
      const entityProps = new EntityPropsFactory();
      const creator = entityProps.get('');

      expect(creator).toBeInstanceOf(EntityProps);
    });

    it('should store and pass contributorCallbacks to EntityPropsCreator instance it returns', () => {
      const entityProps = new EntityPropsFactory();
      const creatorX1 = entityProps.get('X');

      expect(creatorX1).toBeInstanceOf(EntityProps);
      expect(creatorX1.props.toArray()).toHaveLength(0);

      creatorX1.addContributor(propList => propList.addTail(1 as any));

      const creatorX2 = entityProps.get('X');
      expect(creatorX2.props.toArray()).toHaveLength(1);
    });
  });
});

describe('EntityProp', () => {
  it('should be created when options object is passed as argument', () => {
    const options = {
      type: ePropType.String,
      name: 'NAME',
      displayName: 'DISPLAY NAME',
      permission: 'PERMISSION',
      visible: () => false,
      valueResolver: () => NEVER,
      sortable: true,
      columnWidth: 999,
    };

    const prop = new EntityProp(options);

    expect(prop.type).toBe(options.type);
    expect(prop.name).toBe(options.name);
    expect(prop.displayName).toBe(options.displayName);
    expect(prop.permission).toBe(options.permission);
    expect(prop.visible()).toBe(options.visible());
    expect(prop.valueResolver(null)).toBe(options.valueResolver());
    expect(prop.sortable).toBe(options.sortable);
    expect(prop.columnWidth).toBe(options.columnWidth);
  });

  it('should be created when only required options are passed', done => {
    const options = {
      type: ePropType.String,
      name: 'NAME',
    };

    const prop = new EntityProp(options);

    expect(prop.type).toBe(options.type);
    expect(prop.name).toBe(options.name);
    expect(prop.displayName).toBe(options.name);
    expect(prop.permission).toBeUndefined();
    expect(prop.visible()).toBe(true);
    expect(prop.sortable).toBe(false);
    expect(prop.columnWidth).toBeUndefined();
    prop.valueResolver({ record: { NAME: 'X' } } as PropData).subscribe(value => {
      expect(value).toBe('X');
      done();
    });
  });

  describe('#create', () => {
    it('should return a new instance from given options', () => {
      const options = {
        type: ePropType.String,
        name: 'NAME',
      };

      const prop = EntityProp.create(options);

      expect(prop).toBeInstanceOf(EntityProp);
      expect(prop.type).toBe(options.type);
      expect(prop.name).toBe(options.name);
    });
  });

  describe('#createMany', () => {
    it('should return multiple instances from given options array', () => {
      const options1 = {
        type: ePropType.String,
        name: 'NAME 1',
      };
      const options2 = {
        type: ePropType.Boolean,
        name: 'NAME 2',
      };

      const [prop1, prop2] = EntityProp.createMany([options1, options2]);

      expect(prop1).toBeInstanceOf(EntityProp);
      expect(prop1.type).toBe(options1.type);
      expect(prop1.name).toBe(options1.name);
      expect(prop2).toBeInstanceOf(EntityProp);
      expect(prop2.type).toBe(options2.type);
      expect(prop2.name).toBe(options2.name);
    });
  });
});
