import { Component, Input } from '@angular/core';

@Component({
  selector: 'abp-card',
  template: ` <div class="card" [class]="cardClass" [style]="cardStyle">
    <ng-content></ng-content>
  </div>`,
  imports: [],
})
export class CardComponent {
  @Input() cardClass: string;

  @Input() cardStyle: string;
}
