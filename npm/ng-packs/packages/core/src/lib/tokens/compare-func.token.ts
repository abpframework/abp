import { InjectionToken, inject } from '@angular/core';
import { LocalizationService } from '../services';

export const SORT_COMPARE_FUNC = new InjectionToken< 0 | 1 | -1 >('SORT_COMPARE_FUNC');

export function compareFuncFactory() {
  const localizationService = inject(LocalizationService)
  const fn = (a,b) => {
    const aName = localizationService.instant(a.name);
    const bName = localizationService.instant(b.name);
    const aNumber = a.order;
    const bNumber = b.order;
    
    if (!Number.isInteger(aNumber)) return 1;
    if (!Number.isInteger(bNumber)) return -1;
  
    if (aNumber > bNumber) return 1
    if (aNumber < bNumber) return -1
    
    if ( aName > bName ) return 1;
    if ( aName < bName ) return -1;
  
    return 0
  }

  return fn
}