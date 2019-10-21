import { CoreModule, RestOccurError, RouterOutletComponent } from '@abp/ng.core';
import { Location } from '@angular/common';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { DEFAULT_ERROR_MESSAGES, ErrorHandler } from '../handlers';
import { ThemeSharedModule } from '../theme-shared.module';

@Component({ selector: 'abp-dummy', template: 'dummy works! <abp-confirmation></abp-confirmation>' })
class DummyComponent {
  constructor(public errorHandler: ErrorHandler, public store: Store) {}
}

describe('ErrorHandler', () => {
  let spectator: SpectatorRouting<DummyComponent>;
  let store: Store;

  const createComponent = createRoutingFactory({
    component: DummyComponent,
    imports: [CoreModule, ThemeSharedModule.forRoot(), NgxsModule.forRoot([])],
    stubsEnabled: false,
    routes: [{ path: '', component: DummyComponent }, { path: 'account/login', component: RouterOutletComponent }],
  });

  beforeEach(() => {
    spectator = createComponent();
    store = spectator.component.store;

    const abpError = document.querySelector('abp-error');
    if (abpError) document.body.removeChild(abpError);
  });

  it('should display the error component when server error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 500 })));
    spectator.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError500.title);
    expect(document.querySelector('.error-details')).toHaveText(
      DEFAULT_ERROR_MESSAGES.defaultError500.details.defaultValue,
    );
  });

  it('should display the error component when authorize error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 403 })));
    spectator.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError403.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError403.details);
  });

  it('should display the error component when unknown error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 0, statusText: 'Unknown Error' })));
    spectator.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.title);
    expect(document.querySelector('.error-details')).toHaveText(
      DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.details.defaultValue,
    );
  });

  it('should display the confirmation when not found error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 404 })));
    spectator.detectChanges();
    expect(spectator.query('.abp-confirm-summary')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError404.title);
    expect(spectator.query('.abp-confirm-body')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError404.details);
  });

  it('should display the confirmation when default error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 412 })));
    spectator.detectChanges();
    expect(spectator.query('.abp-confirm-summary')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError.title);
    expect(spectator.query('.abp-confirm-body')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError.details);
  });

  it('should display the confirmation when authenticated error occurs', async () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));
    spectator.detectChanges();

    spectator.click('#confirm');
    await spectator.fixture.whenStable();
    expect(spectator.get(Location).path()).toBe('/account/login');
  });

  it('should display the confirmation when authenticated error occurs with _AbpErrorFormat header', async () => {
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('_AbpErrorFormat', '_AbpErrorFormat');

    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401, headers })));
    spectator.detectChanges();

    spectator.click('#confirm');
    await spectator.fixture.whenStable();
    expect(spectator.get(Location).path()).toBe('/account/login');
  });

  it('should display the confirmation when error occurs with _AbpErrorFormat header', () => {
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
    spectator.detectChanges();

    expect(spectator.query('.abp-confirm-summary')).toHaveText('test message');
    expect(spectator.query('.abp-confirm-body')).toHaveText('test detail');
  });
});
