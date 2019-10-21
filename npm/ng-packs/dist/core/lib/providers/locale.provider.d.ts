import { Provider } from '@angular/core';
import { LocalizationService } from '../services/localization.service';
export declare class LocaleId extends String {
    private localizationService;
    constructor(localizationService: LocalizationService);
    toString(): string;
    valueOf(): string;
}
export declare const LocaleProvider: Provider;
