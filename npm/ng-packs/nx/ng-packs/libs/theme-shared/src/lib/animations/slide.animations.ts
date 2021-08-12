import { animate, state, style, transition, trigger, query } from '@angular/animations';
export const slideFromBottom = trigger('slideFromBottom', [
  transition('* <=> *', [
    style({ 'margin-top': '20px', opacity: '0' }),
    animate('0.2s ease-out', style({ opacity: '1', 'margin-top': '0px' })),
  ]),
]);
