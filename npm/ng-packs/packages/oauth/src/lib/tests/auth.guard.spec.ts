import { createServiceFactory, SpectatorService, createSpyObject } from '@ngneat/spectator/jest';
import { OAuthService } from 'angular-oauth2-oidc';
import { AbpOAuthGuard, abpOAuthGuard } from '../guards/oauth.guard';
import { AuthService } from '@abp/ng.core';
import {
  ActivatedRouteSnapshot,
  Route,
  Router,
  RouterStateSnapshot,
  provideRouter,
} from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { RouterTestingHarness } from '@angular/router/testing';
import { SpyObject } from '@ngneat/spectator';

describe('AuthGuard', () => {
  let spectator: SpectatorService<AbpOAuthGuard>;
  let guard: AbpOAuthGuard;
  const route = createSpyObject<ActivatedRouteSnapshot>(ActivatedRouteSnapshot);
  const state = createSpyObject<RouterStateSnapshot>(RouterStateSnapshot);

  const createService = createServiceFactory({
    service: AbpOAuthGuard,
    mocks: [OAuthService, AuthService],
  });

  beforeEach(() => {
    spectator = createService();
    guard = spectator.service;
  });

  it('should return true when user logged in', () => {
    spectator.inject(OAuthService).hasValidAccessToken.andReturn(true);
    expect(guard.canActivate(route, state)).toBe(true);
  });

  it('should execute the navigateToLogin method of the authService', () => {
    const authService = spectator.inject(AuthService);
    spectator.inject(OAuthService).hasValidAccessToken.andReturn(false);
    const navigateToLoginSpy = jest.spyOn(authService, 'navigateToLogin');

    expect(guard.canActivate(route, state)).toBe(false);
    expect(navigateToLoginSpy).toHaveBeenCalled();
  });
});

@Component({ standalone: true, template: '' })
class DummyComponent {}
describe('authGuard', () => {
  let oAuthService: SpyObject<OAuthService>; 
  let authService: SpyObject<AuthService>;
  const routes: Route[] = [
    {
      path: 'dummy',
      canActivate: [abpOAuthGuard],
      component: DummyComponent,
    },
  ];

  beforeEach(() => {
    authService = createSpyObject(AuthService);
    oAuthService = createSpyObject(OAuthService);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        { provide: AuthService, useValue: authService },
        { provide: OAuthService, useValue: oAuthService },
        provideRouter(routes),
      ],
    });
  });

  it('should move to the dummy route', async () => {
    oAuthService.hasValidAccessToken.andReturn(true);
    await RouterTestingHarness.create('/dummy');

    expect(TestBed.inject(Router).url).toEqual('/dummy');
  });

  it("should'nt move to the dummy route", async () => {
    oAuthService.hasValidAccessToken.andReturn(false);
    await RouterTestingHarness.create('/dummy');

    expect(authService.navigateToLogin).toHaveBeenCalled();
    expect(TestBed.inject(Router).url).not.toEqual('/dummy');
    expect(TestBed.inject(Router).url).toEqual('/');
  });
});
