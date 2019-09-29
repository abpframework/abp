import { StartLoader, StopLoader } from '@abp/ng.core';
import { ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Actions } from '@ngxs/store';
import { Subscription } from 'rxjs';
export declare class LoaderBarComponent implements OnDestroy {
    private actions;
    private router;
    private cdRef;
    containerClass: string;
    color: string;
    isLoading: boolean;
    filter: (action: StartLoader | StopLoader) => boolean;
    progressLevel: number;
    interval: Subscription;
    timer: Subscription;
    readonly boxShadow: string;
    constructor(actions: Actions, router: Router, cdRef: ChangeDetectorRef);
    ngOnDestroy(): void;
    startLoading(): void;
    stopLoading(): void;
}
