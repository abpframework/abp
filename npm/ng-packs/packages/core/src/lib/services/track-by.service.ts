import { Injectable, TrackByFunction } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TrackByService<ItemType = any> {
  by<T = ItemType>(key: keyof T): TrackByFunction<T> {
    return ({}, item) => item[key];
  }

  byDeep<T = ItemType>(...keys: (string | number)[]): TrackByFunction<T> {
    return ({}, item) => keys.reduce((acc, key) => acc[key], item);
  }

  bySelf<T = ItemType>(): TrackByFunction<T> {
    return ({}, item) => item;
  }
}
