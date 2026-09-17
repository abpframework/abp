import { inject, Injectable, Pipe, PipeTransform } from '@angular/core';
import {
  Observable,
  of,
  filter,
  take,
  switchMap,
  map,
  startWith,
  distinctUntilChanged,
} from 'rxjs';
import { ConfigStateService, LocalizationService } from '../services';

@Injectable()
@Pipe({
  name: 'abpAsyncLocalization',
})
export class AsyncLocalizationPipe implements PipeTransform {
  private localizationService = inject(LocalizationService);
  private configStateService = inject(ConfigStateService);

  transform(key: string, ...params: (string | string[])[]): Observable<string> {
    if (!key) {
      return of('');
    }

    const flatParams = params.reduce<string[]>(
      (acc, val) => (Array.isArray(val) ? acc.concat(val) : [...acc, val]),
      [],
    );

    return this.configStateService.getAll$().pipe(
      filter(config => !!config.localization),
      take(1),
      switchMap(() => this.localizationService.get(key, ...flatParams)),
      map(translation => (translation && translation !== key ? translation : '')),
      startWith(''),
      distinctUntilChanged(),
    );
  }
}
