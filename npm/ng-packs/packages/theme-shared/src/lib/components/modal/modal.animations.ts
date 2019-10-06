import { transition, trigger, useAnimation } from '@angular/animations';
import { fadeIn, fadeInDown, fadeOut } from '../../animations/fade.animations';

export const backdropAnimation = trigger('backdrop', [
  transition(':enter', useAnimation(fadeIn)),
  transition(':leave', useAnimation(fadeOut)),
]);

export const dialogAnimation = trigger('dialog', [
  transition(':enter', useAnimation(fadeInDown)),
  transition(':leave', useAnimation(fadeOut)),
]);
