export declare namespace ApplicationConfiguration {
    interface Response {
        localization: Localization;
        auth: Auth;
        setting: Setting;
        currentUser: CurrentUser;
        features: Features;
    }
    interface Localization {
        values: LocalizationValue;
        languages: Language[];
    }
    interface LocalizationValue {
        [key: string]: {
            [key: string]: string;
        };
    }
    interface Language {
        cultureName: string;
        uiCultureName: string;
        displayName: string;
        flagIcon: string;
    }
    interface Auth {
        policies: Policy;
        grantedPolicies: Policy;
    }
    interface Policy {
        [key: string]: boolean;
    }
    interface Setting {
        values: {
            [key: string]: 'Abp.Localization.DefaultLanguage';
        };
    }
    interface CurrentUser {
        isAuthenticated: boolean;
        id: string;
        tenantId: string;
        userName: string;
    }
    interface Features {
        values: Setting;
    }
}
