import { Directive } from "@angular/core";

@Directive({
  selector: `abp-card-subtitle, [abp-card-subtitle], [abpCardSubtitle]`,
  host: {
    class: 'card-subtitle',
  },
})
export class CardSubtitle {}
