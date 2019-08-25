import { StateContext } from '@ngxs/store';
import { SetLanguage, SetTenant } from '../actions/session.actions';
import { ABP, Session } from '../models';
export declare class SessionState {
    static getLanguage({ language }: Session.State): string;
    static getTenant({ tenant }: Session.State): ABP.BasicItem;
    constructor();
    setLanguage({ patchState }: StateContext<Session.State>, { payload }: SetLanguage): void;
    setTenantId({ patchState }: StateContext<Session.State>, { payload }: SetTenant): void;
}
