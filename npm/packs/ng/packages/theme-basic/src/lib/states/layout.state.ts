import { State, Action, StateContext, Selector } from '@ngxs/store';
import { LayoutAddNavigationElement } from '../actions/layout.actions';
import { Layout } from '../models/layout';
import { TemplateRef } from '@angular/core';
import snq from 'snq';

@State<Layout.State>({
  name: 'LayoutState',
  defaults: { navigationElements: [] } as Layout.State,
})
export class LayoutState {
  @Selector()
  static getNavigationElements({ navigationElements }: Layout.State): Layout.NavigationElement[] {
    return navigationElements;
  }

  @Action(LayoutAddNavigationElement)
  layoutAction({ getState, patchState }: StateContext<Layout.State>, { payload = [] }: LayoutAddNavigationElement) {
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
}
