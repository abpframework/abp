import { ABP } from './common';

export namespace ApplicationConfiguration {
  /**
   * @deprecated Use the ApplicationConfigurationDto interface instead. To be deleted in v5.0.
   */
  export interface Response {
    localization: Localization;
    auth: Auth;
    setting: Value;
    currentUser: CurrentUser;
    currentTenant: CurrentTenant;
    features: Value;
  }

  /**
   * @deprecated Use the ApplicationLocalizationConfigurationDto interface instead. To be deleted in v5.0.
   */
  export interface Localization {
    currentCulture: CurrentCulture;
    defaultResourceName: string;
    languages: Language[];
    values: LocalizationValue;
  }

  /**
   * @deprecated Use the Record<string, Record<string, string>> type instead. To be deleted in v5.0.
   */
  export interface LocalizationValue {
    [key: string]: { [key: string]: string };
  }

  /**
   * @deprecated Use the LanguageInfo interface instead. To be deleted in v5.0.
   */
  export interface Language {
    cultureName: string;
    uiCultureName: string;
    displayName: string;
    flagIcon: string;
  }

  /**
   * @deprecated Use the CurrentCultureDto interface instead. To be deleted in v5.0.
   */
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

  /**
   * @deprecated Use the DateTimeFormatDto interface instead. To be deleted in v5.0.
   */
  export interface DateTimeFormat {
    calendarAlgorithmType: string;
    dateSeparator: string;
    fullDateTimePattern: string;
    longTimePattern: string;
    shortDatePattern: string;
    shortTimePattern: string;
  }

  /**
   * @deprecated Use the ApplicationAuthConfigurationDto interface instead. To be deleted in v5.0.
   */
  export interface Auth {
    policies: Policy;
    grantedPolicies: Policy;
  }

  /**
   * @deprecated Use the Record<string, boolean> type instead. To be deleted in v5.0.
   */
  export interface Policy {
    [key: string]: boolean;
  }

  /**
   * @deprecated To be deleted in v5.0.
   */
  export interface Value {
    values: ABP.Dictionary<string>;
  }

  /**
   * @deprecated Use the CurrentUserDto interface instead. To be deleted in v5.0.
   */
  export interface CurrentUser {
    email: string;
    emailVerified: false;
    id: string;
    isAuthenticated: boolean;
    roles: string[];
    tenantId: string;
    userName: string;
    name: string;
    phoneNumber: string;
    phoneNumberVerified: boolean;
    surName: string;
  }

  /**
   * @deprecated Use the CurrentTenantDto interface instead. To be deleted in v5.0.
   */
  export interface CurrentTenant {
    id: string;
    name: string;
    isAvailable?: boolean;
  }
}
