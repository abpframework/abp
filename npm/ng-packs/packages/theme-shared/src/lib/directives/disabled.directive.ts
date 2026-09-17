import { Directive, effect, inject, input } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[abpDisabled]',
})
export class DisabledDirective {
  private ngControl = inject(NgControl, { host: true });

  readonly abpDisabled = input(false);

  // Related issue: https://github.com/angular/angular/issues/35330
  private disabledEffect = effect(() => {
    const disabled = this.abpDisabled();
    if (this.ngControl.control) {
      this.ngControl.control[disabled ? 'disable' : 'enable']();
    }
  });
}
