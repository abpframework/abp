import { State, Action, StateContext, Selector, createSelector } from '@ngxs/store';
import { AddReplaceableComponent } from '../actions/replaceable-components.actions';
import { ReplaceableComponents } from '../models/replaceable-components';
import snq from 'snq';

@State<ReplaceableComponents.State>({
  name: 'ReplaceableComponentsState',
  defaults: { replaceableComponents: [] } as ReplaceableComponents.State,
})
export class ReplaceableComponentsState {
  @Selector()
  static getAll({
    replaceableComponents,
  }: ReplaceableComponents.State): ReplaceableComponents.Data[] {
    return replaceableComponents || [];
  }

  static getOne(key: string) {
    const selector = createSelector(
      [ReplaceableComponentsState],
      ({ replaceableComponents }: ReplaceableComponents.State) => {
        return snq(() => replaceableComponents.find(component => component.key === key));
      },
    );

    return selector;
  }

  @Action(AddReplaceableComponent)
  replaceableComponentsAction(
    { getState, patchState }: StateContext<ReplaceableComponents.State>,
    { payload }: AddReplaceableComponent,
  ) {
    if (!Array.isArray(payload)) {
      payload = [payload];
    }

    const { replaceableComponents } = getState();

    patchState({
      replaceableComponents: [...replaceableComponents, ...payload],
    });
  }
}
