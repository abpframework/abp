import {
  EntityAction,
  EntityActionContributorCallbacks,
  EntityActionDefaults,
  EntityActionsFactory,
} from '../lib/models/entity-actions';
import { mergeWithDefaultActions } from '../lib/utils/actions.util';

describe('Entity Action Utils', () => {
  describe('#mergeEntityActions', () => {
    let entityActions: EntityActionsFactory;

    beforeEach(() => {
      entityActions = new EntityActionsFactory();
    });

    it('should merge default actions with action contributors', () => {
      const defaults: EntityActionDefaults = {
        x: [1 as any as EntityAction, 2 as any as EntityAction, 3 as any as EntityAction],
        y: [1 as any as EntityAction, 2 as any as EntityAction, 3 as any as EntityAction],
      };

      const contributors1: EntityActionContributorCallbacks = {
        x: [
          actionList => {
            const x2 = actionList.dropByIndex(1); // 1 <-> 3
            actionList.addHead(x2.value); // 2 <-> 1 <-> 3
          },
          actionList => {
            actionList.dropTail(); // 2 <-> 1
          },
        ],
      };

      const contributors2: EntityActionContributorCallbacks = {
        y: [
          actionList => {
            const y2 = actionList.dropByIndex(1); // 1 <-> 3
            actionList.addTail(y2.value); // 1 <-> 3 <-> 2
          },
          actionList => {
            actionList.dropHead(); // 3 <-> 2
          },
        ],
      };

      mergeWithDefaultActions(entityActions, defaults, contributors1, contributors2);

      expect(entityActions.get('x').actions.toString()).toBe('2 <-> 1');
      expect(entityActions.get('y').actions.toString()).toBe('3 <-> 2');
    });
  });
});
