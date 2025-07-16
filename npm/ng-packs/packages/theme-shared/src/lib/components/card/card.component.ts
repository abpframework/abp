import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'abp-card',
  template: ` <div class="card" [ngClass]="cardClass" [ngStyle]="cardStyle">
    <ng-content></ng-content>
  </div>`,
  imports: [CommonModule],
})
export class CardComponent {
  @Input() cardClass: string;

  @Input() cardStyle: string;
}
