import { InjectionToken, inject } from '@angular/core';
import type { SortableItem } from '../models';
import { LocalizationService } from '../services';

export const SORT_COMPARE_FUNC = new InjectionToken<(a: SortableItem, b: SortableItem) => number>(
  'SORT_COMPARE_FUNC',
);

export function compareFuncFactory() {
  const localizationService = inject(LocalizationService);
  const fn = (a: SortableItem, b: SortableItem) => {
    const aNumber = a.order;
    const bNumber = b.order;

    if (aNumber > bNumber) return 1;
    if (aNumber < bNumber) return -1;

    if (a.id > b.id) return 1;
    if (a.id < b.id) return -1;

    if (!Number.isInteger(aNumber)) return 1;
    if (!Number.isInteger(bNumber)) return -1;

    const aName = localizationService.instant(a.name);
    const bName = localizationService.instant(b.name);

    if (aName > bName) return 1;
    if (aName < bName) return -1;

    return 0;
  };

  return fn;
}
