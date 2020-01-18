import { Pipe, PipeTransform, Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Config } from '../models';
import { ConfigState } from '../states';

@Injectable()
@Pipe({
  name: 'abpLocalization',
})
export class LocalizationPipe implements PipeTransform {
  constructor(private store: Store) {}

  transform(value: string | Config.LocalizationWithDefault = '', ...interpolateParams: string[]): string {
    return this.store.selectSnapshot(
      ConfigState.getLocalization(
        value,
        ...interpolateParams.reduce((acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val]), []),
      ),
    );
  }
}
