import { Injectable, isDevMode } from '@angular/core';
import { Action, createSelector, Selector, State, StateContext } from '@ngxs/store';
import snq from 'snq';
import { AddReplaceableComponent } from '../actions/replaceable-components.actions';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';

function logDeprecationMsg() {
  if (isDevMode()) {
    console.warn(`
     ReplacableComponentsState has been deprecated. Use ReplaceableComponentsService instead.
     See the doc https://docs.abp.io/en/abp/latest/UI/Angular/Component-Replacement
     `);
  }
}

// tslint:disable: max-line-length
/**
 * @deprecated To be deleted in v4.0. Use ReplaceableComponentsService instead. See the doc (https://docs.abp.io/en/abp/latest/UI/Angular/Component-Replacement)
 */
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
    logDeprecationMsg();
    return replaceableComponents || [];
  }

  static getComponent(key: string) {
    const selector = createSelector(
      [ReplaceableComponentsState],
      (state: ReplaceableComponents.State): ReplaceableComponents.ReplaceableComponent => {
        logDeprecationMsg();
        return snq(() => state.replaceableComponents.find(component => component.key === key));
      },
    );

    return selector;
  }

  constructor(private service: ReplaceableComponentsService) {}

  @Action(AddReplaceableComponent)
  replaceableComponentsAction(
    { getState, patchState }: StateContext<ReplaceableComponents.State>,
    { payload, reload }: AddReplaceableComponent,
  ) {
    logDeprecationMsg();

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

    this.service.add(payload, reload);
  }
}
