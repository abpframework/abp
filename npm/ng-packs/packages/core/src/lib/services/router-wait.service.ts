import { Injectable } from '@angular/core';
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
} from '@angular/router';
import { filter } from 'rxjs/operators';
import { InternalStore } from '../utils/internal-store-utils';

export interface RouterWaitState {
  loading: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class RouterWaitService {
  private store = new InternalStore<RouterWaitState>({ loading: false });
  constructor(private router: Router) {
    this.router.events
      .pipe(
        filter(
          event =>
            event instanceof NavigationStart ||
            event instanceof NavigationEnd ||
            event instanceof NavigationError ||
            event instanceof NavigationCancel,
        ),
      )
      .subscribe(event => {
        if (event instanceof NavigationStart) this.setLoading(true);
        else this.setLoading(false);
      });
  }

  getLoading() {
    return this.store.state.loading;
  }

  getLoading$() {
    return this.store.sliceState(({ loading }) => loading);
  }

  updateLoading$() {
    return this.store.sliceUpdate(({ loading }) => loading);
  }

  setLoading(loading: boolean) {
    this.store.patch({ loading });
  }
}
