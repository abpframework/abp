import { InjectionToken } from '@angular/core';

export interface NgxDatatableMessages {
  emptyMessage: string;
  errorMessage: string;
  totalMessage: string;
  selectedMessage: string;
}

export const defaultNgxDatatableMessages = {
  emptyMessage: 'AbpUi::NoDataAvailableInDatatable',
  errorMessage: 'AbpUi::ErrorLoadingDatatable',
  totalMessage: 'AbpUi::Total',
  selectedMessage: 'AbpUi::Selected',
};

export const NGX_DATATABLE_MESSAGES = new InjectionToken<Partial<NgxDatatableMessages>>(
  'NGX_DATATABLE_MESSAGES',
);
