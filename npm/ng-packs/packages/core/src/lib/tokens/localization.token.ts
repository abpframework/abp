import { InjectionToken } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ABP } from '../models/common';

export const LOCALIZATIONS = new InjectionToken('LOCALIZATIONS');

export function localizationContributor(localizations: ABP.Localization[]) {
  if (localizations) {
    localizations$.next([...localizations$.value, ...localizations]);
  }
}

export const localizations$ = new BehaviorSubject([]);
