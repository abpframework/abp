import { Directive, HostListener, inject } from '@angular/core';
import { ModalComponent } from './modal.component';

@Directive({
  selector: '[abpClose]',
})
export class ModalCloseDirective {
  private modal = inject(ModalComponent, { optional: true })!;

  constructor() {
    const modal = this.modal;

    if (!modal) {
      console.error('Please use abpClose within an abp-modal');
    }
  }

  @HostListener('click')
  onClick() {
    this.modal?.close();
  }
}
