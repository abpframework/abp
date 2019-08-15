import { OnInit } from '@angular/core';
import { Actions } from '@ngxs/store';
export declare class ButtonComponent implements OnInit {
    private actions;
    buttonClass: string;
    buttonType: string;
    iconClass: string;
    loading: boolean;
    disabled: boolean;
    requestType: string | string[];
    requestURLContainSearchValue: string;
    readonly icon: string;
    constructor(actions: Actions);
    ngOnInit(): void;
}
