import { Injectable, TrackByFunction } from '@angular/core';
import { O } from 'ts-toolbelt';

@Injectable({
  providedIn: 'root',
})
export class TrackByService<ItemType = any> {
  by<T = ItemType>(key: keyof T): TrackByFunction<T> {
    return (_, item) => item[key];
  }

  byDeep<T = ItemType>(...keys: T extends object ? O.Paths<T> : never): TrackByFunction<T> {
    return (_, item) => keys.reduce((acc, key) => acc[key], item);
  }
}
