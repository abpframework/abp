import { Injectable, Pipe, PipeTransform, inject } from '@angular/core';
import { LocalizationWithDefault } from '../models/localization';
import { LocalizationService } from '../services/localization.service';

@Injectable()
@Pipe({
  name: 'abpLocalization',
})
export class LocalizationPipe implements PipeTransform {
  private localization = inject(LocalizationService);


  transform(
    value: string | LocalizationWithDefault = '',
    ...interpolateParams: (string | string[] | undefined)[]
  ): string {
    const params =
      interpolateParams.reduce((acc, val) => {
        if (!acc) {
          return val;
        }
        if (!val) {
          return acc;
        }
        return Array.isArray(val) ? [...acc, ...val] : [...acc, val];
      }, []) || [];
    return this.localization.instant(value, ...params);
  }
}
