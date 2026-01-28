import { Directive, OnChanges, SimpleChanges, inject, input } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[abpDisabled]',
})
export class DisabledDirective implements OnChanges {
  private ngControl = inject(NgControl, { host: true });

  readonly abpDisabled = input(false);

  // Related issue: https://github.com/angular/angular/issues/35330
  ngOnChanges({ abpDisabled }: SimpleChanges) {
    if (this.ngControl.control && abpDisabled) {
      this.ngControl.control[abpDisabled.currentValue ? 'disable' : 'enable']();
    }
  }
}
