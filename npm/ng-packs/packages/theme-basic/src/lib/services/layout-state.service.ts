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

  dispatchAddNavigationElement(...args: ConstructorParameters<typeof AddNavigationElement>) {
    return this.store.dispatch(new AddNavigationElement(...args));
  }

  dispatchRemoveNavigationElementByName(
    ...args: ConstructorParameters<typeof RemoveNavigationElementByName>
  ) {
    return this.store.dispatch(new RemoveNavigationElementByName(...args));
  }
}
