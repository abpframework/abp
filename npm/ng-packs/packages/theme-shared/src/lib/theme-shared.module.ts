import { CoreModule } from '@abp/ng.core';
import { DatePipe } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { BreadcrumbItemsComponent } from './components/breadcrumb-items/breadcrumb-items.component';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { HttpErrorWrapperComponent } from './components/http-error-wrapper/http-error-wrapper.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { LoadingComponent } from './components/loading/loading.component';
import { ModalCloseDirective } from './components/modal/modal-close.directive';
import { ModalComponent } from './components/modal/modal.component';
import { ToastContainerComponent } from './components/toast-container/toast-container.component';
import { ToastComponent } from './components/toast/toast.component';
import { EllipsisDirective } from './directives/ellipsis.directive';
import { LoadingDirective } from './directives/loading.directive';
import { NgxDatatableDefaultDirective } from './directives/ngx-datatable-default.directive';
import { NgxDatatableListDirective } from './directives/ngx-datatable-list.directive';
import { RootParams } from './models/common';
import {
  provideAbpThemeShared,
  withConfirmationIcon,
  withHttpErrorConfig,
  withValidateOnSubmit,
  withValidationBluePrint,
  withValidationMapErrorsFn,
} from './providers';
import { PasswordComponent } from './components/password/password.component';
import { CardModule } from './components/card/card.module';
import { AbpVisibleDirective, DisabledDirective } from './directives';
import { FormInputComponent } from './components/form-input/form-input.component';
import { FormCheckboxComponent } from './components/checkbox/checkbox.component';

const declarationsWithExports = [
  BreadcrumbComponent,
  BreadcrumbItemsComponent,
  ButtonComponent,
  ConfirmationComponent,
  LoaderBarComponent,
  LoadingComponent,
  ModalComponent,
  ToastComponent,
  ToastContainerComponent,
  LoadingDirective,
  ModalCloseDirective,
  FormInputComponent,
  FormCheckboxComponent,
];

@NgModule({
  imports: [
    CoreModule,
    NgxDatatableModule,
    NgxValidateCoreModule,
    NgbPaginationModule,
    EllipsisDirective,
    CardModule,
    PasswordComponent,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
    DisabledDirective,
    AbpVisibleDirective,
  ],
  declarations: [...declarationsWithExports, HttpErrorWrapperComponent],
  exports: [
    NgxDatatableModule,
    NgxValidateCoreModule,
    CardModule,
    DisabledDirective,
    AbpVisibleDirective,
    NgxDatatableListDirective,
    NgxDatatableDefaultDirective,
    PasswordComponent,
    ...declarationsWithExports,
  ],
  providers: [DatePipe],
})
export class BaseThemeSharedModule {}

@NgModule({
  imports: [BaseThemeSharedModule],
  exports: [BaseThemeSharedModule],
})
export class ThemeSharedModule {
  /**
   * @deprecated forRoot method is deprecated, use `provideAbpThemeShared` *function* for config settings.
   */
  static forRoot(
    { httpErrorConfig, validation = {}, confirmationIcons = {} } = {} as RootParams,
  ): ModuleWithProviders<ThemeSharedModule> {
    return {
      ngModule: ThemeSharedModule,
      providers: [
        provideAbpThemeShared(
          withHttpErrorConfig(httpErrorConfig),
          withValidationBluePrint(validation.blueprints),
          withValidationMapErrorsFn(validation.mapErrorsFn),
          withValidateOnSubmit(validation.validateOnSubmit),
          withConfirmationIcon(confirmationIcons),
        ),
      ],
    };
  }
}
