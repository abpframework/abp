import { Injector } from '@angular/core';
import { NgbInputDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';

export function initDatepickerConfig(injector: Injector) {
  const datepickerConfig = injector.get(NgbInputDatepickerConfig, null);
  if (datepickerConfig) {
    datepickerConfig.container = 'body';
  }

  return () => {};
}
