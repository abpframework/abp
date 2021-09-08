import { RestOccurError } from '@abp/ng.core';
import { CoreTestingModule } from '@abp/ng.core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, NgModule } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { of } from 'rxjs';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { DEFAULT_ERROR_LOCALIZATIONS, DEFAULT_ERROR_MESSAGES, ErrorHandler } from '../handlers';
import { ConfirmationService } from '../services';
import { httpErrorConfigFactory } from '../tokens/http-error.token';

@NgModule({
  exports: [HttpErrorWrapperComponent],
  declarations: [HttpErrorWrapperComponent],
  entryComponents: [HttpErrorWrapperComponent],
  imports: [CoreTestingModule],
})
class MockModule {}

let spectator: SpectatorService<ErrorHandler>;
let service: ErrorHandler;
let store: Store;
const errorConfirmation: jest.Mock = jest.fn(() => of(null));
const CONFIRMATION_BUTTONS = {
  hideCancelBtn: true,
  yesText: 'AbpAccount::Close',
};
describe('ErrorHandler', () => {
  const createService = createServiceFactory({
    service: ErrorHandler,
    imports: [NgxsModule.forRoot([]), CoreTestingModule.withConfig(), MockModule],
    mocks: [OAuthService],
    providers: [
      { provide: APP_BASE_HREF, useValue: '/' },
      {
        provide: 'HTTP_ERROR_CONFIG',
        useFactory: httpErrorConfigFactory,
      },
      {
        provide: ConfirmationService,
        useValue: {
          error: errorConfirmation,
        },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.inject(Store);
    store.selectSnapshot = jest.fn(() => '/x');
  });

  afterEach(() => {
    errorConfirmation.mockClear();
    removeIfExistsInDom(selectHtmlErrorWrapper);
  });

  test('should display HttpErrorWrapperComponent when server error occurs', () => {
    const createComponent = jest.spyOn(service, 'createErrorComponent');
    const error = new HttpErrorResponse({ status: 500 });
    const params = {
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError500.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.title,
      },
      details: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError500.details,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.details,
      },
      status: 500,
    };

    expect(selectHtmlErrorWrapper()).toBeNull();

    store.dispatch(new RestOccurError(error));

    expect(createComponent).toHaveBeenCalledWith(params);
  });

  test('should display HttpErrorWrapperComponent when authorize error occurs', () => {
    const createComponent = jest.spyOn(service, 'createErrorComponent');
    const error = new HttpErrorResponse({ status: 403 });
    const params = {
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError403.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.title,
      },
      details: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError403.details,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.details,
      },
      status: 403,
    };

    expect(selectHtmlErrorWrapper()).toBeNull();

    store.dispatch(new RestOccurError(error));

    expect(createComponent).toHaveBeenCalledWith(params);
  });

  test('should display HttpErrorWrapperComponent when unknown error occurs', () => {
    const createComponent = jest.spyOn(service, 'createErrorComponent');
    const error = new HttpErrorResponse({
      status: 0,
      statusText: 'Unknown Error',
    });
    const params = {
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
      },
      details: error.message,
      isHomeShow: false,
    };

    expect(selectHtmlErrorWrapper()).toBeNull();

    store.dispatch(new RestOccurError(error));

    expect(createComponent).toHaveBeenCalledWith(params);
  });

  test('should call error method of ConfirmationService when not found error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 404 })));

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
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 412 })));

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
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));

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
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401, headers })));

    expect(errorConfirmation).toHaveBeenCalledWith(
      {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
      },
      null,
      CONFIRMATION_BUTTONS,
    );
  });

  test('should call error method of ConfirmationService when error occurs with _AbpErrorFormat header', () => {
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('_AbpErrorFormat', '_AbpErrorFormat');
    store.dispatch(
      new RestOccurError(
        new HttpErrorResponse({
          error: { error: { message: 'test message', details: 'test detail' } },
          status: 412,
          headers,
        }),
      ),
    );

    expect(errorConfirmation).toHaveBeenCalledWith(
      'test detail',
      'test message',
      CONFIRMATION_BUTTONS,
    );
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
  declarations: [DummyErrorComponent],
  exports: [DummyErrorComponent],
  entryComponents: [DummyErrorComponent],
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

export function customHttpErrorConfigFactory() {
  return httpErrorConfigFactory({
    errorScreen: {
      component: DummyErrorComponent,
      forWhichErrors: [401, 403, 404, 500],
    },
  });
}

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
