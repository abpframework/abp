import { Component, Input } from '@angular/core';

@Component({
  selector: 'abp-card-footer',
  template: `
    <div [ngStyle]="cardFooterStyle" [ngClass]="cardFooterClass" >
      <ng-content ></ng-content>
    </div>
  `,
  styles: [],
  host: {
    class: 'card-footer',
  }
})
export class CardFooterComponent {
  @Input() cardFooterStyle: string;
  @Input() cardFooterClass: string;

}
