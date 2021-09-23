import { DateTimeFormatDto } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';
import { ConfigStateService } from '../services';

export function getShortDateFormat(configStateService: ConfigStateService) {
  const dateTimeFormat = configStateService.getDeep(
    'localization.currentCulture.dateTimeFormat',
  ) as DateTimeFormatDto;

  return dateTimeFormat.shortDatePattern;
}

export function getShortTimeFormat(configStateService: ConfigStateService) {
  const dateTimeFormat = configStateService.getDeep(
    'localization.currentCulture.dateTimeFormat',
  ) as DateTimeFormatDto;

  return dateTimeFormat.shortTimePattern.replace('tt', 'a');
}

export function getShortDateShortTimeFormat(configStateService: ConfigStateService) {
  const dateTimeFormat = configStateService.getDeep(
    'localization.currentCulture.dateTimeFormat',
  ) as DateTimeFormatDto;

  return `${dateTimeFormat.shortDatePattern} ${dateTimeFormat.shortTimePattern.replace('tt', 'a')}`;
}
