import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
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
    it('should initially emit default query', done => {
      service.query$.pipe(take(1)).subscribe(query => {
        expect(query).toEqual({
          filter: undefined,
          maxResultCount: 10,
          skipCount: 0,
          sorting: undefined,
        });

        done();
      });
    });

    it('should emit a query based on params set', done => {
      service.filter = 'foo';
      service.sortKey = 'bar';
      service.sortOrder = 'baz';
      service.maxResultCount = 20;
      service.page = 9;

      service.query$.pipe(take(1)).subscribe(query => {
        expect(query).toEqual({
          filter: 'foo',
          sorting: 'bar baz',
          maxResultCount: 20,
          skipCount: 180,
        });

        done();
      });
    });
  });

  describe('#hookToQuery', () => {
    it('should call given callback with the query', done => {
      const callback: QueryStreamCreatorCallback<ABP.PageQueryParams> = query =>
        of({ items: [query], totalCount: 1 });

      service.hookToQuery(callback).subscribe(({ items: [query] }) => {
        expect(query).toEqual({
          filter: undefined,
          maxResultCount: 10,
          skipCount: 0,
          sorting: undefined,
        });

        done();
      });
    });

    it('should emit isLoading as side effect', done => {
      const callback: QueryStreamCreatorCallback<ABP.PageQueryParams> = query =>
        of({ items: [query], totalCount: 1 });

      service.isLoading$.pipe(bufferCount(3)).subscribe(([idle, init, end]) => {
        expect(idle).toBe(false);
        expect(init).toBe(true);
        expect(end).toBe(false);

        done();
      });

      service.hookToQuery(callback).subscribe();
    });
  });
});
