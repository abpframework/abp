import { Injectable, Type } from '@angular/core';
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
  RouterEvent,
} from '@angular/router';
import { filter } from 'rxjs/operators';

export const NavigationEvent = {
  Cancel: NavigationCancel,
  End: NavigationEnd,
  Error: NavigationError,
  Start: NavigationStart,
};

@Injectable({ providedIn: 'root' })
export class RouterEvents {
  constructor(private router: Router) {}

  getEvents<T extends RouterEventConstructors>(...eventTypes: T) {
    type FilteredRouterEvent = T extends Type<infer Ctor>[] ? Ctor : never;

    const filterRouterEvents = (event: RouterEvent): event is FilteredRouterEvent =>
      eventTypes.some(type => event instanceof type);

    return this.router.events.pipe(filter(filterRouterEvents));
  }

  getNavigationEvents<T extends NavigationEventKeys>(...navigationEventKeys: T) {
    type FilteredNavigationEvent = T extends (infer Key)[]
      ? Key extends NavigationEventKey
        ? InstanceType<NavigationEventType[Key]>
        : never
      : never;

    const filterNavigationEvents = (event: RouterEvent): event is FilteredNavigationEvent =>
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
