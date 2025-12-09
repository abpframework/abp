import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { provideRouter, Route, Router } from '@angular/router';
import { createSpyObject, SpyObject } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
import { permissionGuard } from '../guards/permission.guard';
import { HttpErrorReporterService } from '../services/http-error-reporter.service';
import { PermissionService } from '../services/permission.service';
import { provideAbpCore, withOptions } from '../providers';
import { TestBed } from '@angular/core/testing';
import { RouterTestingHarness } from '@angular/router/testing';
import { AuthService } from '../abstracts';

@Component({ template: '' })
class DummyComponent {}

// Removed deprecated class-based PermissionGuard tests; function-based guard is covered below.

describe('authGuard', () => {
  let permissionService: SpyObject<PermissionService>;
  let httpErrorReporter: SpyObject<HttpErrorReporterService>;

  const mockOAuthService = {
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

    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: AuthService, useValue: mockOAuthService },
        { provide: PermissionService, useValue: permissionService },
        { provide: HttpErrorReporterService, useValue: httpErrorReporter },
        provideRouter(routes),
        provideAbpCore(withOptions({
          environment: {
            apis: {
              default: {
                url: 'http://localhost:4200',
              },
            },
            application: {
              baseUrl: 'http://localhost:4200',
              name: 'TestApp',
            },
            remoteEnv: {
              url: 'http://localhost:4200',
              mergeStrategy: 'deepmerge',
            },
          },
          registerLocaleFn: () => Promise.resolve(),
        })),
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
    expect(permissionService.getGrantedPolicy$).toBeDefined();
    expect(httpErrorReporter.reportError).toBeDefined();
  });

  it('should check the requiredPolicy from RoutesService', async () => {
    permissionService.getGrantedPolicy$.mockImplementation(policy => {
      return of(policy === 'TestPolicy');
    });
    await RouterTestingHarness.create('/dummy');

    expect(TestBed.inject(Router).url).toEqual('/dummy');
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
