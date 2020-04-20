import { Component, ViewChild, ViewContainerRef } from '@angular/core';

@Component({
  selector: 'abp-modal-container',
  template: `
    <ng-container #container></ng-container>
  `,
})
export class ModalContainerComponent {
  @ViewChild('container', { static: true, read: ViewContainerRef })
  container: ViewContainerRef;
}
