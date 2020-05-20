import { ABP } from './common';

export namespace ApplicationConfiguration {
  export interface Response {
    localization: Localization;
    auth: Auth;
    setting: Value;
    currentUser: CurrentUser;
    features: Value;
  }

  export interface Localization {
    currentCulture: CurrentCulture;
    defaultResourceName: string;
    languages: Language[];
    values: LocalizationValue;
  }

  export interface LocalizationValue {
    [key: string]: { [key: string]: string };
  }

  export interface Language {
    cultureName: string;
    uiCultureName: string;
    displayName: string;
    flagIcon: string;
  }

  export interface CurrentCulture {
    cultureName: string;
    dateTimeFormat: DateTimeFormat;
    displayName: string;
    englishName: string;
    isRightToLeft: boolean;
    name: string;
    nativeName: string;
    threeLetterIsoLanguageName: string;
    twoLetterIsoLanguageName: string;
  }

  export interface DateTimeFormat {
    calendarAlgorithmType: string;
    dateSeparator: string;
    fullDateTimePattern: string;
    longTimePattern: string;
    shortDatePattern: string;
    shortTimePattern: string;
  }

  export interface Auth {
    policies: Policy;
    grantedPolicies: Policy;
  }

  export interface Policy {
    [key: string]: boolean;
  }

  export interface Value {
    values: ABP.Dictionary<string>;
  }

  export interface CurrentUser {
    isAuthenticated: boolean;
    id: string;
    tenantId: string;
    userName: string;
    email: string;
  }
}
