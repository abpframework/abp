import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { of, firstValueFrom } from 'rxjs';
import { bufferCount, take } from 'rxjs/operators';
import { ABP } from '../models';
import { ListService, QueryStreamCreatorCallback } from '../services/list.service';
import { LIST_QUERY_DEBOUNCE_TIME } from '../tokens';

describe('ListService', () => {
  let spectator: SpectatorService<ListService>;
  let service: ListService;

  const createService = createServiceFactory({
    service: ListService,
    providers: [
      {
        provide: LIST_QUERY_DEBOUNCE_TIME,
        useValue: 0,
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  describe('#filter', () => {
    it('should initially be empty string', () => {
      expect(service.filter).toBe('');
    });

    it('should be changed', () => {
      service.filter = 'foo';

      expect(service.filter).toBe('foo');
    });
  });

  describe('#maxResultCount', () => {
    it('should initially be 10', () => {
      expect(service.maxResultCount).toBe(10);
    });

    it('should be changed', () => {
      service.maxResultCount = 20;

      expect(service.maxResultCount).toBe(20);
    });
  });

  describe('#page', () => {
    it('should initially be 0', () => {
      expect(service.page).toBe(0);
    });

    it('should be changed', () => {
      service.page = 9;

      expect(service.page).toBe(9);
    });
  });

  describe('#sortKey', () => {
    it('should initially be empty string', () => {
      expect(service.sortKey).toBe('');
    });

    it('should be changed', () => {
      service.sortKey = 'foo';

      expect(service.sortKey).toBe('foo');
    });
  });

  describe('#sortOrder', () => {
    it('should initially be empty string', () => {
      expect(service.sortOrder).toBe('');
    });

    it('should be changed', () => {
      service.sortOrder = 'foo';

      expect(service.sortOrder).toBe('foo');
    });
  });

  describe('#query$', () => {
    it('should initially emit default query', async () => {
      const query = await firstValueFrom(service.query$.pipe(take(1)));
      expect(query).toEqual({
        filter: undefined,
        maxResultCount: 10,
        skipCount: 0,
        sorting: undefined,
      });
    });

    it('should emit a query based on params set', async () => {
      service.filter = 'foo';
      service.sortKey = 'bar';
      service.sortOrder = 'baz';
      service.maxResultCount = 20;
      service.page = 9;

      const query = await firstValueFrom(service.query$.pipe(take(1)));
      expect(query).toEqual({
        filter: 'foo',
        sorting: 'bar baz',
        maxResultCount: 20,
        skipCount: 180,
      });
    });
  });

  describe('#hookToQuery', () => {
    it('should call given callback with the query', async () => {
      const callback: QueryStreamCreatorCallback<ABP.PageQueryParams> = query =>
        of({ items: [query], totalCount: 1 });

      const result = await firstValueFrom(service.hookToQuery(callback));
      expect(result.items[0]).toEqual({
        filter: undefined,
        maxResultCount: 10,
        skipCount: 0,
        sorting: undefined,
      });
    });

    it('should emit isLoading as side effect', async () => {
      const callback: QueryStreamCreatorCallback<ABP.PageQueryParams> = query =>
        of({ items: [query], totalCount: 1 });

      // Subscribe to capture the sequence: false (idle) -> true (loading) -> false (idle after completion)
      const loadingPromise = firstValueFrom(service.isLoading$.pipe(bufferCount(3)));
      const hookSubscription = service.hookToQuery(callback).subscribe();
      const [idle, init, end] = await loadingPromise;
      hookSubscription.unsubscribe();

      expect(idle).toBe(false);
      expect(init).toBe(true);
      expect(end).toBe(false);
    });

    it('should emit requestStatus as side effect', async () => {
      const callback: QueryStreamCreatorCallback<ABP.PageQueryParams> = query =>
        of({ items: [query], totalCount: 1 });

      // Subscribe to capture the sequence: 'idle' -> 'loading' -> 'success'
      const statusPromise = firstValueFrom(service.requestStatus$.pipe(bufferCount(3)));
      const hookSubscription = service.hookToQuery(callback).subscribe();
      const [idle, init, end] = await statusPromise;
      hookSubscription.unsubscribe();

      expect(idle).toBe('idle');
      expect(init).toBe('loading');
      expect(end).toBe('success');
    });

    it('should emit error requestStatus as side effect and stop processing', async () => {
      const errCallback: QueryStreamCreatorCallback<ABP.PageQueryParams> = query => {
        throw Error('A server error occurred');
      };

      // Subscribe to capture the sequence: 'idle' -> 'loading' -> 'error'
      // Must subscribe BEFORE hookToQuery to capture the initial 'idle' value
      const statusPromise = firstValueFrom(service.requestStatus$.pipe(bufferCount(3)));

      // Subscribe to hookToQuery which will emit 'loading' and 'error'
      // The error is caught by the service's catchError, which sets status to 'error'
      const hookSubscription = service.hookToQuery(errCallback).subscribe({
        error: () => {
          // Error is expected - the service catches it and sets status to 'error'
        },
      });

      const [idle, loading, error] = await statusPromise;
      hookSubscription.unsubscribe();

      expect(idle).toBe('idle');
      expect(loading).toBe('loading');
      expect(error).toBe('error');
    });
  });
});
