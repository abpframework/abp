import { HttpErrorReporterService } from '@abp/ng.core';
import { CoreTestingModule } from '@abp/ng.core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { OAuthService } from 'angular-oauth2-oidc';
import { of, Subject } from 'rxjs';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { ErrorHandler } from '../handlers';
import { ConfirmationService } from '../services';
import { CUSTOM_ERROR_HANDLERS, HTTP_ERROR_CONFIG } from '../tokens/http-error.token';
import { CustomHttpErrorHandlerService } from '../models';

const customHandlerMock: CustomHttpErrorHandlerService = {
  priority: 100,
  canHandle: jest.fn().mockReturnValue(true),
  execute: jest.fn(),
};

const reporter$ = new Subject();

@NgModule({
  exports: [HttpErrorWrapperComponent],
  declarations: [],
  imports: [CoreTestingModule, HttpErrorWrapperComponent],
})
class MockModule {}

let spectator: SpectatorService<ErrorHandler>;
let service: ErrorHandler;
let httpErrorReporter: HttpErrorReporterService;
const errorConfirmation: jest.Mock = jest.fn(() => of(null));
const CONFIRMATION_BUTTONS = {
  hideCancelBtn: true,
  yesText: 'AbpAccount::Close',
};

describe('ErrorHandler', () => {
  const createService = createServiceFactory({
    service: ErrorHandler,
    imports: [CoreTestingModule.withConfig(), MockModule],
    mocks: [OAuthService],
    providers: [
      {
        provide: HttpErrorReporterService,
        useValue: {
          reportError: err => {
            reporter$.next(err);
          },
          reporter$: reporter$.asObservable(),
        },
      },
      { provide: APP_BASE_HREF, useValue: '/' },
      {
        provide: ConfirmationService,
        useValue: {
          error: errorConfirmation,
        },
      },
      {
        provide: CUSTOM_ERROR_HANDLERS,
        useValue: customHandlerMock,
        multi: true,
      },
      {
        provide: HTTP_ERROR_CONFIG,
        useFactory: () => ({}),
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    httpErrorReporter = spectator.inject(HttpErrorReporterService);
  });

  afterEach(() => {
    errorConfirmation.mockClear();
  });

  test('should create service', () => {
    expect(service).toBeTruthy();
  });

  test('should handle server error', () => {
    const error = new HttpErrorResponse({ status: 500 });
    httpErrorReporter.reportError(error);
    expect(service).toBeTruthy();
  });

  test('should handle authorize error', () => {
    const error = new HttpErrorResponse({ status: 403 });
    httpErrorReporter.reportError(error);
    expect(service).toBeTruthy();
  });

  test('should handle unknown error', () => {
    const error = new HttpErrorResponse({ status: 999 });
    httpErrorReporter.reportError(error);
    expect(service).toBeTruthy();
  });

  test('should handle not found error', () => {
    const error = new HttpErrorResponse({ status: 404 });
    httpErrorReporter.reportError(error);
    expect(service).toBeTruthy();
  });

  test('should handle default error', () => {
    const error = new HttpErrorResponse({ status: 412 });
    httpErrorReporter.reportError(error);
    expect(service).toBeTruthy();
  });

  test('should handle authenticated error', () => {
    const error = new HttpErrorResponse({ status: 401 });
    httpErrorReporter.reportError(error);
    expect(service).toBeTruthy();
  });

  test('should handle authenticated error with _AbpErrorFormat header', () => {
    const headers = new HttpHeaders().set('_AbpErrorFormat', 'true');
    const error = new HttpErrorResponse({ status: 401, headers });
    httpErrorReporter.reportError(error);
    expect(service).toBeTruthy();
  });

  test('should handle error with _AbpErrorFormat header', () => {
    const headers = new HttpHeaders().set('_AbpErrorFormat', 'true');
    const error = new HttpErrorResponse({
      status: 400,
      headers,
      error: {
        error: {
          message: 'test message',
          details: 'test detail',
        },
      },
    });
    httpErrorReporter.reportError(error);
    expect(service).toBeTruthy();
  });
});
