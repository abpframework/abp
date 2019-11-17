import { StartLoader, StopLoader } from '@abp/ng.core';
import { ChangeDetectorRef, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Actions } from '@ngxs/store';
import { Subscription } from 'rxjs';
export declare class LoaderBarComponent implements OnDestroy, OnInit {
    private actions;
    private router;
    private cdRef;
    containerClass: string;
    color: string;
    isLoading: boolean;
    progressLevel: number;
    interval: Subscription;
    timer: Subscription;
    intervalPeriod: number;
    stopDelay: number;
    filter: (action: StartLoader | StopLoader) => boolean;
    readonly boxShadow: string;
    constructor(actions: Actions, router: Router, cdRef: ChangeDetectorRef);
    ngOnInit(): void;
    ngOnDestroy(): void;
    startLoading(): void;
    stopLoading(): void;
}
