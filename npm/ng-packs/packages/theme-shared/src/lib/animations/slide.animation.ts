import { animate, state, style, transition, trigger, query } from '@angular/animations';
export const slideFromBottom = trigger('routeAnimations', [
  state('void', style({ 'margin-top': '20px', opacity: '0' })),
  state('*', style({ 'margin-top': '0px', opacity: '1' })),
  transition(':enter', [animate('0.2s ease-out', style({ opacity: '1', 'margin-top': '0px' }))]),
]);
