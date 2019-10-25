import { ABP } from './common';
export declare namespace ApplicationConfiguration {
    interface Response {
        localization: Localization;
        auth: Auth;
        setting: Value;
        currentUser: CurrentUser;
        features: Value;
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
    interface Value {
        values: ABP.Dictionary<string>;
    }
    interface CurrentUser {
        isAuthenticated: boolean;
        id: string;
        tenantId: string;
        userName: string;
    }
}
