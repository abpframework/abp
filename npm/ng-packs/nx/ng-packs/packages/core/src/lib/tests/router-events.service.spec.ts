import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  ResolveEnd,
  ResolveStart,
  Router,
  RouterEvent,
} from '@angular/router';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';
import { NavigationEventKey, RouterEvents } from '../services/router-events.service';

describe('RouterEvents', () => {
  let spectator: SpectatorService<RouterEvents>;
  let service: RouterEvents;
  const events = new Subject<RouterEvent>();
  const emitRouterEvents = () => {
    events.next(new RouterEvent(0, null));
    events.next(new NavigationStart(1, null, null));
    events.next(new ResolveStart(2, null, null, null));
    events.next(new RouterEvent(3, null));
    events.next(new NavigationError(4, null, null));
    events.next(new NavigationEnd(5, null, null));
    events.next(new ResolveEnd(6, null, null, null));
    events.next(new NavigationCancel(7, null, null));
  };

  const createService = createServiceFactory({
    service: RouterEvents,
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

  describe('getNavigationEvents', () => {
    test.each`
      filtered               | expected
      ${['Start', 'Cancel']} | ${[1, 7]}
      ${['Error', 'Cancel']} | ${[4, 7]}
      ${['Start', 'End']}    | ${[1, 5]}
      ${['Error', 'End']}    | ${[4, 5]}
    `(
      'should return a stream of given navigation events',
      ({ filtered, expected }: NavigationEventTest) => {
        const stream = service.getNavigationEvents(...filtered);
        const collected: number[] = [];

        stream.pipe(take(2)).subscribe(event => collected.push(event.id));

        emitRouterEvents();

        expect(collected).toEqual(expected);
      },
    );
  });

  describe('getAnyNavigationEvent', () => {
    it('should return a stream of any navigation event', () => {
      const stream = service.getAllNavigationEvents();
      const collected: number[] = [];

      stream.pipe(take(4)).subscribe(event => collected.push(event.id));

      emitRouterEvents();

      expect(collected).toEqual([1, 4, 5, 7]);
    });
  });

  describe('getEvents', () => {
    it('should return a stream of given router events', () => {
      const stream = service.getEvents(ResolveEnd, ResolveStart);
      const collected: number[] = [];

      stream.pipe(take(2)).subscribe(event => collected.push(event.id));

      emitRouterEvents();

      expect(collected).toEqual([2, 6]);
    });
  });

  describe('getAnyEvent', () => {
    it('should return a stream of any router event', () => {
      const stream = service.getAllEvents();
      const collected: number[] = [];

      stream.pipe(take(8)).subscribe((event: RouterEvent) => collected.push(event.id));

      emitRouterEvents();

      expect(collected).toEqual([0, 1, 2, 3, 4, 5, 6, 7]);
    });
  });
});

interface NavigationEventTest {
  filtered: [NavigationEventKey, ...NavigationEventKey[]];
  expected: number[];
}
