import { StartLoader, StopLoader } from '@abp/ng.core';
import { ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Actions } from '@ngxs/store';
import { Subscription } from 'rxjs';
export declare class LoaderBarComponent implements OnDestroy {
    private actions;
    private router;
    private cdRef;
    readonly boxShadow: string;
    constructor(actions: Actions, router: Router, cdRef: ChangeDetectorRef);
    containerClass: string;
    color: string;
    isLoading: boolean;
    progressLevel: number;
    interval: Subscription;
    timer: Subscription;
    filter: (action: StartLoader | StopLoader) => boolean;
    ngOnDestroy(): void;
    startLoading(): void;
    stopLoading(): void;
}
