import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { LayoutState } from '../states/layout.state';

@Injectable()
export class LayoutStateService {
  constructor(private store: Store) {}

  getNavigationElements() {
    return this.store.selectSnapshot(LayoutState.getNavigationElements);
  }
}
