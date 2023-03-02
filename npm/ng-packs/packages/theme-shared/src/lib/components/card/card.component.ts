import { Component, Directive, Input } from '@angular/core';

@Directive({
  selector: `abp-card-header, [abp-card-header], [abpCardHeader]`,
  host: {
    class: 'card-header',
  },
})
export class CardHeader {}

@Directive({
  selector: `abp-card-title, [abp-card-title], [abpCardTitle]`,
  host: {
    class: 'card-title',
  },
})
export class CardTitle {}

@Directive({
  selector: `abp-card-subtitle, [abp-card-subtitle], [abpCardSubtitle]`,
  host: {
    class: 'card-subtitle',
  },
})
export class CardSubtitle {}

@Directive({
  selector: `abp-card-text, [abp-card-text], [abpCardText]`,
  host: {
    class: 'card-text',
  },
})
export class CardText {}

@Directive({
  selector: `abp-card-img-top, [abp-card-img-top], [abpCardImgTop]`,
  host: {
    class: 'card-img-top',
  },
})
export class CardImgTop {}

@Directive({
  selector: `abp-card-link, [abp-card-link], [abpCardLink]`,
  host: {
    class: 'card-link',
  },
})
export class CardLink {}

@Component({
  selector: 'abp-card',
  template: ` <div class="card" [ngClass]="cardClass" [ngStyle]="cardStyle">
    <ng-content></ng-content>
  </div>`,
})
export class CardComponent {
  @Input() cardClass: string;

  @Input() cardStyle: string;
}
