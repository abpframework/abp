import {Component, Injector, Input} from '@angular/core';
import {AbstractNgModelComponent} from "@abp/ng.core";

@Component({
  selector: 'abp-password',
  templateUrl: `password.component.html`,
})
export class PasswordComponent extends AbstractNgModelComponent{
  @Input() inputId!: string;

  fieldTextType: boolean;

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
}
