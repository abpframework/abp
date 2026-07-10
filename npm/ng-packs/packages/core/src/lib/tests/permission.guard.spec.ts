import { Component } from '@angular/core';
import { provideRouter, Route, Router } from '@angular/router';
import { RouterTestingHarness } from '@angular/router/testing';
import { TestBed } from '@angular/core/testing';
import { createSpyObject, SpyObject } from '@ngneat/spectator/vitest';
import { of } from 'rxjs';
import { permissionGuard } from '../guards/permission.guard';
import { HttpErrorReporterService } from '../services/http-error-reporter.service';
import { PermissionService } from '../services/permission.service';
import { AuthService } from '../abstracts';
import { ConfigStateService, RouteBasedCultureUrlService, RoutesService } from '../services';

@Component({ template: '' })
class DummyComponent {}

// Removed deprecated class-based PermissionGuard tests; function-based guard is covered below.

describe('authGuard', () => {
  let permissionService: SpyObject<PermissionService>;
  let httpErrorReporter: SpyObject<HttpErrorReporterService>;
  let routesService: Pick<RoutesService, 'find'>;
  let routeCultureUrl: Pick<RouteBasedCultureUrlService, 'getRoutePathForMatching'>;
  let configStateService: Pick<ConfigStateService, 'getAll$'>;

  const authService = {
    isAuthenticated: true,
  };

  const routes: Route[] = [
    {
      path: 'dummy',
      component: DummyComponent,
      canActivate: [permissionGuard],
      data: {
        requiredPolicy: 'TestPolicy',
      },
    },
    {
      path: 'zibzib',
      component: DummyComponent,
      canActivate: [permissionGuard],
    },
    {
      path: 'redirect-test',
      component: DummyComponent,
      canActivate: [permissionGuard],
      data: {
        requiredPolicy: 'TestPolicy',
        redirectUrl: '/zibzib',
      },
    },
  ];

  beforeEach(() => {
    httpErrorReporter = createSpyObject(HttpErrorReporterService);
    permissionService = createSpyObject(PermissionService);
    permissionService.getGrantedPolicy$.andReturn(of(true));
    routesService = {
      find: vi.fn(),
    };
    routeCultureUrl = {
      getRoutePathForMatching: vi.fn((_: Router, url: string) => url),
    };
    configStateService = {
      getAll$: vi.fn(() => of({ auth: { grantedPolicies: {} } })),
    };

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: authService },
        { provide: PermissionService, useValue: permissionService },
        { provide: HttpErrorReporterService, useValue: httpErrorReporter },
        { provide: RoutesService, useValue: routesService },
        { provide: RouteBasedCultureUrlService, useValue: routeCultureUrl },
        { provide: ConfigStateService, useValue: configStateService },
        provideRouter(routes),
      ],
    });
  });

  it('should return true when the grantedPolicy is true', async () => {
    permissionService.getGrantedPolicy$.andReturn(of(true));
    await RouterTestingHarness.create('/dummy');

    expect(TestBed.inject(Router).url).toEqual('/dummy');
    expect(httpErrorReporter.reportError).not.toHaveBeenCalled();
  });

  it('should return false and report an error when the grantedPolicy is false', () => {
    permissionService.getGrantedPolicy$.andReturn(of(false));
    return RouterTestingHarness.create('/dummy').then(() => {
      expect(TestBed.inject(Router).url).toEqual('/');
      expect(httpErrorReporter.reportError).toHaveBeenCalledWith({ status: 403 });
    });
  });

  it('should check the requiredPolicy from RoutesService', async () => {
    routesService.find = vi.fn(predicate => {
      const route = { path: '/zibzib', requiredPolicy: 'TestPolicy' };
      return predicate(route) ? route : null;
    });
    permissionService.getGrantedPolicy$.mockImplementation(policy => {
      return of(policy === 'TestPolicy');
    });
    await RouterTestingHarness.create('/zibzib');

    expect(permissionService.getGrantedPolicy$).toHaveBeenCalledWith('TestPolicy');
    expect(TestBed.inject(Router).url).toEqual('/zibzib');
    expect(httpErrorReporter.reportError).not.toHaveBeenCalled();
  });

  it('should return Observable<true> if RoutesService does not have requiredPolicy for given URL', async () => {
    await RouterTestingHarness.create('/zibzib');
    expect(TestBed.inject(Router).url).toEqual('/zibzib');
  });

  it('should redirect to redirectUrl when the grantedPolicy is false and redirectUrl is provided', async () => {
    permissionService.getGrantedPolicy$.andReturn(of(false));
    await RouterTestingHarness.create('/redirect-test');

    expect(TestBed.inject(Router).url).toEqual('/zibzib');
    expect(httpErrorReporter.reportError).not.toHaveBeenCalled();
  });
});
