import { APP_BASE_HREF } from '@angular/common';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
import { PermissionGuard } from '../guards/permission.guard';
import { HttpErrorReporterService, PermissionService } from '../services';
import { RoutesService } from '../services/routes.service';
import { CORE_OPTIONS } from '../tokens';

describe('PermissionGuard', () => {
  let spectator: SpectatorService<PermissionGuard>;
  let guard: PermissionGuard;
  let routes: SpyObject<RoutesService>;
  let httpErrorReporter: SpyObject<HttpErrorReporterService>;
  let permissionService: SpyObject<PermissionService>;

  @Component({ template: '' })
  class DummyComponent {}

  const createService = createServiceFactory({
    service: PermissionGuard,
    mocks: [PermissionService, Store],
    declarations: [DummyComponent],
    imports: [
      HttpClientTestingModule,
      RouterModule.forRoot(
        [
          {
            path: 'test',
            component: DummyComponent,
            data: {
              requiredPolicy: 'TestPolicy',
            },
          },
        ],
        { relativeLinkResolution: 'legacy' },
      ),
    ],
    providers: [
      {
        provide: APP_BASE_HREF,
        useValue: '/',
      },
      { provide: CORE_OPTIONS, useValue: { skipGetAppConfiguration: true } },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    guard = spectator.service;
    routes = spectator.inject(RoutesService);
    httpErrorReporter = spectator.inject(HttpErrorReporterService);
    permissionService = spectator.inject(PermissionService);
  });

  it('should return true when the grantedPolicy is true', done => {
    permissionService.getGrantedPolicy$.andReturn(of(true));
    const spy = jest.spyOn(httpErrorReporter, 'reportError');
    guard.canActivate({ data: { requiredPolicy: 'test' } } as any, null).subscribe(res => {
      expect(res).toBe(true);
      expect(spy.mock.calls).toHaveLength(0);
      done();
    });
  });

  it('should return false and report an error when the grantedPolicy is false', done => {
    permissionService.getGrantedPolicy$.andReturn(of(false));
    const spy = jest.spyOn(httpErrorReporter, 'reportError');
    guard.canActivate({ data: { requiredPolicy: 'test' } } as any, null).subscribe(res => {
      expect(res).toBe(false);
      expect(spy.mock.calls[0][0]).toEqual({
        status: 403,
      });
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
    permissionService.getGrantedPolicy$.mockImplementation(policy => of(policy === 'TestPolicy'));
    guard.canActivate({ data: {} } as any, { url: 'test' } as any).subscribe(result => {
      expect(result).toBe(true);
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
