import { StateContext, Store } from '@ngxs/store';
import { Config } from '../models';
import { PatchRouteByName } from '../actions/config.actions';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
export declare class ConfigState {
    private appConfigurationService;
    private store;
    static getAll(state: Config.State): Config.State;
    static getOne(key: string): (state: Config.State) => any;
    static getDeep(keys: string[] | string): (state: Config.State) => any;
    static getApiUrl(key?: string): (state: Config.State) => string;
    static getSetting(key: string): (state: Config.State) => any;
    static getGrantedPolicy(condition?: string): (state: Config.State) => boolean;
    static getCopy(key: string, ...interpolateParams: string[]): (state: Config.State) => any;
    constructor(appConfigurationService: ApplicationConfigurationService, store: Store);
    addData({ patchState, dispatch }: StateContext<Config.State>): import("rxjs").Observable<any>;
    patchRoute({ patchState, getState }: StateContext<Config.State>, { name, newValue }: PatchRouteByName): Config.State;
}
