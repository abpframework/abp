import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ChartComponent } from './components/chart/chart.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { ErrorComponent } from './components/errors/error.component';
import { ValidationErrorComponent } from './components/errors/validation-error.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { ModalComponent } from './components/modal/modal.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ToastComponent } from './components/toast/toast.component';
import styles from './contants/styles';
import { ErrorHandler } from './handlers/error.handler';
import { chartJsLoaded$ } from './utils/widget-utils';

export function appendScript(injector: Injector) {
  const fn = function() {
    import('chart.js').then(() => chartJsLoaded$.next(true));

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
    NgxValidateCoreModule.forRoot({
      targetSelector: '.form-group',
      blueprints: {
        email: `AbpAccount::ThisFieldIsNotAValidEmailAddress.`,
        max: `AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]`,
        maxlength: `AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ requiredLength }}]`,
        min: `AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]`,
        minlength: `AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf[{{ min }},{{ max }}]`,
        required: `AbpAccount::ThisFieldIsRequired.`,
        passwordMismatch: `AbpIdentity::Identity.PasswordConfirmationFailed`,
      },
      errorTemplate: ValidationErrorComponent,
    }),
  ],
  declarations: [
    BreadcrumbComponent,
    ButtonComponent,
    ChangePasswordComponent,
    ChartComponent,
    ConfirmationComponent,
    ErrorComponent,
    LoaderBarComponent,
    ModalComponent,
    ProfileComponent,
    ToastComponent,
    ValidationErrorComponent,
  ],
  exports: [
    BreadcrumbComponent,
    ButtonComponent,
    ChangePasswordComponent,
    ChartComponent,
    ConfirmationComponent,
    LoaderBarComponent,
    ModalComponent,
    ProfileComponent,
    ToastComponent,
  ],
  entryComponents: [ErrorComponent, ValidationErrorComponent],
})
export class ThemeSharedModule {
  static forRoot(): ModuleWithProviders {
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
