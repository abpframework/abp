import { Directive } from "@angular/core";

@Directive({
  selector: `abp-card-header, [abp-card-header], [abpCardHeader]`,
  host: {
    class: 'card-header',
  },
})
export class CardHeader {}
