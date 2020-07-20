import { Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { Action, createSelector, Selector, State, StateContext } from '@ngxs/store';
import snq from 'snq';
import { AddReplaceableComponent } from '../actions/replaceable-components.actions';
import { ReplaceableComponents } from '../models/replaceable-components';
import { noop } from '../utils/common-utils';

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

  constructor(private ngZone: NgZone, private router: Router) {}

  // TODO: Create a shared service for route reload and more
  private reloadRoute() {
    const { shouldReuseRoute } = this.router.routeReuseStrategy;
    const setRouteReuse = (reuse: typeof shouldReuseRoute) => {
      this.router.routeReuseStrategy.shouldReuseRoute = reuse;
    };

    setRouteReuse(() => false);
    this.router.navigated = false;

    this.ngZone.run(async () => {
      await this.router.navigateByUrl(this.router.url).catch(noop);
      setRouteReuse(shouldReuseRoute);
    });
  }

  @Action(AddReplaceableComponent)
  replaceableComponentsAction(
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

    if (reload) this.reloadRoute();
  }
}
