import { ComponentFixture } from '@angular/core/testing';

export function wait<T extends any>(fixture: ComponentFixture<T>, timeout = 0) {
  fixture.detectChanges();
  return new Promise(res => setTimeout(res, timeout));
}
