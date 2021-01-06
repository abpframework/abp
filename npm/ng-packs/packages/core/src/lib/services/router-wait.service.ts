import { Injectable, Injector } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { filter, map, mapTo, switchMap, takeUntil, tap } from 'rxjs/operators';
import { InternalStore } from '../utils/internal-store-utils';
import { of, Subject, timer } from 'rxjs';
import { LOADER_DELAY } from '../tokens/lodaer-delay.token';

export interface RouterWaitState {
  loading: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class RouterWaitService {
  private store = new InternalStore<RouterWaitState>({ loading: false });
  private destroy$ = new Subject();
  private delay: number;
  constructor(private router: Router, injector: Injector) {
    this.delay = injector.get(LOADER_DELAY, 500);
    this.router.events
      .pipe(
        filter(
          event =>
            event instanceof NavigationStart ||
            event instanceof NavigationEnd ||
            event instanceof NavigationError ||
            event instanceof NavigationCancel,
        ),
        map(event => event instanceof NavigationStart),
        switchMap(condition =>
          condition
            ? this.delay === 0
              ? of(true)
              : timer(this.delay || 0).pipe(mapTo(true), takeUntil(this.destroy$))
            : of(false),
        ),
        tap(() => this.destroy$.next()),
      )
      .subscribe(status => {
        this.setLoading(status);
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
