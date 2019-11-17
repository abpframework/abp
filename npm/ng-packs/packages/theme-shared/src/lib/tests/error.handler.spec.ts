import { CoreModule, RestOccurError, RouterOutletComponent } from '@abp/ng.core';
import { Location } from '@angular/common';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, NgModule } from '@angular/core';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { DEFAULT_ERROR_MESSAGES, ErrorHandler } from '../handlers';
import { ThemeSharedModule } from '../theme-shared.module';
import { MessageService } from 'primeng/components/common/messageservice';
import { RouterError, RouterDataResolved } from '@ngxs/router-plugin';
import { NavigationError, ResolveEnd } from '@angular/router';

@Component({ selector: 'abp-dummy', template: 'dummy works! <abp-confirmation></abp-confirmation>' })
class DummyComponent {
  constructor(public errorHandler: ErrorHandler) {}
}

let spectator: SpectatorRouting<DummyComponent>;
let store: Store;
describe('ErrorHandler', () => {
  const createComponent = createRoutingFactory({
    component: DummyComponent,
    imports: [CoreModule, ThemeSharedModule.forRoot(), NgxsModule.forRoot([])],
    stubsEnabled: false,
    routes: [{ path: '', component: DummyComponent }, { path: 'account/login', component: RouterOutletComponent }],
  });

  beforeEach(() => {
    spectator = createComponent();
    store = spectator.get(Store);

    const abpError = document.querySelector('abp-error');
    if (abpError) document.body.removeChild(abpError);
  });

  it('should display the error component when server error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 500 })));
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError500.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError500.details);
  });

  it('should display the error component when authorize error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 403 })));
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError403.title);
    expect(document.querySelector('.error-details')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError403.details);
  });

  it('should display the error component when unknown error occurs', () => {
    store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 0, statusText: 'Unknown Error' })));
    expect(document.querySelector('.error-template')).toHaveText(DEFAULT_ERROR_MESSAGES.defaultError.title);
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

@Component({
  selector: 'abp-dummy-error',
  template: '<p>{{errorStatus}}</p><button id="close-dummy" (click)="destroy$.next()">Close</button>',
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
  const createComponent = createRoutingFactory({
    component: DummyComponent,
    imports: [
      CoreModule,
      ThemeSharedModule.forRoot({
        httpErrorConfig: { errorScreen: { component: DummyErrorComponent, forWhichErrors: [401, 403, 404, 500] } },
      }),
      NgxsModule.forRoot([]),
      ErrorModule,
    ],
    stubsEnabled: false,
    routes: [{ path: '', component: DummyComponent }, { path: 'account/login', component: RouterOutletComponent }],
  });

  beforeEach(() => {
    spectator = createComponent();
    store = spectator.get(Store);

    const abpError = document.querySelector('abp-error');
    if (abpError) document.body.removeChild(abpError);
  });

  describe('Custom error component', () => {
    it('should create when occur 401', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 401 })));
      expect(document.querySelector('abp-dummy-error')).toBeTruthy();
      expect(document.querySelector('p')).toHaveExactText('401');
    });

    it('should create when occur 403', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 403 })));
      expect(document.querySelector('p')).toHaveExactText('403');
    });

    it('should create when occur 404', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 404 })));
      expect(document.querySelector('p')).toHaveExactText('404');
    });

    it('should create when dispatched the RouterError', () => {
      store.dispatch(new RouterError(null, null, new NavigationError(1, 'test', 'Cannot match')));
      expect(document.querySelector('p')).toHaveExactText('404');
      store.dispatch(new RouterDataResolved(null, new ResolveEnd(1, 'test', 'test', null)));
    });

    it('should create when occur 500', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 500 })));
      expect(document.querySelector('p')).toHaveExactText('500');
    });

    it('should be destroyed when click the close button', () => {
      store.dispatch(new RestOccurError(new HttpErrorResponse({ status: 500 })));
      document.querySelector<HTMLButtonElement>('#close-dummy').click();
      spectator.detectChanges();
      expect(document.querySelector('abp-dummy-error')).toBeFalsy();
    });
  });
});
