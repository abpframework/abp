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

export type NavigationEventType = 'Cancel' | 'End' | 'Error' | 'Start';

@Injectable({ providedIn: 'root' })
export class NavigationEvents {
  private eventTypes = new Map<NavigationEventType, Type<RouterEvent>>([
    ['Cancel', NavigationCancel],
    ['End', NavigationEnd],
    ['Error', NavigationError],
    ['Start', NavigationStart],
  ]);

  constructor(private router: Router) {}

  getOneOf(...eventTypes: NavigationEventType[]) {
    return this.router.events.pipe(
      filter((event: RouterEvent) =>
        eventTypes.some(type => event instanceof this.eventTypes.get(type)),
      ),
    );
  }

  getAny() {
    return this.getOneOf(...this.eventTypes.keys());
  }
}
