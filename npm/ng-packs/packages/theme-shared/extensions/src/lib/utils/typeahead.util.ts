import { ABP, ExtensionPropertyUiLookupDto, RestService } from '@abp/ng.core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { ePropType } from '../enums/props.enum';
import { ObjectExtensions } from '../models/object-extensions';
import { PropCallback } from '../models/props';

const TYPEAHEAD_TEXT_SUFFIX = '_Text';
const TYPEAHEAD_TEXT_SUFFIX_REGEX = /_Text$/;

export function createTypeaheadOptions(
  lookup: ExtensionPropertyUiLookupDto,
): PropCallback<any, Observable<ABP.Option<any>[]>> {
  return (data, searchText) =>
    searchText && data
      ? data
          .getInjected(RestService)
          .request(
            {
              method: 'GET',
              url: lookup.url || '',
              params: {
                [lookup.filterParamName || '']: searchText,
              },
            },
            { apiName: 'Default' },
          )
          .pipe(
            map((response: any) => {
              const list = response[lookup.resultListPropertyName || ''];
              const mapToOption = (item: any) => ({
                key: item[lookup.displayPropertyName || ''],
                value: item[lookup.valuePropertyName || ''],
              });
              return list.map(mapToOption);
            }),
          )
      : of([]);
}

export function getTypeaheadType(lookup: ExtensionPropertyUiLookupDto, name: string) {
  return Boolean(lookup.url)
    ? ePropType.Typeahead
    : name.endsWith(TYPEAHEAD_TEXT_SUFFIX)
    ? ePropType.Hidden
    : undefined;
}

export function createTypeaheadDisplayNameGenerator(
  displayNameGeneratorFn: ObjectExtensions.DisplayNameGeneratorFn,
  properties: ObjectExtensions.EntityExtensionProperties,
): ObjectExtensions.DisplayNameGeneratorFn {
  return (displayName, fallback) => {
    const name = removeTypeaheadTextSuffix(fallback.name || '');
    return displayNameGeneratorFn(displayName || properties[name].displayName, {
      name,
      resource: fallback.resource,
    });
  };
}

export function addTypeaheadTextSuffix(name: string) {
  return name + TYPEAHEAD_TEXT_SUFFIX;
}

export function hasTypeaheadTextSuffix(name: string) {
  return TYPEAHEAD_TEXT_SUFFIX_REGEX.test(name);
}

export function removeTypeaheadTextSuffix(name: string) {
  return name.replace(TYPEAHEAD_TEXT_SUFFIX_REGEX, '');
}
