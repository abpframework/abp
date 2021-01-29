import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
  RouterEvent,
} from '@angular/router';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';
import { NavigationEvents } from '../services/navigation-events.service';

describe('NavigationEvents', () => {
  let spectator: SpectatorService<NavigationEvents>;
  let service: NavigationEvents;
  const events = new Subject<RouterEvent>();

  const createService = createServiceFactory({
    service: NavigationEvents,
    providers: [
      {
        provide: Router,
        useValue: { events },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  describe('getOneOf', () => {
    test.each`
      filtered               | expected
      ${['Start', 'Cancel']} | ${[0, 3]}
      ${['Error', 'Cancel']} | ${[0, 1]}
      ${['Start', 'End']}    | ${[2, 3]}
      ${['Error', 'End']}    | ${[1, 2]}
    `('should return a stream of given navigation events', ({ filtered, expected }) => {
      const stream = service.getOneOf(...filtered);
      const collected: number[] = [];

      stream.pipe(take(2)).subscribe(event => collected.push(event.id));

      events.next(new NavigationCancel(0, null, null));
      events.next(new NavigationError(1, null, null));
      events.next(new NavigationEnd(2, null, null));
      events.next(new NavigationStart(3, null, null));

      expect(collected).toEqual(expected);
    });
  });

  describe('getAny', () => {
    it('should return a stream of any navigation event', () => {
      const stream = service.getAny();
      const collected: number[] = [];

      stream.pipe(take(4)).subscribe(event => collected.push(event.id));

      events.next(new RouterEvent(0, null));
      events.next(new NavigationStart(1, null, null));
      events.next(new RouterEvent(2, null));
      events.next(new RouterEvent(3, null));
      events.next(new NavigationError(4, null, null));
      events.next(new NavigationEnd(5, null, null));
      events.next(new RouterEvent(6, null));
      events.next(new NavigationCancel(7, null, null));

      expect(collected).toEqual([1, 4, 5, 7]);
    });
  });
});
