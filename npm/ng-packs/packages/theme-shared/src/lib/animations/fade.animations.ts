import { animate, state, style, transition, trigger } from '@angular/animations';

export const fade = trigger('fade', [
  state('void', style({ opacity: 1 })),
  transition(':enter', [style({ opacity: 0 }), animate(250)]),
  transition(':leave', animate(250, style({ opacity: 0 }))),
]);

export const fadeWithStates = trigger('fadeInOut', [
  state('out', style({ opacity: 0 })),
  state('in', style({ opacity: 1 })),
  transition('in <=> out', [animate(250)]),
]);

export const fadeIn = trigger('fadeIn', [
  state('*', style({ opacity: 1 })),
  transition('* => *', [style({ opacity: 0 }), animate(250)]),
  transition(':enter', [style({ opacity: 0 }), animate(250)]),
]);
