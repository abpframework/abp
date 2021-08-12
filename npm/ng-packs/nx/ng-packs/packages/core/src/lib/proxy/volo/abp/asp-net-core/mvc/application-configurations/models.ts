import type { LanguageInfo } from '../../../localization/models';
import type { NameValue } from '../../../models';
import type { CurrentTenantDto, MultiTenancyInfoDto } from '../multi-tenancy/models';
import type { ObjectExtensionsDto } from './object-extending/models';

export interface ApplicationAuthConfigurationDto {
  policies: Record<string, boolean>;
  grantedPolicies: Record<string, boolean>;
}

export interface ApplicationConfigurationDto {
  localization: ApplicationLocalizationConfigurationDto;
  auth: ApplicationAuthConfigurationDto;
  setting: ApplicationSettingConfigurationDto;
  currentUser: CurrentUserDto;
  features: ApplicationFeatureConfigurationDto;
  multiTenancy: MultiTenancyInfoDto;
  currentTenant: CurrentTenantDto;
  timing: TimingDto;
  clock: ClockDto;
  objectExtensions: ObjectExtensionsDto;
}

export interface ApplicationFeatureConfigurationDto {
  values: Record<string, string>;
}

export interface ApplicationLocalizationConfigurationDto {
  values: Record<string, Record<string, string>>;
  languages: LanguageInfo[];
  currentCulture: CurrentCultureDto;
  defaultResourceName?: string;
  languagesMap: Record<string, NameValue[]>;
  languageFilesMap: Record<string, NameValue[]>;
}

export interface ApplicationSettingConfigurationDto {
  values: Record<string, string>;
}

export interface ClockDto {
  kind?: string;
}

export interface CurrentCultureDto {
  displayName?: string;
  englishName?: string;
  threeLetterIsoLanguageName?: string;
  twoLetterIsoLanguageName?: string;
  isRightToLeft: boolean;
  cultureName?: string;
  name?: string;
  nativeName?: string;
  dateTimeFormat: DateTimeFormatDto;
}

export interface CurrentUserDto {
  isAuthenticated: boolean;
  id?: string;
  tenantId?: string;
  userName?: string;
  name?: string;
  surName?: string;
  email?: string;
  emailVerified: boolean;
  phoneNumber?: string;
  phoneNumberVerified: boolean;
  roles: string[];
  impersonatorUserId?: string;
  impersonatorTenantId?: string;
}

export interface DateTimeFormatDto {
  calendarAlgorithmType?: string;
  dateTimeFormatLong?: string;
  shortDatePattern?: string;
  fullDateTimePattern?: string;
  dateSeparator?: string;
  shortTimePattern?: string;
  longTimePattern?: string;
}

export interface IanaTimeZone {
  timeZoneName?: string;
}

export interface TimeZone {
  iana: IanaTimeZone;
  windows: WindowsTimeZone;
}

export interface TimingDto {
  timeZone: TimeZone;
}

export interface WindowsTimeZone {
  timeZoneId?: string;
}
