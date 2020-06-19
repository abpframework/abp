import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector, createSelector } from '@ngxs/store';
import { AddReplaceableComponent } from '../actions/replaceable-components.actions';
import { ReplaceableComponents } from '../models/replaceable-components';

@State<ReplaceableComponents.State>({
  name: 'ReplaceableComponentsState',
  defaults: { replaceableComponents: [] } as ReplaceableComponents.State,
})
@Injectable()
export class ReplaceableComponentsState {
  @Selector()
  static getAll({
    replaceableComponents,
  }: ReplaceableComponents.State): ReplaceableComponents.ReplaceableComponent[] {
    return replaceableComponents || [];
  }

  static getComponent(key: string) {
    const selector = createSelector(
      [ReplaceableComponentsState],
      (state: ReplaceableComponents.State): ReplaceableComponents.ReplaceableComponent => {
        return state.replaceableComponents.find(component => component.key === key);
      },
    );

    return selector;
  }

  @Action(AddReplaceableComponent)
  replaceableComponentsAction(
    { getState, patchState }: StateContext<ReplaceableComponents.State>,
    { payload }: AddReplaceableComponent,
  ) {
    let { replaceableComponents } = getState();

    const index = replaceableComponents.findIndex(component => component.key === payload.key);
    if (index > -1) {
      replaceableComponents[index] = payload;
    } else {
      replaceableComponents = [...replaceableComponents, payload];
    }

    patchState({
      replaceableComponents,
    });
  }
}
