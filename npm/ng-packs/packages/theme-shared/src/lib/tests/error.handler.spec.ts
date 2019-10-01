import { CoreModule, RestOccurError, RouterOutletComponent } from '@abp/ng.core';
import { Location } from '@angular/common';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator';
import { RouterState } from '@ngxs/router-plugin';
import { NgxsModule, Store } from '@ngxs/store';
import { NgxsResetPluginModule, StateOverwrite } from 'ngxs-reset-plugin';
import { DEFAULT_ERROR_MESSAGES, ErrorHandler } from '../handlers';
import { ThemeSharedModule } from '../theme-shared.module';

@Component({ selector: 'abp-dummy', template: 'dummy works! <abp-confirmation></abp-confirmation>' })
class DummyComponent {
  constructor(public errorHandler: ErrorHandler, public store: Store) {}
}

describe('With Custom Host Component', () => {
  let component: SpectatorRouting<DummyComponent>;
  const createComponent = createRoutingFactory({
    component: DummyComponent,
    imports: [CoreModule, ThemeSharedModule.forRoot(), NgxsModule.forRoot([]), NgxsResetPluginModule.forRoot()],
    stubsEnabled: false,
    routes: [{ path: '', component: DummyComponent }, { path: 'account/login', component: RouterOutletComponent }]
  });

  beforeEach(() => {
    component = createComponent();
    const abpError = document.querySelector('abp-error');
    if (abpError) document.body.removeChild(abpError);
  });

  it('should display the error component when server error occurs', () => {
    component.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 500 })));
    component.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError500.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError500.details);
  });

  it('should display the error component when authorize error occurs', () => {
    component.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 403 })));
    component.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError403.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError403.details);
  });

  it('should display the error component when unknown error occurs', () => {
    component.component.store.dispatch(
      new RestOccurError(new HttpErrorResponse({ status: 0, statusText: 'Unknown Error' }))
    );
    component.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.details);
  });

  it('should display the confirmation when not found error occurs', () => {
    component.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 404 })));
    component.detectChanges();
    expect(component.query('.abp-confirm-summary')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError404.title);
    expect(component.query('.abp-confirm-body')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError404.details);
  });

  it('should display the confirmation when default error occurs', () => {
    component.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 412 })));
    component.detectChanges();
    expect(component.query('.abp-confirm-summary')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError.title);
    expect(component.query('.abp-confirm-body')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError.details);
  });

  it('should display the confirmation when authenticated error occurs', async () => {
    component.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));
    component.detectChanges();

    component.component.store.dispatch(new StateOverwrite([RouterState, { state: { url: '/' } }]));
    component.click('#confirm');
    await component.fixture.whenStable();
    expect(component.get(Location).path()).toBe('/account/login');
  });

  it('should display the confirmation when authenticated error occurs with _AbpErrorFormat header', async () => {
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('_AbpErrorFormat', '_AbpErrorFormat');

    component.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401, headers })));
    component.detectChanges();
    component.component.store.dispatch(new StateOverwrite([RouterState, { state: { url: '/' } }]));
    component.click('#confirm');
    await component.fixture.whenStable();
    expect(component.get(Location).path()).toBe('/account/login');
  });

  it('should display the confirmation when error occurs with _AbpErrorFormat header', () => {
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('_AbpErrorFormat', '_AbpErrorFormat');

    component.component.store.dispatch(
      new RestOccurError(
        new HttpErrorResponse({
          error: { error: { message: 'test message', details: 'test detail' } },
          status: 412,
          headers
        })
      )
    );
    component.detectChanges();

    expect(component.query('.abp-confirm-summary')).toHaveText('test message');
    expect(component.query('.abp-confirm-body')).toHaveText('test detail');
  });
});
