import { StateContext, Store } from '@ngxs/store';
import { PatchRouteByName } from '../actions/config.actions';
import { ABP } from '../models/common';
import { Config } from '../models/config';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
export declare class ConfigState {
    private appConfigurationService;
    private store;
    static getAll(state: Config.State): Config.State;
    static getApplicationInfo(state: Config.State): Config.Application;
    static getOne(key: string): (state: Config.State) => any;
    static getDeep(keys: string[] | string): (state: Config.State) => any;
    static getRoute(path?: string, name?: string): (state: Config.State) => ABP.FullRoute;
    static getApiUrl(key?: string): (state: Config.State) => string;
    static getSetting(key: string): (state: Config.State) => string;
    static getSettings(keyword?: string): (state: Config.State) => {};
    static getGrantedPolicy(key: string): (state: Config.State) => boolean;
    static getLocalization(key: string | Config.LocalizationWithDefault, ...interpolateParams: string[]): (state: Config.State) => any;
    constructor(appConfigurationService: ApplicationConfigurationService, store: Store);
    addData({ patchState, dispatch }: StateContext<Config.State>): import("rxjs").Observable<any>;
    patchRoute({ patchState, getState }: StateContext<Config.State>, { name, newValue }: PatchRouteByName): Config.State;
}
