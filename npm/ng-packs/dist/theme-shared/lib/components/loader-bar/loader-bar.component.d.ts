import { OnDestroy } from '@angular/core';
import { Actions } from '@ngxs/store';
import { LoaderStart, LoaderStop } from '@abp/ng.core';
import { Router } from '@angular/router';
export declare class LoaderBarComponent implements OnDestroy {
    private actions;
    private router;
    containerClass: string;
    progressClass: string;
    isLoading: boolean;
    filter: (action: LoaderStart | LoaderStop) => boolean;
    progressLevel: number;
    interval: any;
    constructor(actions: Actions, router: Router);
    ngOnDestroy(): void;
    startLoading(): void;
    stopLoading(): void;
}
