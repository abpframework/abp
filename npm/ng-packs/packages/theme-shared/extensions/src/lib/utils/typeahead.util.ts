import { ABP, ExtensionPropertyUiLookupDto, RestService } from '@abp/ng.core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { ePropType } from '../enums/props.enum';
import { PropCallback } from '../models/props';

export function createTypeaheadOptions(
  lookup: ExtensionPropertyUiLookupDto,
): PropCallback<any, Observable<ABP.Option<any>[]>> {
  return (data, searchText) =>
    searchText
      ? data
          .getInjected(RestService)
          .request(
            {
              method: 'GET',
              url: lookup.url,
              params: {
                [lookup.filterParamName]: searchText,
              },
            },
            { apiName: 'Default' },
          )
          .pipe(
            map(response => {
              const list = response[lookup.resultListPropertyName];
              const mapToOption = (item: any) => ({
                key: item[lookup.displayPropertyName],
                value: item[lookup.valuePropertyName],
              });
              return list.map(mapToOption);
            }),
          )
      : of([]);
}

export function getTypeaheadType(lookup: ExtensionPropertyUiLookupDto, name: string) {
  return Boolean(lookup.url)
    ? ePropType.Typeahead
    : name.endsWith('_Text')
    ? ePropType.Hidden
    : undefined;
}
