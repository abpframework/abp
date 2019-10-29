import { OnDestroy, Type } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { Config } from '../models/config';
export declare class DynamicLayoutComponent implements OnDestroy {
    private router;
    private route;
    private store;
    requirements$: Observable<Config.Requirements>;
    layout: Type<any>;
    constructor(router: Router, route: ActivatedRoute, store: Store);
    ngOnDestroy(): void;
}
