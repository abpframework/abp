import { transition, trigger, useAnimation } from '@angular/animations';
import { fadeIn, fadeInDown, fadeOut } from './fade.animations';

export const fadeAnimation = trigger('fade', [
  transition(':enter', useAnimation(fadeIn)),
  transition(':leave', useAnimation(fadeOut)),
]);

export const dialogAnimation = trigger('dialog', [
  transition(':enter', useAnimation(fadeInDown)),
  transition(':leave', useAnimation(fadeOut)),
]);
