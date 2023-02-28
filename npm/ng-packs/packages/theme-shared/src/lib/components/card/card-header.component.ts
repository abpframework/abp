import { Component, Input } from '@angular/core';

@Component({
  selector: 'abp-card-header',
  template: `
    <div [ngClass]="cardHeaderClass" [ngStyle]="cardHeaderStyle">
      <ng-content ></ng-content>
    </div>
  `,
  styles: [],
  host: {
    class: 'card-header',
  }
})
export class CardHeaderComponent {
  @Input() cardHeaderClass: string;
  @Input() cardHeaderStyle: string;
}
