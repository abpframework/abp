export namespace ApplicationConfiguration {
  export interface Response {
    localization: Localization;
    auth: Auth;
    setting: Setting;
    currentUser: CurrentUser;
    features: Features;
  }

  export interface Localization {
    values: LocalizationValue;
    languages: Language[];
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

  export interface Auth {
    policies: Policy;
    grantedPolicies: Policy;
  }

  export interface Policy {
    [key: string]: boolean;
  }

  export interface Setting {
    values: { [key: string]: 'Abp.Localization.DefaultLanguage' };
  }

  export interface CurrentUser {
    isAuthenticated: boolean;
    id: string;
    tenantId: string;
    userName: string;
  }

  export interface Features {
    values: Setting;
  }
}
