import { CoreModule, RestOccurError } from '@abp/ng.core';
import { APP_BASE_HREF } from '@angular/common';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, NgModule } from '@angular/core';
import { NavigationError, ResolveEnd, RouterModule } from '@angular/router';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { Navigate, RouterDataResolved, RouterError } from '@ngxs/router-plugin';
import { Actions, NgxsModule, ofActionDispatched, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { of } from 'rxjs';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { DEFAULT_ERROR_MESSAGES, ErrorHandler } from '../handlers';
import { ConfirmationService } from '../services';
import { httpErrorConfigFactory } from '../tokens/http-error.token';

@NgModule({
  exports: [HttpErrorWrapperComponent],
  declarations: [HttpErrorWrapperComponent],
  entryComponents: [HttpErrorWrapperComponent],
  imports: [CoreModule],
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
    imports: [RouterModule.forRoot([]), NgxsModule.forRoot([]), CoreModule, MockModule],
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
    store = spectator.get(Store);
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
        key: 'AbpAccount::500Message',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.title,
      },
      details: {
        key: 'AbpAccount::InternalServerErrorMessage',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.details,
      },
      status: 500,
    };

    expect(selectHtmlErrorWrapper()).toBeNull();

    store.dispatch(new RestOccurError(error));

    expect(createComponent).toHaveBeenCalledWith(params);

    const wrapper = service.componentRef.instance;
    expect(wrapper.title).toEqual(params.title);
    expect(wrapper.details).toEqual(params.details);
    expect(wrapper.status).toBe(params.status);

    expect(selectHtmlErrorWrapper()).not.toBeNull();
  });

  test('should display HttpErrorWrapperComponent when authorize error occurs', () => {
    const createComponent = jest.spyOn(service, 'createErrorComponent');
    const error = new HttpErrorResponse({ status: 403 });
    const params = {
      title: {
        key: 'AbpAccount::DefaultErrorMessage403',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.title,
      },
      details: {
        key: 'AbpAccount::DefaultErrorMessage403Detail',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.details,
      },
      status: 403,
    };

    expect(selectHtmlErrorWrapper()).toBeNull();

    store.dispatch(new RestOccurError(error));

    expect(createComponent).toHaveBeenCalledWith(params);

    const wrapper = service.componentRef.instance;
    expect(wrapper.title).toEqual(params.title);
    expect(wrapper.details).toEqual(params.details);
    expect(wrapper.status).toBe(params.status);

    expect(selectHtmlErrorWrapper()).not.toBeNull();
  });

  test('should display HttpErrorWrapperComponent when unknown error occurs', () => {
    const createComponent = jest.spyOn(service, 'createErrorComponent');
    const error = new HttpErrorResponse({ status: 0, statusText: 'Unknown Error' });
    const params = {
      title: {
        key: 'AbpAccount::DefaultErrorMessage',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
      },
      details: error.message,
      isHomeShow: false,
    };

    expect(selectHtmlErrorWrapper()).toBeNull();

    store.dispatch(new RestOccurError(error));

    expect(createComponent).toHaveBeenCalledWith(params);

    const wrapper = service.componentRef.instance;
    expect(wrapper.title).toEqual(params.title);
    expect(wrapper.details).toEqual(params.details);
    expect(wrapper.isHomeShow).toBe(params.isHomeShow);

    expect(selectHtmlErrorWrapper()).not.toBeNull();
  });

  test('should call error method of ConfirmationService when not found error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 404 })));

    expect(errorConfirmation).toHaveBeenCalledWith(
      {
        key: 'AbpAccount::DefaultErrorMessage404',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.details,
      },
      {
        key: 'AbpAccount::DefaultErrorMessage404Detail',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
      },
      CONFIRMATION_BUTTONS,
    );
  });

  test('should call error method of ConfirmationService when default error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 412 })));

    expect(errorConfirmation).toHaveBeenCalledWith(
      DEFAULT_ERROR_MESSAGES.defaultError.details,
      DEFAULT_ERROR_MESSAGES.defaultError.title,
      CONFIRMATION_BUTTONS,
    );
  });

  test('should call error method of ConfirmationService when authenticated error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));

    expect(errorConfirmation).toHaveBeenCalledWith(
      {
        key: 'AbpAccount::DefaultErrorMessage401',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
      },
      {
        key: 'AbpAccount::DefaultErrorMessage401Detail',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.details,
      },
      CONFIRMATION_BUTTONS,
    );
  });

  test('should call error method of ConfirmationService when authenticated error occurs with _AbpErrorFormat header', done => {
    spectator
      .get(Actions)
      .pipe(ofActionDispatched(Navigate))
      .subscribe(({ path, queryParams, extras }) => {
        expect(path).toEqual(['/account/login']);
        expect(queryParams).toBeNull();
        expect(extras).toEqual({ state: { redirectUrl: '/x' } });

        done();
      });

    const headers: HttpHeaders = new HttpHeaders({
      _AbpErrorFormat: '_AbpErrorFormat',
    });
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401, headers })));

    expect(errorConfirmation).toHaveBeenCalledWith(
      DEFAULT_ERROR_MESSAGES.defaultError.title,
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

  test('should call destroy method of componentRef when ResolveEnd is dispatched', () => {
    store.dispatch(new RouterError(null, null, new NavigationError(1, 'test', 'Cannot match')));

    const destroyComponent = jest.spyOn(service.componentRef, 'destroy');

    store.dispatch(new RouterDataResolved(null, new ResolveEnd(1, 'test', 'test', null)));

    expect(destroyComponent).toHaveBeenCalledTimes(1);
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

describe('ErrorHandler with custom error component', () => {
  const createService = createServiceFactory({
    service: ErrorHandler,
    imports: [
      RouterModule.forRoot([]),
      NgxsModule.forRoot([]),
      CoreModule,
      MockModule,
      ErrorModule,
    ],
    mocks: [OAuthService, ConfirmationService],
    providers: [
      { provide: APP_BASE_HREF, useValue: '/' },
      {
        provide: 'HTTP_ERROR_CONFIG',
        useFactory: customHttpErrorConfigFactory,
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.get(Store);
    store.selectSnapshot = jest.fn(() => '/x');
  });

  afterEach(() => {
    removeIfExistsInDom(selectCustomError);
  });

  describe('Custom error component', () => {
    test('should be created when 401 error is dispatched', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));

      expect(selectCustomErrorText()).toBe('401');
    });

    test('should be created when 403 error is dispatched', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 403 })));

      expect(selectCustomErrorText()).toBe('403');
    });

    test('should be created when 404 error is dispatched', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 404 })));

      expect(selectCustomErrorText()).toBe('404');
    });

    test('should be created when RouterError is dispatched', () => {
      store.dispatch(new RouterError(null, null, new NavigationError(1, 'test', 'Cannot match')));

      expect(selectCustomErrorText()).toBe('404');
    });

    test('should be created when 500 error is dispatched', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 500 })));

      expect(selectCustomErrorText()).toBe('500');
    });

    test('should call destroy method of componentRef when destroy$ emits', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));

      expect(selectCustomErrorText()).toBe('401');

      const destroyComponent = jest.spyOn(service.componentRef, 'destroy');

      service.componentRef.instance.destroy$.next();

      expect(destroyComponent).toHaveBeenCalledTimes(1);
    });
  });
});

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
