import { Pipe, PipeTransform, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import { distinctUntilChanged } from 'rxjs/operators';

@Pipe({
  name: 'abpLocalization',
  pure: false, // required to update the value
})
export class LocalizationPipe implements PipeTransform, OnDestroy {
  initialized: boolean = false;

  value: string;

  constructor(private store: Store) {}

  transform(value: string, ...interpolateParams: string[]): string {
    if (!this.initialized) {
      this.initialized = true;

      this.store
        .select(
          ConfigState.getCopy(
            value,
            ...interpolateParams.reduce((acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val]), []),
          ),
        )
        .pipe(
          takeUntilDestroy(this),
          distinctUntilChanged(),
        )
        .subscribe(copy => (this.value = copy));
    }

    return this.value;
  }

  ngOnDestroy() {}
}
