import { LinkedList } from '@abp/utils';
import {
  EntityAction,
  EntityActionContributorCallback,
  EntityActionList,
  EntityActions,
  EntityActionsFactory,
} from '../lib/models/entity-actions';

describe('EntityActionList', () => {
  it('should inherit from LinkedList', () => {
    expect(new EntityActionList() instanceof LinkedList).toBe(true);
  });
});

describe('EntityActions', () => {
  const add1toTail: EntityActionContributorCallback = actionList => actionList.addTail(1 as any);
  const add2toTail: EntityActionContributorCallback = actionList => actionList.addTail(2 as any);
  const add3toTail: EntityActionContributorCallback = actionList => actionList.addTail(3 as any);
  const dropIndex1: EntityActionContributorCallback = actionList => actionList.dropByIndex(1);

  describe('#actions', () => {
    test.each`
      callbackList                                        | expected
      ${[]}                                               | ${''}
      ${[add1toTail, add2toTail, add3toTail]}             | ${'1 <-> 2 <-> 3'}
      ${[add3toTail, add2toTail, add1toTail]}             | ${'3 <-> 2 <-> 1'}
      ${[add1toTail, add2toTail, add3toTail, dropIndex1]} | ${'1 <-> 3'}
    `(
      'should return $expected when given callbackList is $callbackList',
      ({ callbackList, expected }) => {
        const creator = new EntityActions(callbackList);
        expect(creator.actions.toString()).toBe(expected);
      },
    );
  });

  describe('#addContributor', () => {
    const creator = new EntityActions([]);

    test.each`
      callbackList                            | callback      | expected
      ${[]}                                   | ${add1toTail} | ${'1'}
      ${[add1toTail]}                         | ${add2toTail} | ${'1 <-> 2'}
      ${[add1toTail, add2toTail]}             | ${add3toTail} | ${'1 <-> 2 <-> 3'}
      ${[add1toTail, add2toTail, add3toTail]} | ${dropIndex1} | ${'1 <-> 3'}
    `(
      'should set actions to $expected when callbackList is $callbackList and given callback is $callback',
      ({ callback, expected }) => {
        creator.addContributor(callback);
        expect(creator.actions.toString()).toBe(expected);
      },
    );
  });
});

describe('EntityActionsFactory', () => {
  describe('#get', () => {
    it('should create and return an EntityActions instance', () => {
      const entityActions = new EntityActionsFactory();
      const creator = entityActions.get('');

      expect(creator).toBeInstanceOf(EntityActions);
    });

    it('should store and pass contributorCallbacks to EntityActionsCreator instance it returns', () => {
      const entityActions = new EntityActionsFactory();
      const creatorX1 = entityActions.get('X');

      expect(creatorX1).toBeInstanceOf(EntityActions);
      expect(creatorX1.actions.toArray()).toHaveLength(0);

      creatorX1.addContributor(actionList => actionList.addTail(1 as any));

      const creatorX2 = entityActions.get('X');
      expect(creatorX2.actions.toArray()).toHaveLength(1);
    });
  });
});

describe('EntityAction', () => {
  it('should be created when options object is passed as argument', () => {
    const options = {
      text: 'TEXT',
      action: () => 'ACTION',
      permission: 'PERMISSION',
      visible: () => false,
      icon: 'ICON',
    };

    const action = new EntityAction(options);

    expect(action.text).toBe(options.text);
    expect(action.action(null)).toBe(options.action());
    expect(action.permission).toBe(options.permission);
    expect(action.visible(null)).toBe(options.visible());
    expect(action.icon).toBe(options.icon);
  });

  it('should be created when only required options are passed', () => {
    const options = {
      text: 'TEXT',
      action: () => 'ACTION',
    };

    const action = new EntityAction(options);

    expect(action.text).toBe(options.text);
    expect(action.action).toBe(options.action);
    expect(action.permission).toBeUndefined();
    expect(action.visible(null)).toBe(true);
    expect(action.icon).toBe('');
  });

  describe('#create', () => {
    it('should return a new instance from given options', () => {
      const options = {
        text: 'TEXT',
        action: () => 'ACTION',
      };

      const action = EntityAction.create(options);

      expect(action).toBeInstanceOf(EntityAction);
      expect(action.text).toBe(options.text);
      expect(action.action).toBe(options.action);
    });
  });

  describe('#createMany', () => {
    it('should return multiple instances from given options array', () => {
      const options1 = {
        text: 'TEXT 1',
        action: () => 'ACTION 1',
      };
      const options2 = {
        text: 'TEXT 2',
        action: () => 'ACTION 2',
      };

      const [action1, action2] = EntityAction.createMany([options1, options2]);

      expect(action1).toBeInstanceOf(EntityAction);
      expect(action1.text).toBe(options1.text);
      expect(action1.action).toBe(options1.action);
      expect(action2).toBeInstanceOf(EntityAction);
      expect(action2.text).toBe(options2.text);
      expect(action2.action).toBe(options2.action);
    });
  });
});
