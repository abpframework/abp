import type { CurrentTenantDto, MultiTenancyInfoDto } from '../multi-tenancy/models';
import type { ObjectExtensionsDto } from './object-extending/models';
import type { LanguageInfo } from '../../../localization/models';
import type { NameValue } from '../../../models';

export interface ApplicationAuthConfigurationDto {
  grantedPolicies: Record<string, boolean>;
}

export interface ApplicationConfigurationDto {
  localization: ApplicationLocalizationConfigurationDto;
  auth: ApplicationAuthConfigurationDto;
  setting: ApplicationSettingConfigurationDto;
  currentUser: CurrentUserDto;
  features: ApplicationFeatureConfigurationDto;
  globalFeatures: ApplicationGlobalFeatureConfigurationDto;
  multiTenancy: MultiTenancyInfoDto;
  currentTenant: CurrentTenantDto;
  timing: TimingDto;
  clock: ClockDto;
  objectExtensions: ObjectExtensionsDto;
  extraProperties: Record<string, object>;
}

export interface ApplicationConfigurationRequestOptions {
  includeLocalizationResources: boolean;
}

export interface ApplicationFeatureConfigurationDto {
  values: Record<string, string>;
}

export interface ApplicationGlobalFeatureConfigurationDto {
  enabledFeatures: string[];
}

export interface ApplicationLocalizationConfigurationDto {
  values: Record<string, Record<string, string>>;
  resources: Record<string, ApplicationLocalizationResourceDto>;
  languages: LanguageInfo[];
  currentCulture: CurrentCultureDto;
  defaultResourceName?: string;
  languagesMap: Record<string, NameValue[]>;
  languageFilesMap: Record<string, NameValue[]>;
}

export interface ApplicationLocalizationDto {
  resources: Record<string, ApplicationLocalizationResourceDto>;
}

export interface ApplicationLocalizationRequestDto {
  cultureName: string;
  onlyDynamics: boolean;
}

export interface ApplicationLocalizationResourceDto {
  texts: Record<string, string>;
  baseResources: string[];
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
  impersonatorUserId?: string;
  impersonatorTenantId?: string;
  impersonatorUserName?: string;
  impersonatorTenantName?: string;
  userName?: string;
  name?: string;
  surName?: string;
  email?: string;
  emailVerified: boolean;
  phoneNumber?: string;
  phoneNumberVerified: boolean;
  roles: string[];
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
