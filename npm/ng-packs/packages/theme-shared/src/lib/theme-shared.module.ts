import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { forkJoin } from 'rxjs';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ChartComponent } from './components/chart/chart.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { HttpErrorWrapperComponent } from './components/http-error-wrapper/http-error-wrapper.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { ModalComponent } from './components/modal/modal.component';
import { SortOrderIconComponent } from './components/sort-order-icon/sort-order-icon.component';
import { TableEmptyMessageComponent } from './components/table-empty-message/table-empty-message.component';
import { ToastComponent } from './components/toast/toast.component';
import styles from './constants/styles';
import { TableSortDirective } from './directives/table-sort.directive';
import { ErrorHandler } from './handlers/error.handler';
import { chartJsLoaded$ } from './utils/widget-utils';
import { RootParams } from './models/common';
import { HTTP_ERROR_CONFIG, httpErrorConfigFactory } from './tokens/http-error.token';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { DateParserFormatter } from './utils/date-parser-formatter';
import { DatePipe } from '@angular/common';

export function appendScript(injector: Injector) {
  const fn = () => {
    import('chart.js').then(() => chartJsLoaded$.next(true));

    const lazyLoadService: LazyLoadService = injector.get(LazyLoadService);
    return lazyLoadService.load(null, 'style', styles, 'head', 'beforeend').toPromise();
  };

  return fn;
}

@NgModule({
  imports: [CoreModule, ToastModule, NgxValidateCoreModule],
  declarations: [
    BreadcrumbComponent,
    ButtonComponent,
    ChartComponent,
    ConfirmationComponent,
    HttpErrorWrapperComponent,
    LoaderBarComponent,
    ModalComponent,
    TableEmptyMessageComponent,
    ToastComponent,
    SortOrderIconComponent,
    TableSortDirective,
  ],
  exports: [
    BreadcrumbComponent,
    ButtonComponent,
    ChartComponent,
    ConfirmationComponent,
    LoaderBarComponent,
    ModalComponent,
    TableEmptyMessageComponent,
    ToastComponent,
    SortOrderIconComponent,
    TableSortDirective,
  ],
  providers: [DatePipe],
  entryComponents: [HttpErrorWrapperComponent],
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
        { provide: MessageService, useClass: MessageService },
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
