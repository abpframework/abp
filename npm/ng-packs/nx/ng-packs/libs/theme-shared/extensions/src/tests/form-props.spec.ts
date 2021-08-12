import { LinkedList } from '@abp/utils';
import { NEVER } from 'rxjs';
import { ePropType } from '../lib/enums/props.enum';
import {
  CreateFormPropContributorCallback,
  CreateFormPropsFactory,
  EditFormPropsFactory,
  FormProp,
  FormPropList,
  FormProps,
} from '../lib/models/form-props';

describe('FormPropList', () => {
  it('should inherit from LinkedList', () => {
    expect(new FormPropList() instanceof LinkedList).toBe(true);
  });
});

describe('FormProps', () => {
  const add1toTail: CreateFormPropContributorCallback = propList => propList.addTail(1 as any);
  const add2toTail: CreateFormPropContributorCallback = propList => propList.addTail(2 as any);
  const add3toTail: CreateFormPropContributorCallback = propList => propList.addTail(3 as any);
  const dropIndex1: CreateFormPropContributorCallback = propList => propList.dropByIndex(1);

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
        const creator = new FormProps(callbackList);
        expect(creator.props.toString()).toBe(expected);
      },
    );
  });

  describe('#addContributor', () => {
    const creator = new FormProps([]);

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

describe('FormPropsFactory', () => {
  describe('#get', () => {
    it('should create and return an FormProps instance', () => {
      const formProps = new CreateFormPropsFactory();
      const creator = formProps.get('');

      expect(creator).toBeInstanceOf(FormProps);
    });

    it('should store and pass contributorCallbacks to FormPropsCreator instance it returns', () => {
      const formProps = new EditFormPropsFactory();
      const creatorX1 = formProps.get('X');

      expect(creatorX1).toBeInstanceOf(FormProps);
      expect(creatorX1.props.toArray()).toHaveLength(0);

      creatorX1.addContributor(propList => propList.addTail(1 as any));

      const creatorX2 = formProps.get('X');
      expect(creatorX2.props.toArray()).toHaveLength(1);
    });
  });
});

describe('FormProp', () => {
  it('should be created when options object is passed as argument', () => {
    const options = {
      type: ePropType.String,
      name: 'NAME',
      displayName: 'DISPLAY NAME',
      permission: 'PERMISSION',
      visible: () => false,
      asyncValidators: () => [null],
      validators: () => [null],
      disabled: () => true,
      readonly: () => true,
      autocomplete: 'AUTOCOMPLETE',
      defaultValue: 'DEFAULT VALUE',
      options: () => NEVER,
      id: 'ID',
    };

    const prop = new FormProp(options);

    expect(prop.type).toBe(options.type);
    expect(prop.name).toBe(options.name);
    expect(prop.displayName).toBe(options.displayName);
    expect(prop.permission).toBe(options.permission);
    expect(prop.visible()).toBe(options.visible());
    expect(prop.asyncValidators()).toEqual(options.asyncValidators());
    expect(prop.validators()).toEqual(options.validators());
    expect(prop.disabled()).toBe(options.disabled());
    expect(prop.readonly()).toBe(options.readonly());
    expect(prop.autocomplete).toBe(options.autocomplete);
    expect(prop.defaultValue).toBe(options.defaultValue);
    expect(prop.options()).toBe(options.options());
    expect(prop.id).toBe(options.id);
  });

  it('should be created when only required options are passed', () => {
    const options = {
      type: ePropType.String,
      name: 'NAME',
    };

    const prop = new FormProp(options);

    expect(prop.type).toBe(options.type);
    expect(prop.name).toBe(options.name);
    expect(prop.displayName).toBe(options.name);
    expect(prop.permission).toBeUndefined();
    expect(prop.visible()).toBe(true);
    expect(prop.asyncValidators()).toEqual([]);
    expect(prop.validators()).toEqual([]);
    expect(prop.disabled()).toBe(false);
    expect(prop.readonly()).toBe(false);
    expect(prop.autocomplete).toBe('off');
    expect(prop.defaultValue).toBeNull();
    expect(prop.options).toBeUndefined();
    expect(prop.id).toBe(options.name);
  });

  test.each`
    defaultValue | expected
    ${0}         | ${0}
    ${''}        | ${''}
    ${false}     | ${false}
    ${undefined} | ${null}
  `(
    'should set defaultValue as $expected when $defaultValue is given',
    ({ defaultValue, expected }) => {
      const options = { type: null, name: null, defaultValue };
      const prop = new FormProp(options);
      expect(prop.defaultValue).toBe(expected);
    },
  );

  describe('#create', () => {
    it('should return a new instance from given options', () => {
      const options = {
        type: ePropType.String,
        name: 'NAME',
      };

      const prop = FormProp.create(options);

      expect(prop).toBeInstanceOf(FormProp);
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

      const [prop1, prop2] = FormProp.createMany([options1, options2]);

      expect(prop1).toBeInstanceOf(FormProp);
      expect(prop1.type).toBe(options1.type);
      expect(prop1.name).toBe(options1.name);
      expect(prop2).toBeInstanceOf(FormProp);
      expect(prop2.type).toBe(options2.type);
      expect(prop2.name).toBe(options2.name);
    });
  });
});
