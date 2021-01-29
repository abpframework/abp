import { Injectable, Injector } from '@angular/core';
import { NavigationStart } from '@angular/router';
import { of, Subject, timer } from 'rxjs';
import { map, mapTo, switchMap, takeUntil, tap } from 'rxjs/operators';
import { LOADER_DELAY } from '../tokens/lodaer-delay.token';
import { InternalStore } from '../utils/internal-store-utils';
import { RouterEvents } from './router-events.service';

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
  constructor(private routerEvents: RouterEvents, injector: Injector) {
    this.delay = injector.get(LOADER_DELAY, 500);
    this.updateLoadingStatusOnNavigationEvents();
  }

  private updateLoadingStatusOnNavigationEvents() {
    this.routerEvents
      .getAllNavigationEvents()
      .pipe(
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
