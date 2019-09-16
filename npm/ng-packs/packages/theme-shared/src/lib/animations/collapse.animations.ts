import { trigger, state, style, transition, animate } from '@angular/animations';

export const collapse = trigger('collapse', [
  state(
    'open',
    style({
      height: '*',
      overflow: 'hidden',
    }),
  ),
  state(
    'close',
    style({
      height: '0px',
      overflow: 'hidden',
    }),
  ),
  transition('open <=> close', animate(350)),
]);
