/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { LOCALE_ID } from '@angular/core';
import localesMapping from '../constants/different-locales';
import { LocalizationService } from '../services/localization.service';
export class LocaleId extends String {
    /**
     * @param {?} localizationService
     */
    constructor(localizationService) {
        super();
        this.localizationService = localizationService;
    }
    /**
     * @return {?}
     */
    toString() {
        const { currentLang } = this.localizationService;
        return localesMapping[currentLang] || currentLang;
    }
    /**
     * @return {?}
     */
    valueOf() {
        return this.toString();
    }
}
if (false) {
    /**
     * @type {?}
     * @private
     */
    LocaleId.prototype.localizationService;
}
/** @type {?} */
export const LocaleProvider = {
    provide: LOCALE_ID,
    useClass: LocaleId,
    deps: [LocalizationService],
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxlLnByb3ZpZGVyLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3Byb3ZpZGVycy9sb2NhbGUucHJvdmlkZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVksTUFBTSxlQUFlLENBQUM7QUFDcEQsT0FBTyxjQUFjLE1BQU0sZ0NBQWdDLENBQUM7QUFDNUQsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sa0NBQWtDLENBQUM7QUFFdkUsTUFBTSxPQUFPLFFBQVMsU0FBUSxNQUFNOzs7O0lBQ2xDLFlBQW9CLG1CQUF3QztRQUMxRCxLQUFLLEVBQUUsQ0FBQztRQURVLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7SUFFNUQsQ0FBQzs7OztJQUVELFFBQVE7Y0FDQSxFQUFFLFdBQVcsRUFBRSxHQUFHLElBQUksQ0FBQyxtQkFBbUI7UUFDaEQsT0FBTyxjQUFjLENBQUMsV0FBVyxDQUFDLElBQUksV0FBVyxDQUFDO0lBQ3BELENBQUM7Ozs7SUFFRCxPQUFPO1FBQ0wsT0FBTyxJQUFJLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDekIsQ0FBQztDQUNGOzs7Ozs7SUFaYSx1Q0FBZ0Q7OztBQWM5RCxNQUFNLE9BQU8sY0FBYyxHQUFhO0lBQ3RDLE9BQU8sRUFBRSxTQUFTO0lBQ2xCLFFBQVEsRUFBRSxRQUFRO0lBQ2xCLElBQUksRUFBRSxDQUFDLG1CQUFtQixDQUFDO0NBQzVCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTE9DQUxFX0lELCBQcm92aWRlciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IGxvY2FsZXNNYXBwaW5nIGZyb20gJy4uL2NvbnN0YW50cy9kaWZmZXJlbnQtbG9jYWxlcyc7XG5pbXBvcnQgeyBMb2NhbGl6YXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UnO1xuXG5leHBvcnQgY2xhc3MgTG9jYWxlSWQgZXh0ZW5kcyBTdHJpbmcge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGxvY2FsaXphdGlvblNlcnZpY2U6IExvY2FsaXphdGlvblNlcnZpY2UpIHtcbiAgICBzdXBlcigpO1xuICB9XG5cbiAgdG9TdHJpbmcoKTogc3RyaW5nIHtcbiAgICBjb25zdCB7IGN1cnJlbnRMYW5nIH0gPSB0aGlzLmxvY2FsaXphdGlvblNlcnZpY2U7XG4gICAgcmV0dXJuIGxvY2FsZXNNYXBwaW5nW2N1cnJlbnRMYW5nXSB8fCBjdXJyZW50TGFuZztcbiAgfVxuXG4gIHZhbHVlT2YoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy50b1N0cmluZygpO1xuICB9XG59XG5cbmV4cG9ydCBjb25zdCBMb2NhbGVQcm92aWRlcjogUHJvdmlkZXIgPSB7XG4gIHByb3ZpZGU6IExPQ0FMRV9JRCxcbiAgdXNlQ2xhc3M6IExvY2FsZUlkLFxuICBkZXBzOiBbTG9jYWxpemF0aW9uU2VydmljZV0sXG59O1xuIl19