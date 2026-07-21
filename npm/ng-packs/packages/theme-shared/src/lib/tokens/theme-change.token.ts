import { InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';

export const THEME_CHANGE_TOKEN = new InjectionToken<Observable<{ styleName: string }>>(
  'THEME_CHANGE_TOKEN',
);
