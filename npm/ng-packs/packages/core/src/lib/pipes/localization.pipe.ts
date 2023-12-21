import { Injectable, Pipe, PipeTransform, inject } from '@angular/core';
import { LocalizationWithDefault } from '../models/localization';
import { LocalizationService } from '../services/localization.service';
import { AsyncPipe } from '@angular/common';

@Injectable()
@Pipe({
  name: 'abpLocalization',
})
export class LocalizationPipe implements PipeTransform {
  private localization = inject(LocalizationService)
   asyncPipe = inject(AsyncPipe);

  transform(
    value: string | LocalizationWithDefault = '',
    ...interpolateParams: (string | string[] | undefined)[]
  ): string {
    
    const params = this.reduceParams(interpolateParams)
    const localize$ = this.localization.get(value, ...params);
    return this.asyncPipe.transform(localize$) || '';

  }
  private reduceParams( paramaters : (string | string[] | undefined)[]){

   return  paramaters.reduce((acc, val) => {
      if (!acc) {
        return val;
      }
      if (!val) {
        return acc;
      }
      return Array.isArray(val) ? [...acc, ...val] : [...acc, val];
    }, []) || [];
  }
}
