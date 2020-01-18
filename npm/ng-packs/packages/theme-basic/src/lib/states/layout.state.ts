import { Action, Selector, State, StateContext } from '@ngxs/store';
import snq from 'snq';
import { AddNavigationElement, RemoveNavigationElementByName } from '../actions/layout.actions';
import { Layout } from '../models/layout';

@State<Layout.State>({
  name: 'LayoutState',
  defaults: { navigationElements: [] } as Layout.State,
})
export class LayoutState {
  @Selector()
  static getNavigationElements({ navigationElements }: Layout.State): Layout.NavigationElement[] {
    return navigationElements;
  }

  @Action(AddNavigationElement)
  layoutAddAction(
    { getState, patchState }: StateContext<Layout.State>,
    { payload = [] }: AddNavigationElement,
  ) {
    let { navigationElements } = getState();

    if (!Array.isArray(payload)) {
      payload = [payload];
    }

    if (navigationElements.length) {
      payload = snq(
        () =>
          (payload as Layout.NavigationElement[]).filter(
            ({ name }) => navigationElements.findIndex(nav => nav.name === name) < 0,
          ),
        [],
      );
    }

    if (!payload.length) return;

    navigationElements = [...navigationElements, ...payload]
      .map(element => ({ ...element, order: element.order || 99 }))
      .sort((a, b) => a.order - b.order);

    return patchState({
      navigationElements,
    });
  }

  @Action(RemoveNavigationElementByName)
  layoutRemoveAction(
    { getState, patchState }: StateContext<Layout.State>,
    { name }: RemoveNavigationElementByName,
  ) {
    let { navigationElements } = getState();

    const index = navigationElements.findIndex(element => element.name === name);

    if (index > -1) {
      navigationElements = navigationElements.splice(index, 1);
    }

    return patchState({
      navigationElements,
    });
  }
}
