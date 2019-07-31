import { Actions } from '@ngxs/store';
import { LoaderStart, LoaderStop } from '@abp/ng.core';
export declare class LoaderBarComponent {
    private actions;
    containerClass: string;
    progressClass: string;
    isLoading: boolean;
    filter: (action: LoaderStart | LoaderStop) => boolean;
    progressLevel: number;
    interval: any;
    constructor(actions: Actions);
    startLoading(): void;
    stopLoading(): void;
}
