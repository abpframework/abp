import { inject, provideAppInitializer } from '@angular/core';
import { NgbDatepickerI18n, NgbInputDatepickerConfig, NgbTypeaheadConfig, NgbTimepickerI18n } from '@ng-bootstrap/ng-bootstrap';
import { DatepickerI18nAdapter, TimepickerI18nAdapter } from '../adapters';

export const NG_BOOTSTRAP_CONFIG_PROVIDERS = [
  {
    provide: NgbDatepickerI18n,
    useClass: DatepickerI18nAdapter,
  },
  {
    provide: NgbTimepickerI18n,
    useClass: TimepickerI18nAdapter,
  },
  provideAppInitializer(() => {
    configureNgBootstrap();
  }),
];

export function configureNgBootstrap() {
  const datepicker: NgbInputDatepickerConfig = inject(NgbInputDatepickerConfig);
  const typeahead: NgbTypeaheadConfig = inject(NgbTypeaheadConfig);
  datepicker.container = 'body';
  typeahead.container = 'body';
}
