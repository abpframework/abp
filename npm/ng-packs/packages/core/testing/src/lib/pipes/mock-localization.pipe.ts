import { Config } from '@abp/ng.core';
import { Injectable, Pipe, PipeTransform } from '@angular/core';

@Injectable()
@Pipe({
  name: 'abpLocalization',
})
export class MockLocalizationPipe implements PipeTransform {
  transform(value: string | Config.LocalizationWithDefault = '', ..._: string[]) {
    return typeof value === 'string' ? value : value.defaultValue;
  }
}
