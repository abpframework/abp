import { Component, input } from '@angular/core';

@Component({
  selector: 'abp-card',
  template: ` <div class="card" [class]="cardClass()" [style]="cardStyle()">
    <ng-content></ng-content>
  </div>`,
  imports: [],
})
export class CardComponent {
  readonly cardClass = input<string>(undefined);

  readonly cardStyle = input<string>(undefined);
}
