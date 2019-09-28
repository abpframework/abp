import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { Component } from '@angular/core';
import { ErrorHandler, DEFAULT_ERROR_MESSAGES } from '../handlers';
import { CoreModule, RestOccurError } from '@abp/ng.core';
import { ThemeSharedModule } from '../theme-shared.module';
import { NgxsModule, Store } from '@ngxs/store';
import { RouterModule } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({ selector: 'dummy', template: 'dummy works! <abp-confirmation></abp-confirmation>' })
class DummyComponent {
  constructor(public errorHandler: ErrorHandler, public store: Store) {}
}

describe('With Custom Host Component', function() {
  let host: SpectatorHost<DummyComponent>;
  const createHost = createHostFactory({
    component: DummyComponent,
    imports: [CoreModule, ThemeSharedModule.forRoot(), NgxsModule.forRoot([]), RouterModule.forRoot([])],
  });

  beforeEach(() => {
    host = createHost(`<dummy></dummy>`);
    const abpError = document.querySelector('abp-error');
    if (abpError) document.body.removeChild(abpError);
  });

  it('should display the error component when server error occurs', () => {
    host.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 500 })));
    host.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError500.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError500.details);
  });

  it('should display the error component when authorize error occurs', () => {
    host.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 403 })));
    host.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError403.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError403.details);
  });

  it('should display the error component when unknown error occurs', () => {
    host.component.store.dispatch(
      new RestOccurError(new HttpErrorResponse({ status: 0, statusText: 'Unknown Error' })),
    );
    host.detectChanges();
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.details);
  });

  it('should display the confirmation when not found error occurs', () => {
    host.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 404 })));
    host.detectChanges();
    expect(host.query('.abp-confirm-summary')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError404.title);
    expect(host.query('.abp-confirm-body')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError404.details);
  });

  it('should display the confirmation when default error occurs', () => {
    host.component.store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 412 })));
    host.detectChanges();
    expect(host.query('.abp-confirm-summary')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError.title);
    expect(host.query('.abp-confirm-body')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError.details);
  });
});
