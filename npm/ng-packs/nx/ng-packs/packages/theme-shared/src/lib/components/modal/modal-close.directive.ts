import { Directive, HostListener, Optional } from '@angular/core';
import { ModalComponent } from './modal.component';

@Directive({ selector: '[abpClose]' })
export class ModalCloseDirective {
  constructor(@Optional() private modal: ModalComponent) {
    if (!modal) {
      console.error('Please use abpClose within an abp-modal');
    }
  }

  @HostListener('click')
  onClick() {
    this.modal?.close();
  }
}
