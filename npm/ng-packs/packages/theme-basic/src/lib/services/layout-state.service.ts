import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { LayoutState } from '../states/layout.state';
import { AddNavigationElement, RemoveNavigationElementByName } from '../actions';
import { Layout } from '../models/layout';

@Injectable()
export class LayoutStateService {
  constructor(private store: Store) {}

  getNavigationElements() {
    return this.store.selectSnapshot(LayoutState.getNavigationElements);
  }

  dispatchAddNavigationElement(payload: Layout.NavigationElement | Layout.NavigationElement[]) {
    return this.store.dispatch(new AddNavigationElement(payload));
  }

  dispatchRemoveNavigationElementByName(name: string) {
    return this.store.dispatch(new RemoveNavigationElementByName(name));
  }
}
