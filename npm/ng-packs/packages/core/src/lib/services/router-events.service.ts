import { Injectable, Type, inject, signal } from '@angular/core';
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
  RouterEvent,
  Event,
  RouterState,
} from '@angular/router';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/operators';

export const NavigationEvent = {
  Cancel: NavigationCancel,
  End: NavigationEnd,
  Error: NavigationError,
  Start: NavigationStart,
};

@Injectable({ providedIn: 'root' })
export class RouterEvents {
  protected readonly router = inject(Router);

  readonly #previousNavigation = signal<string | undefined>(undefined);
  previousNavigation = this.#previousNavigation.asReadonly();

  readonly #currentNavigation = signal<string | undefined>(undefined);
  currentNavigation = this.#currentNavigation.asReadonly();

  constructor() {
    this.listenToNavigation();
  }

  protected listenToNavigation(): void {
    const routerEvent$ = this.router.events.pipe(
      filter(e => e instanceof NavigationEvent.End && !e.url.includes('error'))
    ) as Observable<NavigationEnd>;
    
    routerEvent$.subscribe(event => {
      this.#previousNavigation.set(this.currentNavigation());
      this.#currentNavigation.set(event.url);
    });
  }

  getEvents<T extends RouterEventConstructors>(...eventTypes: T) {
    const filterRouterEvents = (event: Event) => eventTypes.some(type => event instanceof type);

    return this.router.events.pipe(filter(filterRouterEvents));
  }

  getNavigationEvents<T extends NavigationEventKeys>(...navigationEventKeys: T) {
    type FilteredNavigationEvent = T extends (infer Key)[]
      ? Key extends NavigationEventKey
        ? InstanceType<NavigationEventType[Key]>
        : never
      : never;

    const filterNavigationEvents = (event: Event): event is FilteredNavigationEvent =>
      navigationEventKeys.some(key => event instanceof NavigationEvent[key]);

    return this.router.events.pipe(filter(filterNavigationEvents));
  }

  getAllEvents() {
    return this.router.events;
  }

  getAllNavigationEvents() {
    const keys = Object.keys(NavigationEvent) as NavigationEventKeys;
    return this.getNavigationEvents(...keys);
  }
}

type RouterEventConstructors = [Type<RouterEvent>, ...Type<RouterEvent>[]];

type NavigationEventKeys = [NavigationEventKey, ...NavigationEventKey[]];

type NavigationEventType = typeof NavigationEvent;

export type NavigationEventKey = keyof NavigationEventType;
