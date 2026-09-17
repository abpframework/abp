import { ModuleWithProviders, NgModule } from '@angular/core';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import {
  BreadcrumbItemsComponent,
  BreadcrumbComponent,
  ButtonComponent,
  ConfirmationComponent,
  HttpErrorWrapperComponent,
  LoaderBarComponent,
  LoadingComponent,
  ModalComponent,
  ToastContainerComponent,
  ToastComponent,
  ModalCloseDirective,
  PasswordComponent,
  CardModule,
  FormInputComponent,
  FormCheckboxComponent,
} from './components';
import {
  LoadingDirective,
  NgxDatatableDefaultDirective,
  NgxDatatableListDirective,
  AbpVisibleDirective,
  DisabledDirective,
} from './directives';
import { RootParams } from './models';
import {
  provideAbpThemeShared,
  withConfirmationIcon,
  withHttpErrorConfig,
  withValidateOnSubmit,
  withValidationBluePrint,
  withValidationMapErrorsFn,
} from './providers';

export const THEME_SHARED_EXPORTS = [
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
  HttpErrorWrapperComponent,
  NgxDatatableModule,
  NgxValidateCoreModule,
  CardModule,
  DisabledDirective,
  AbpVisibleDirective,
  NgxDatatableListDirective,
  NgxDatatableDefaultDirective,
  PasswordComponent,
];

@NgModule({
  imports: [...THEME_SHARED_EXPORTS],
  declarations: [],
  exports: [...THEME_SHARED_EXPORTS],
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
