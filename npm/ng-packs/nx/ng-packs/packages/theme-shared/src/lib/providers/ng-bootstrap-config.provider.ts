import { APP_INITIALIZER } from '@angular/core';
import { NgbInputDatepickerConfig, NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';

export const NG_BOOTSTRAP_CONFIG_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureNgBootstrap,
    deps: [NgbInputDatepickerConfig, NgbTypeaheadConfig],
    multi: true,
  },
];

export function configureNgBootstrap(
  datepicker: NgbInputDatepickerConfig,
  typeahead: NgbTypeaheadConfig,
) {
  return () => {
    datepicker.container = 'body';
    typeahead.container = 'body';
  };
}
