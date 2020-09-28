import { APP_BASE_HREF } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Actions, Store } from '@ngxs/store';
import { of } from 'rxjs';
import { RestOccurError } from '../actions';
import { PermissionGuard } from '../guards/permission.guard';
import { RoutesService } from '../services/routes.service';

describe('PermissionGuard', () => {
  let spectator: SpectatorService<PermissionGuard>;
  let guard: PermissionGuard;
  let routes: SpyObject<RoutesService>;
  let store: SpyObject<Store>;

  @Component({ template: '' })
  class DummyComponent {}

  const createService = createServiceFactory({
    service: PermissionGuard,
    mocks: [Store],
    declarations: [DummyComponent],
    imports: [
      RouterModule.forRoot([
        {
          path: 'test',
          component: DummyComponent,
          data: {
            requiredPolicy: 'TestPolicy',
          },
        },
      ]),
    ],
    providers: [
      {
        provide: APP_BASE_HREF,
        useValue: '/',
      },
      {
        provide: Actions,
        useValue: {
          pipe() {
            return of(null);
          },
        },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    guard = spectator.service;
    routes = spectator.inject(RoutesService);
    store = spectator.inject(Store);
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

  it('should check the requiredPolicy from RoutesService', done => {
    routes.add([
      {
        path: '/test',
        name: 'Test',
        requiredPolicy: 'TestPolicy',
      },
    ]);
    store.select.andReturn(of(false));
    const spy = jest.spyOn(store, 'select');
    guard.canActivate({ data: {} } as any, { url: 'test' } as any).subscribe(() => {
      expect(spy.mock.calls[0][0]({ auth: { grantedPolicies: { TestPolicy: true } } })).toBe(true);
      done();
    });
  });

  it('should return Observable<true> if RoutesService does not have requiredPolicy for given URL', done => {
    routes.add([
      {
        path: '/test',
        name: 'Test',
      },
    ]);
    guard.canActivate({ data: {} } as any, { url: 'test' } as any).subscribe(result => {
      expect(result).toBe(true);
      done();
    });
  });
});
