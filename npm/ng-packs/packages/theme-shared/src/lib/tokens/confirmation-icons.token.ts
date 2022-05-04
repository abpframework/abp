import { InjectionToken } from '@angular/core';

export interface ConfirmationIcons {
  info: string;
  success: string;
  warning: string;
  error: string;
  default: string;
}

export const CONFIRMATION_ICONS = new InjectionToken<Partial<ConfirmationIcons>>(
  'CONFIRMATION_ICONS',
);

export const DEFAULT_CONFIRMATION_ICONS: ConfirmationIcons = {
  info: 'fa fa-info-circle',
  success: 'fa fa-check-circle',
  warning: 'fa fa-exclamation-triangle',
  error: 'fa fa-times-circle',
  default: 'fa fa-question-circle',
};
