import { OnDestroy, Type } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { Config } from '../models';
export declare class DynamicLayoutComponent implements OnDestroy {
    private router;
    private store;
    requirements$: Observable<Config.Requirements>;
    layout: Type<any>;
    constructor(router: Router, store: Store);
    ngOnDestroy(): void;
}
