import { ApplicationConfiguration } from '../models/application-configuration';
import { ConfigStateService } from '../services';

export function getShortDateFormat(configStateService: ConfigStateService) {
  const dateTimeFormat = configStateService.getDeep(
    'localization.currentCulture.dateTimeFormat',
  ) as ApplicationConfiguration.DateTimeFormat;

  return dateTimeFormat.shortDatePattern;
}

export function getShortTimeFormat(configStateService: ConfigStateService) {
  const dateTimeFormat = configStateService.getDeep(
    'localization.currentCulture.dateTimeFormat',
  ) as ApplicationConfiguration.DateTimeFormat;

  return dateTimeFormat.shortTimePattern.replace('tt', 'a');
}

export function getShortDateShortTimeFormat(configStateService: ConfigStateService) {
  const dateTimeFormat = configStateService.getDeep(
    'localization.currentCulture.dateTimeFormat',
  ) as ApplicationConfiguration.DateTimeFormat;

  return `${dateTimeFormat.shortDatePattern} ${dateTimeFormat.shortTimePattern.replace('tt', 'a')}`;
}
