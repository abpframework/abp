import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { of } from 'rxjs';
import { PermissionGuard } from '../guards/permission.guard';
import { RestOccurError } from '../actions';

describe('PermissionGuard', () => {
  let spectator: SpectatorService<PermissionGuard>;
  let guard: PermissionGuard;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({
    service: PermissionGuard,
    mocks: [Store],
  });

  beforeEach(() => {
    spectator = createService();
    guard = spectator.service;
    store = spectator.get(Store);
  });

  it('should return true when the grantedPolicy is true', done => {
    store.select.andReturn(of(true));
    const spy = jest.spyOn(store, 'dispatch');
    guard.canActivate({ data: { requiredPolicy: 'test' } } as any, null).subscribe(res => {
      expect(res).toBe(true);
      expect(spy.mock.calls).toHaveLength(0);
      done();
    });
  });

  it('should return false and dispatch RestOccurError when the grantedPolicy is false', done => {
    store.select.andReturn(of(false));
    const spy = jest.spyOn(store, 'dispatch');
    guard.canActivate({ data: { requiredPolicy: 'test' } } as any, null).subscribe(res => {
      expect(res).toBe(false);
      expect(spy.mock.calls[0][0] instanceof RestOccurError).toBeTruthy();
      expect((spy.mock.calls[0][0] as RestOccurError).payload).toEqual({ status: 403 });
      done();
    });
  });

  it('should find the requiredPolicy from child route', done => {
    store.select.andReturn(of(false));
    const spy = jest.spyOn(store, 'select');
    guard
      .canActivate(
        { data: {}, routeConfig: { children: [{ path: 'test', data: { requiredPolicy: 'TestPolicy' } }] } } as any,
        { url: 'test' } as any,
      )
      .subscribe(() => {
        expect(spy.mock.calls[0][0]({ auth: { grantedPolicies: { TestPolicy: true } } })).toBe(true);
        done();
      });
  });
});
