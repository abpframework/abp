import { StartLoader, StopLoader } from '@abp/ng.core';
import { OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Actions } from '@ngxs/store';
export declare class LoaderBarComponent implements OnDestroy {
    private actions;
    private router;
    containerClass: string;
    progressClass: string;
    isLoading: boolean;
    filter: (action: StartLoader | StopLoader) => boolean;
    progressLevel: number;
    interval: any;
    constructor(actions: Actions, router: Router);
    ngOnDestroy(): void;
    startLoading(): void;
    stopLoading(): void;
}
