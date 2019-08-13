import { StateContext } from '@ngxs/store';
import { SessionSetLanguage } from '../actions/session.actions';
import { Session } from '../models/session';
export declare class SessionState {
    static getLanguage({ language }: Session.State): string;
    constructor();
    sessionSetLanguage({ patchState }: StateContext<Session.State>, { payload }: SessionSetLanguage): void;
}
