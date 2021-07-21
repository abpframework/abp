import { CoreModule, noop } from '@abp/ng.core';
import { DatePipe } from '@angular/common';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { NgbDateParserFormatter, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import {
  defaultMapErrorsFn,
  NgxValidateCoreModule,
  VALIDATION_BLUEPRINTS,
  VALIDATION_MAP_ERRORS_FN,
  VALIDATION_VALIDATE_ON_SUBMIT,
} from '@ngx-validate/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ChartComponent } from './components/chart/chart.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { HttpErrorWrapperComponent } from './components/http-error-wrapper/http-error-wrapper.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { LoadingComponent } from './components/loading/loading.component';
import { ModalCloseDirective } from './components/modal/modal-close.directive';
import { ModalContainerComponent } from './components/modal/modal-container.component';
import { ModalComponent } from './components/modal/modal.component';
import { SortOrderIconComponent } from './components/sort-order-icon/sort-order-icon.component';
import { TableEmptyMessageComponent } from './components/table-empty-message/table-empty-message.component';
import { TableComponent } from './components/table/table.component';
import { ToastContainerComponent } from './components/toast-container/toast-container.component';
import { ToastComponent } from './components/toast/toast.component';
import { DEFAULT_VALIDATION_BLUEPRINTS } from './constants/validation';
import { EllipsisModule } from './directives/ellipsis.directive';
import { LoadingDirective } from './directives/loading.directive';
import { NgxDatatableDefaultDirective } from './directives/ngx-datatable-default.directive';
import { NgxDatatableListDirective } from './directives/ngx-datatable-list.directive';
import { TableSortDirective } from './directives/table-sort.directive';
import { ErrorHandler } from './handlers/error.handler';
import { initLazyStyleHandler } from './handlers/lazy-style.handler';
import { RootParams } from './models/common';
import { NG_BOOTSTRAP_CONFIG_PROVIDERS } from './providers';
import { THEME_SHARED_ROUTE_PROVIDERS } from './providers/route.provider';
import { THEME_SHARED_APPEND_CONTENT } from './tokens/append-content.token';
import { httpErrorConfigFactory, HTTP_ERROR_CONFIG } from './tokens/http-error.token';
import { DateParserFormatter } from './utils/date-parser-formatter';

const declarationsWithExports = [
  BreadcrumbComponent,
  ButtonComponent,
  ChartComponent,
  ConfirmationComponent,
  LoaderBarComponent,
  LoadingComponent,
  ModalComponent,
  TableComponent,
  TableEmptyMessageComponent,
  ToastComponent,
  ToastContainerComponent,
  SortOrderIconComponent,
  NgxDatatableDefaultDirective,
  NgxDatatableListDirective,
  LoadingDirective,
  TableSortDirective,
  ModalCloseDirective,
];

@NgModule({
  imports: [
    CoreModule,
    NgxDatatableModule,
    NgxValidateCoreModule,
    NgbPaginationModule,
    EllipsisModule,
  ],
  declarations: [...declarationsWithExports, HttpErrorWrapperComponent, ModalContainerComponent],
  exports: [NgxDatatableModule, EllipsisModule, ...declarationsWithExports],
  providers: [DatePipe],
  entryComponents: [
    HttpErrorWrapperComponent,
    LoadingComponent,
    ModalContainerComponent,
    ToastContainerComponent,
    ConfirmationComponent,
  ],
})
export class BaseThemeSharedModule {}

@NgModule({
  imports: [BaseThemeSharedModule],
  exports: [BaseThemeSharedModule],
})
export class ThemeSharedModule {
  static forRoot(
    { httpErrorConfig, validation = {} } = {} as RootParams,
  ): ModuleWithProviders<ThemeSharedModule> {
    return {
      ngModule: ThemeSharedModule,
      providers: [
        {
          provide: APP_INITIALIZER,
          multi: true,
          deps: [ErrorHandler],
          useFactory: noop,
        },
        THEME_SHARED_ROUTE_PROVIDERS,
        {
          provide: APP_INITIALIZER,
          multi: true,
          deps: [THEME_SHARED_APPEND_CONTENT],
          useFactory: noop,
        },
        {
          provide: APP_INITIALIZER,
          multi: true,
          deps: [Injector],
          useFactory: initLazyStyleHandler,
        },
        { provide: HTTP_ERROR_CONFIG, useValue: httpErrorConfig },
        {
          provide: 'HTTP_ERROR_CONFIG',
          useFactory: httpErrorConfigFactory,
          deps: [HTTP_ERROR_CONFIG],
        },
        { provide: NgbDateParserFormatter, useClass: DateParserFormatter },
        NG_BOOTSTRAP_CONFIG_PROVIDERS,
        {
          provide: VALIDATION_BLUEPRINTS,
          useValue: {
            ...DEFAULT_VALIDATION_BLUEPRINTS,
            ...(validation.blueprints || {}),
          },
        },
        {
          provide: VALIDATION_MAP_ERRORS_FN,
          useValue: validation.mapErrorsFn || defaultMapErrorsFn,
        },
        {
          provide: VALIDATION_VALIDATE_ON_SUBMIT,
          useValue: validation.validateOnSubmit,
        },
      ],
    };
  }
}
