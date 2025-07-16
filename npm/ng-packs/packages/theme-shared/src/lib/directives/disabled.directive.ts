import { Directive, Host, Input, OnChanges, SimpleChanges } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[abpDisabled]',
})
export class DisabledDirective implements OnChanges {
  @Input()
  abpDisabled = false;

  constructor(@Host() private ngControl: NgControl) {}

  // Related issue: https://github.com/angular/angular/issues/35330
  ngOnChanges({ abpDisabled }: SimpleChanges) {
    if (this.ngControl.control && abpDisabled) {
      this.ngControl.control[abpDisabled.currentValue ? 'disable' : 'enable']();
    }
  }
}
