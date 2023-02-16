import { Component, ContentChild, Input } from '@angular/core';
import { CardBodyComponent } from './card-body.component';
import { CardTitleComponent } from './card-title.component';

@Component({
  selector: 'abp-card',
  template: ` <div class="card" [ngClass]="cardClass" [ngStyle]="cardStyle">
    <ng-content *ngIf="cardTitleTemplate" select="abp-card-title"></ng-content>
    <ng-content *ngIf="cardBodyTemplate" select="abp-card-body"></ng-content>
  </div>`,
})
export class CardComponent {
  @Input() cardClass: string;

  @Input() cardStyle: string;

  @ContentChild(CardBodyComponent)
  cardBodyTemplate?: CardBodyComponent;

  @ContentChild(CardTitleComponent)
  cardTitleTemplate?: CardTitleComponent;
}
