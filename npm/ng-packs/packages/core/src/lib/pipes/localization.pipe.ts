import { Pipe, PipeTransform, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import { distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Pipe({
  name: 'abpLocalization',
  pure: false, // required to update the value
})
export class LocalizationPipe implements PipeTransform, OnDestroy {
  initialValue: string = '';

  value: string;

  destroy$ = new Subject();

  constructor(private store: Store) {}

  transform(value: string = '', ...interpolateParams: string[]): string {
    if (this.initialValue !== value) {
      this.initialValue = value;
      this.destroy$.next();

      this.store
        .select(
          ConfigState.getCopy(
            value,
            ...interpolateParams.reduce((acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val]), []),
          ),
        )
        .pipe(
          takeUntil(this.destroy$),
          takeUntilDestroy(this),
          distinctUntilChanged(),
        )
        .subscribe(copy => (this.value = copy));
    }

    return this.value;
  }

  ngOnDestroy() {}
}
