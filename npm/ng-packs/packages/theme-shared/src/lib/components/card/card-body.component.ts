import { Component, Input } from '@angular/core';

@Component({
  selector: 'abp-card-body',
  template: `
    <div [ngClass]="cardBodyClass" [ngStyle]="cardBodyStyle" >
      <ng-content></ng-content>
    </div>`,
  host: {
    class: 'card-body',
  }
})
export class CardBodyComponent {
  @Input() cardBodyClass: string;
  @Input() cardBodyStyle: string;
}
