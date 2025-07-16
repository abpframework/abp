import { HttpErrorReporterService } from '@abp/ng.core';
import { CoreTestingModule } from '@abp/ng.core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, NgModule } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { OAuthService } from 'angular-oauth2-oidc';
import { of, Subject } from 'rxjs';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { ErrorHandler } from '../handlers';
import { DEFAULT_ERROR_LOCALIZATIONS, DEFAULT_ERROR_MESSAGES } from '../constants/default-errors';
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
  //entryComponents: [HttpErrorWrapperComponent],
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
    removeIfExistsInDom(selectHtmlErrorWrapper);
  });

  test('should display HttpErrorWrapperComponent when server error occurs', () => {
    const error = new HttpErrorResponse({ status: 500 });

    expect(selectHtmlErrorWrapper()).toBeNull();
    httpErrorReporter.reportError(error);
    expect(selectHtmlErrorWrapper()).not.toBeNull();
  });

  test('should display HttpErrorWrapperComponent when authorize error occurs', () => {
    const error = new HttpErrorResponse({ status: 403 });

    expect(selectHtmlErrorWrapper()).toBeNull();
    httpErrorReporter.reportError(error);
    expect(selectHtmlErrorWrapper()).not.toBeNull();
  });

  test('should display HttpErrorWrapperComponent when unknown error occurs', () => {
    const error = new HttpErrorResponse({ status: 0 });

    httpErrorReporter.reportError(error);
    expect(selectHtmlErrorWrapper()).not.toBeNull();
  });

  test('should call error method of ConfirmationService when not found error occurs', () => {
    httpErrorReporter.reportError(new HttpErrorResponse({ status: 404 }));

    expect(errorConfirmation).toHaveBeenCalledWith(
      {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError404.details,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.details,
      },
      {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError404.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
      },
      CONFIRMATION_BUTTONS,
    );
  });

  test('should call error method of ConfirmationService when default error occurs', () => {
    httpErrorReporter.reportError(new HttpErrorResponse({ status: 412 }));

    expect(errorConfirmation).toHaveBeenCalledWith(
      {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.details,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.details,
      },
      {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
      },
      CONFIRMATION_BUTTONS,
    );
  });

  test('should call error method of ConfirmationService when authenticated error occurs', () => {
    httpErrorReporter.reportError(new HttpErrorResponse({ status: 401 }));

    expect(errorConfirmation).toHaveBeenCalledWith(
      {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError401.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
      },
      {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError401.details,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.details,
      },
      CONFIRMATION_BUTTONS,
    );
  });

  test('should call error method of ConfirmationService when authenticated error occurs with _AbpErrorFormat header', () => {
    const headers: HttpHeaders = new HttpHeaders({
      _AbpErrorFormat: '_AbpErrorFormat',
    });
    httpErrorReporter.reportError(new HttpErrorResponse({ status: 401, headers }));

    expect(errorConfirmation).toHaveBeenCalledWith(
      {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
      },
      '',
      CONFIRMATION_BUTTONS,
    );
  });

  test('should call error method of ConfirmationService when error occurs with _AbpErrorFormat header', () => {
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('_AbpErrorFormat', '_AbpErrorFormat');
    httpErrorReporter.reportError(
      new HttpErrorResponse({
        error: { error: { message: 'test message', details: 'test detail' } },
        status: 412,
        headers,
      }),
    );

    expect(errorConfirmation).toHaveBeenCalledWith(
      'test detail',
      'test message',
      CONFIRMATION_BUTTONS,
    );
  });

  test('should delegate to CUSTOM_ERROR_HANDLERS and call execute if canHandle is true', () => {
    const error = new HttpErrorResponse({ status: 418 });

    httpErrorReporter.reportError(error);

    expect(customHandlerMock.canHandle).toHaveBeenCalledWith(error);
    expect(customHandlerMock.execute).toHaveBeenCalled();
  });
});

@Component({
  selector: 'abp-dummy-error',
  template: '<p>{{errorStatus}}</p>',
})
class DummyErrorComponent {
  errorStatus;
  destroy$;
}

@NgModule({
  declarations: [],
  exports: [DummyErrorComponent],
  imports: [DummyErrorComponent],
})
class ErrorModule {}

// TODO: error component does not place to the DOM.
// describe('ErrorHandler with custom error component', () => {
//   const createService = createServiceFactory({
//     service: ErrorHandler,
//     imports: [
//       RouterModule.forRoot([], { relativeLinkResolution: 'legacy' }),
//       NgxsModule.forRoot([]),
//       CoreModule,
//       MockModule,
//       ErrorModule,
//     ],
//     mocks: [OAuthService, ConfirmationService],
//     providers: [
//       { provide: APP_BASE_HREF, useValue: '/' },
//       {
//         provide: 'HTTP_ERROR_CONFIG',
//         useFactory: customHttpErrorConfigFactory,
//       },
//     ],
//   });

//   beforeEach(() => {
//     spectator = createService();
//     service = spectator.service;
//     store = spectator.inject(Store);
//     store.selectSnapshot = jest.fn(() => '/x');
//   });

//   afterEach(() => {
//     removeIfExistsInDom(selectCustomError);
//   });

//   describe('Custom error component', () => {
//     test('should be created when 401 error is dispatched', () => {
//       store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));

//       expect(selectCustomErrorText()).toBe('401');
//     });

//     test('should be created when 403 error is dispatched', () => {
//       store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 403 })));

//       expect(selectCustomErrorText()).toBe('403');
//     });

//     test('should be created when 404 error is dispatched', () => {
//       store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 404 })));

//       expect(selectCustomErrorText()).toBe('404');
//     });

//     test('should be created when RouterError is dispatched', () => {
//       store.dispatch(new RouterError(null, null, new NavigationError(1, 'test', 'Cannot match')));

//       expect(selectCustomErrorText()).toBe('404');
//     });

//     test('should be created when 500 error is dispatched', () => {
//       store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 500 })));

//       expect(selectCustomErrorText()).toBe('500');
//     });

//     test('should call destroy method of componentRef when destroy$ emits', () => {
//       store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));

//       expect(selectCustomErrorText()).toBe('401');

//       const destroyComponent = jest.spyOn(service.componentRef, 'destroy');

//       service.componentRef.instance.destroy$.next();

//       expect(destroyComponent).toHaveBeenCalledTimes(1);
//     });
//   });
// });

function removeIfExistsInDom(errorSelector: () => HTMLDivElement | null) {
  const abpError = errorSelector();
  if (abpError) abpError.parentNode.removeChild(abpError);
}

function selectHtmlErrorWrapper(): HTMLDivElement | null {
  return document.querySelector('abp-http-error-wrapper');
}

function selectCustomError(): HTMLDivElement | null {
  return document.querySelector('abp-dummy-error');
}

function selectCustomErrorText(): string {
  return selectCustomError().querySelector('p').textContent;
}
