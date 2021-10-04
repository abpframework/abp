import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { LocalizationWithDefault } from '../models/localization';
import { LocalizationService } from '../services/localization.service';

@Injectable()
@Pipe({
  name: 'abpLocalization',
})
export class LocalizationPipe implements PipeTransform {
  constructor(private localization: LocalizationService) {}

  transform(value: string | LocalizationWithDefault = '', ...interpolateParams: string[]): string {
    return this.localization.instant(
      value,
      ...interpolateParams.reduce(
        (acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val]),
        [],
      ),
    );
  }
}
