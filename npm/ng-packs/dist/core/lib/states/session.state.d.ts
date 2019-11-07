import { StateContext } from '@ngxs/store';
import { SetLanguage, SetTenant } from '../actions/session.actions';
import { ABP, Session } from '../models';
import { LocalizationService } from '../services/localization.service';
export declare class SessionState {
    private localizationService;
    static getLanguage({ language }: Session.State): string;
    static getTenant({ tenant }: Session.State): ABP.BasicItem;
    constructor(localizationService: LocalizationService);
    setLanguage({ patchState, dispatch }: StateContext<Session.State>, { payload }: SetLanguage): import("rxjs").Observable<void>;
    setTenant({ patchState }: StateContext<Session.State>, { payload }: SetTenant): void;
}
