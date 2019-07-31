import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { ModalComponent } from './components/modal/modal.component';
import { ToastComponent } from './components/toast/toast.component';
import styles from './contants/styles';
import { ErrorHandler } from './handlers/error.handler';
import { Error500Component } from './components/errors/error-500.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { Options } from './models/options';

export function appendScript(injector: Injector) {
  const fn = function() {
    const lazyLoadService: LazyLoadService = injector.get(LazyLoadService);

    return forkJoin(
      lazyLoadService.load(
        null,
        'style',
        styles,
        'head',
        'afterbegin',
      ) /* lazyLoadService.load(null, 'script', scripts) */,
    ).pipe(take(1));
  };

  return fn;
}

@NgModule({
  imports: [
    CoreModule,
    ToastModule,
    NgbModalModule,
    NgxValidateCoreModule.forRoot({
      targetSelector: '.form-group',
    }),
  ],
  declarations: [ConfirmationComponent, ToastComponent, ModalComponent, Error500Component, LoaderBarComponent],
  exports: [NgbModalModule, ConfirmationComponent, ToastComponent, ModalComponent, LoaderBarComponent],
  entryComponents: [Error500Component],
})
export class ThemeSharedModule {
  static forRoot(options = {} as Options): ModuleWithProviders {
    return {
      ngModule: ThemeSharedModule,
      providers: [
        {
          provide: APP_INITIALIZER,
          multi: true,
          deps: [Injector, ErrorHandler],
          useFactory: appendScript,
        },
        { provide: MessageService, useClass: MessageService },
      ],
    };
  }
}
