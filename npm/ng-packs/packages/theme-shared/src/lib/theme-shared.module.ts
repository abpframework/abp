import { CoreModule, LazyLoadService } from '@abp/ng.core';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { MessageService } from 'primeng/components/common/messageservice';
import { ToastModule } from 'primeng/toast';
import { forkJoin } from 'rxjs';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { ButtonComponent } from './components/button/button.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ChartComponent } from './components/chart/chart.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { ErrorComponent } from './components/error/error.component';
import { LoaderBarComponent } from './components/loader-bar/loader-bar.component';
import { ModalComponent } from './components/modal/modal.component';
import { ProfileComponent } from './components/profile/profile.component';
import { SortOrderIconComponent } from './components/sort-order-icon/sort-order-icon.component';
import { TableEmptyMessageComponent } from './components/table-empty-message/table-empty-message.component';
import { ToastComponent } from './components/toast/toast.component';
import styles from './contants/styles';
import { TableSortDirective } from './directives/table-sort.directive';
import { ErrorHandler } from './handlers/error.handler';
import { chartJsLoaded$ } from './utils/widget-utils';

export function appendScript(injector: Injector) {
  const fn = () => {
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
    ).toPromise();
  };

  return fn;
}

@NgModule({
  imports: [CoreModule, ToastModule, NgxValidateCoreModule],
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
    TableEmptyMessageComponent,
    ToastComponent,
    SortOrderIconComponent,
    TableSortDirective,
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
    TableEmptyMessageComponent,
    ToastComponent,
    SortOrderIconComponent,
    TableSortDirective,
  ],
  entryComponents: [ErrorComponent],
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
