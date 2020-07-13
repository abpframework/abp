import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Action, createSelector, Selector, State, StateContext } from '@ngxs/store';
import snq from 'snq';
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
        return snq(() => state.replaceableComponents.find(component => component.key === key));
      },
    );

    return selector;
  }

  constructor(private router: Router) {}

  @Action(AddReplaceableComponent)
  async replaceableComponentsAction(
    { getState, patchState }: StateContext<ReplaceableComponents.State>,
    { payload, reload }: AddReplaceableComponent,
  ) {
    let { replaceableComponents } = getState();

    const index = snq(
      () => replaceableComponents.findIndex(component => component.key === payload.key),
      -1,
    );
    if (index > -1) {
      replaceableComponents[index] = payload;
    } else {
      replaceableComponents = [...replaceableComponents, payload];
    }

    patchState({
      replaceableComponents,
    });

    if (reload) {
      // TODO: Create a shared service for route reload and more
      const { shouldReuseRoute } = this.router.routeReuseStrategy;
      this.router.routeReuseStrategy.shouldReuseRoute = () => false;
      this.router.navigated = false;

      await this.router.navigateByUrl(this.router.url).catch();
      this.router.routeReuseStrategy.shouldReuseRoute = shouldReuseRoute;
    }
  }
}
