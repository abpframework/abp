import { trigger, state, style, transition, useAnimation } from '@angular/animations';
import { fadeInDown, fadeOut, fadeIn } from '../../animations';

export const backdropAnimation = trigger('backdrop', [
  state('void', style({ opacity: '0' })),
  state('*', style({ opacity: '1' })),
  transition('void => *', useAnimation(fadeIn)),
  transition('* => void', useAnimation(fadeOut))
]);

export const dialogAnimation = trigger('dialog', [
  transition('void => *', useAnimation(fadeInDown)),
  transition('* => void', useAnimation(fadeOut))
]);
