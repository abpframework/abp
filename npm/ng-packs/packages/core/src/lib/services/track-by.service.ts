import { Injectable, TrackByFunction } from '@angular/core';
import { Paths } from 'type-fest';

export const trackBy =
  <T = any>(key: keyof T): TrackByFunction<T> =>
  (_, item) =>
    item[key];

export const trackByDeep =
  <T = any>(
    // eslint-disable-next-line @typescript-eslint/ban-types
    ...keys: T extends object ? Paths<T>[] : never
  ): TrackByFunction<T> =>
  (_, item) => {
    keys.reduce(acc => acc, item);
  };

@Injectable({
  providedIn: 'root',
})
export class TrackByService<ItemType = any> {
  by = trackBy;

  byDeep = trackByDeep;
}
