import { ActionContributorCallback, ActionList, ActionsFactory } from '../models/actions';
import {
  EntityActionContributorCallbacks,
  EntityActionDefaults,
  EntityActions,
  EntityActionsFactory,
} from '../models/entity-actions';
import {
  ToolbarActionContributorCallbacks,
  ToolbarActionDefaults,
  ToolbarActions,
  ToolbarActionsFactory,
} from '../models/toolbar-actions';

export function mergeWithDefaultActions<F extends ActionsFactory<any>>(
  extension: F,
  defaultActions: InferredActionDefaults<F>,
  ...contributors: InferredActionContributorCallbacks<F>[]
) {
  Object.keys(defaultActions).forEach((name: string) => {
    const actions: InferredActions<F> = extension.get(name);
    actions.clearContributors();
    actions.addContributor((actionList: ActionList) =>
      actionList.addManyTail(defaultActions[name]),
    );
    contributors.forEach(contributor =>
      (contributor[name] || []).forEach((callback: ActionContributorCallback<any>) =>
        actions.addContributor(callback),
      ),
    );
  });
}
type InferredActionDefaults<F> = F extends EntityActionsFactory<infer RE>
  ? EntityActionDefaults<RE>
  : F extends ToolbarActionsFactory<infer RT>
  ? ToolbarActionDefaults<RT>
  : never;

type InferredActionContributorCallbacks<F> = F extends EntityActionsFactory<infer RE>
  ? EntityActionContributorCallbacks<RE>
  : F extends ToolbarActionsFactory<infer RT>
  ? ToolbarActionContributorCallbacks<RT>
  : never;

type InferredActions<F> = F extends EntityActionsFactory<infer RE>
  ? EntityActions<RE>
  : F extends ToolbarActionsFactory<infer RT>
  ? ToolbarActions<RT>
  : never;
