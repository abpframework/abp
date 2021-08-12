import { ComponentFixture } from '@angular/core/testing';

export function clearPage<T extends any>(_fixture: ComponentFixture<T>) {
  if (!document) return;

  const elements = document.querySelectorAll('body > *');
  elements.forEach(element => {
    if (/^(abp|ngb)-/i.test(element.tagName)) document.body.removeChild(element);
  });
}
