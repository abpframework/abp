import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LocalizationService {
  constructor(private store: Store) {}

  get(keys: string, ...interpolateParams: string[]): Observable<string> {
    return this.store.select(ConfigState.getCopy(keys, ...interpolateParams));
  }

  instant(keys: string, ...interpolateParams: string[]): string {
    return this.store.selectSnapshot(ConfigState.getCopy(keys, ...interpolateParams));
  }
}
