import { Router } from '@angular/router';
import { Actions, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
export declare class LocalizationService {
    private store;
    private router;
    private actions;
    readonly currentLang: string;
    constructor(store: Store, router: Router, actions: Actions, otherInstance: LocalizationService);
    private setRouteReuse;
    registerLocale(locale: string): Promise<void>;
    get(keys: string, ...interpolateParams: string[]): Observable<string>;
    instant(keys: string, ...interpolateParams: string[]): string;
}
