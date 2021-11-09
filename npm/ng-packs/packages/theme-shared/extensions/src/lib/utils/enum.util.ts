import { ABP, LocalizationService } from '@abp/ng.core';
import { merge, Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { EXTRA_PROPERTIES_KEY } from '../constants/extra-properties';
import { ObjectExtensions } from '../models/object-extensions';
import { PropCallback } from '../models/props';

export function createEnum(members: ObjectExtensions.ExtensionEnumFieldDto[]) {
  const enumObject: any = {};

  members.forEach(({ name = '', value }) => {
    enumObject[(enumObject[name] = value as any)] = name;
  });

  return enumObject;
}

export function createEnumValueResolver<T = any>(
  enumType: string,
  lookupEnum: ObjectExtensions.ExtensionEnumDto,
  propName: string,
): PropCallback<T, Observable<string>> {
  return data => {
    const value = (data.record as any)[EXTRA_PROPERTIES_KEY][propName];
    const key = lookupEnum.transformed[value];
    const l10n = data.getInjected(LocalizationService);
    const localizeEnum = createEnumLocalizer(l10n, enumType, lookupEnum);

    return createLocalizationStream(l10n, localizeEnum(key));
  };
}

export function createEnumOptions<T = any>(
  enumType: string,
  lookupEnum: ObjectExtensions.ExtensionEnumDto,
): PropCallback<T, Observable<ABP.Option<any>[]>> {
  return data => {
    const l10n = data.getInjected(LocalizationService);
    const localizeEnum = createEnumLocalizer(l10n, enumType, lookupEnum);

    return createLocalizationStream(
      l10n,
      lookupEnum.fields.map(({ name = '', value }) => ({
        key: localizeEnum(name),
        value,
      })),
    );
  };
}

function createLocalizationStream(l10n: LocalizationService, mapTarget: any) {
  return merge(of(null), l10n.languageChange$).pipe(map(() => mapTarget));
}

function createEnumLocalizer(
  l10n: LocalizationService,
  enumType: string,
  lookupEnum: ObjectExtensions.ExtensionEnumDto,
): (key: string) => string {
  const resource = lookupEnum.localizationResource;
  const shortType = getShortEnumType(enumType);

  return key =>
    l10n.localizeWithFallbackSync(
      [resource || ''],
      ['Enum:' + shortType + '.' + key, shortType + '.' + key, key],
      key,
    );
}

function getShortEnumType(enumType: string) {
  return enumType.split('.').pop();
}
