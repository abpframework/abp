import { Component, Input, TemplateRef } from '@angular/core';

@Component({
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
})
export class AuthWrapperComponent {
  @Input()
  mainContentRef: TemplateRef<any>;

  @Input()
  cancelContentRef: TemplateRef<any>;
}
