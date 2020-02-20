import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { DatePipe } from '@angular/common';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ChartComponent } from './components/chart/chart.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { HttpErrorWrapperComponent } from './components/http-error-wrapper/http-error-wrapper.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { ModalComponent } from './components/modal/modal.component';
import { SortOrderIconComponent } from './components/sort-order-icon/sort-order-icon.component';
import { TableEmptyMessageComponent } from './components/table-empty-message/table-empty-message.component';
import { ToastContainerComponent } from './components/toast-container/toast-container.component';
import { TableComponent } from './components/table/table.component';
import { ToastComponent } from './components/toast/toast.component';
import styles from './constants/styles';
import { TableSortDirective } from './directives/table-sort.directive';
import { ErrorHandler } from './handlers/error.handler';
import { RootParams } from './models/common';
import { httpErrorConfigFactory, HTTP_ERROR_CONFIG } from './tokens/http-error.token';
import { DateParserFormatter } from './utils/date-parser-formatter';
import { chartJsLoaded$ } from './utils/widget-utils';
import { PaginationComponent } from './components/pagination/pagination.component';
import { LoadingComponent } from './components/loading/loading.component';
import { LoadingDirective } from './directives/loading.directive';

export function appendScript(injector: Injector) {
  const fn = () => {
    import('chart.js').then(() => chartJsLoaded$.next(true));

    const lazyLoadService: LazyLoadService = injector.get(LazyLoadService);
    return lazyLoadService.load(null, 'style', styles, 'head', 'beforeend').toPromise();
  };

  return fn;
}

@NgModule({
  imports: [CoreModule, NgxValidateCoreModule],
  declarations: [
    BreadcrumbComponent,
    ButtonComponent,
    ChartComponent,
    ConfirmationComponent,
    HttpErrorWrapperComponent,
    LoaderBarComponent,
    LoadingComponent,
    ModalComponent,
    PaginationComponent,
    TableComponent,
    TableEmptyMessageComponent,
    ToastComponent,
    ToastContainerComponent,
    SortOrderIconComponent,
    LoadingDirective,
    TableSortDirective,
  ],
  exports: [
    BreadcrumbComponent,
    ButtonComponent,
    ChartComponent,
    ConfirmationComponent,
    LoaderBarComponent,
    LoadingComponent,
    ModalComponent,
    PaginationComponent,
    TableComponent,
    TableEmptyMessageComponent,
    ToastComponent,
    ToastContainerComponent,
    SortOrderIconComponent,
    LoadingDirective,
    TableSortDirective,
  ],
  providers: [DatePipe],
  entryComponents: [HttpErrorWrapperComponent, LoadingComponent],
})
export class ThemeSharedModule {
  constructor(private errorHandler: ErrorHandler) {}

  static forRoot(options = {} as RootParams): ModuleWithProviders {
    return {
      ngModule: ThemeSharedModule,
      providers: [
        {
          provide: APP_INITIALIZER,
          multi: true,
          deps: [Injector],
          useFactory: appendScript,
        },
        { provide: HTTP_ERROR_CONFIG, useValue: options.httpErrorConfig },
        {
          provide: 'HTTP_ERROR_CONFIG',
          useFactory: httpErrorConfigFactory,
          deps: [HTTP_ERROR_CONFIG],
        },
        { provide: NgbDateParserFormatter, useClass: DateParserFormatter },
      ],
    };
  }
}
