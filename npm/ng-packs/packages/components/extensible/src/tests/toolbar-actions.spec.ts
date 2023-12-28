import { LinkedList } from '@abp/utils';
import {
  ToolbarAction,
  ToolbarActionContributorCallback,
  ToolbarActionList,
  ToolbarActions,
  ToolbarActionsFactory,
  ToolbarComponent,
} from '../lib/models/toolbar-actions';

describe('ToolbarActionList', () => {
  it('should inherit from LinkedList', () => {
    expect(new ToolbarActionList() instanceof LinkedList).toBe(true);
  });
});

describe('ToolbarActions', () => {
  const add1toTail: ToolbarActionContributorCallback = actionList => actionList.addTail(1 as any);
  const add2toTail: ToolbarActionContributorCallback = actionList => actionList.addTail(2 as any);
  const add3toTail: ToolbarActionContributorCallback = actionList => actionList.addTail(3 as any);
  const dropIndex1: ToolbarActionContributorCallback = actionList => actionList.dropByIndex(1);

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
        const creator = new ToolbarActions(callbackList);
        expect(creator.actions.toString()).toBe(expected);
      },
    );
  });

  describe('#addContributor', () => {
    const creator = new ToolbarActions([]);

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

describe('ToolbarActionsFactory', () => {
  describe('#get', () => {
    it('should create and return an ToolbarActions instance', () => {
      const entityActions = new ToolbarActionsFactory();
      const creator = entityActions.get('');

      expect(creator).toBeInstanceOf(ToolbarActions);
    });

    it('should store and pass contributorCallbacks to ToolbarActionsCreator instance it returns', () => {
      const entityActions = new ToolbarActionsFactory();
      const creatorX1 = entityActions.get('X');

      expect(creatorX1).toBeInstanceOf(ToolbarActions);
      expect(creatorX1.actions.toArray()).toHaveLength(0);

      creatorX1.addContributor(actionList => actionList.addTail(1 as any));

      const creatorX2 = entityActions.get('X');
      expect(creatorX2.actions.toArray()).toHaveLength(1);
    });
  });
});

describe('ToolbarAction', () => {
  it('should be created when options object is passed as argument', () => {
    const options = {
      text: 'TEXT',
      action: () => 'ACTION',
      permission: 'PERMISSION',
      visible: () => false,
      icon: 'ICON',
    };

    const action = new ToolbarAction(options);

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

    const action = new ToolbarAction(options);

    expect(action.text).toBe(options.text);
    expect(action.action).toBe(options.action);
    expect(action.permission).toBe('');
    expect(action.visible(null)).toBe(true);
    expect(action.icon).toBe('');
  });

  describe('#create', () => {
    it('should return a new instance from given options', () => {
      const options = {
        text: 'TEXT',
        action: () => 'ACTION',
      };

      const action = ToolbarAction.create(options);

      expect(action).toBeInstanceOf(ToolbarAction);
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

      const [action1, action2] = ToolbarAction.createMany([options1, options2]);

      expect(action1).toBeInstanceOf(ToolbarAction);
      expect(action1.text).toBe(options1.text);
      expect(action1.action).toBe(options1.action);
      expect(action2).toBeInstanceOf(ToolbarAction);
      expect(action2.text).toBe(options2.text);
      expect(action2.action).toBe(options2.action);
    });
  });
});
class MockComponent1 {}
class MockComponent2 {}

describe('ToolbarComponent', () => {
  it('should be created when options object is passed as argument', () => {
    const options = {
      component: MockComponent1,
      action: () => 'ACTION',
      permission: 'PERMISSION',
      visible: () => false,
    };

    const action = new ToolbarComponent(options);

    expect(action.component).toBe(options.component);
    expect(action.action).toBe(options.action);
    expect(action.permission).toBe(options.permission);
    expect(action.visible).toBe(options.visible);
  });

  it('should be created when only required options are passed', () => {
    const options = {
      component: MockComponent1,
    };

    const action = new ToolbarComponent(options);

    expect(action.component).toBe(options.component);
    expect(action.action(null)).toBeUndefined();
    expect(action.permission).toBe('');
    expect(action.visible()).toBe(true);
  });

  describe('#create', () => {
    it('should return a new instance from given options', () => {
      const options = {
        component: MockComponent1,
      };

      const action = ToolbarComponent.create(options);

      expect(action).toBeInstanceOf(ToolbarComponent);
      expect(action.component).toBe(options.component);
    });
  });

  describe('#createMany', () => {
    it('should return multiple instances from given options array', () => {
      const options1 = {
        component: MockComponent1,
      };
      const options2 = {
        component: MockComponent2,
      };

      const [action1, action2] = ToolbarComponent.createMany([options1, options2]);

      expect(action1).toBeInstanceOf(ToolbarComponent);
      expect(action1.component).toBe(options1.component);
      expect(action2).toBeInstanceOf(ToolbarComponent);
      expect(action2.component).toBe(options2.component);
    });
  });
});
