import { Directive, HostBinding } from '@angular/core';

@Directive({
  selector: `abp-card-title, [abp-card-title], [abpCardTitle]`,
})
export class CardTitleDirective {
  @HostBinding('class') directiveClass = 'card-title';
}
