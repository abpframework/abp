import { Component, ContentChild, Input } from '@angular/core';
import { CardBodyComponent } from './card-body.component';
import { CardTitleComponent } from './card-title.component';

@Component({
  selector: 'abp-card',
  template: `
  <div class="card" [ngClass]="class" [ngStyle]="style" >
  <div class="card-title" *ngIf="cardTitleTemplate">
    <ng-content select="abp-card-title"></ng-content>
  </div>
  <div class="card-body" *ngIf="cardBodyTemplate">
    <ng-content select="abp-card-body"></ng-content>
  </div>
</div>`,
})
export class CardComponent {
  @Input() class:string

  @Input() style:string

  @ContentChild(CardBodyComponent)
  cardBodyTemplate?: CardBodyComponent;

  @ContentChild(CardTitleComponent)
  cardTitleTemplate?: CardTitleComponent;
}
