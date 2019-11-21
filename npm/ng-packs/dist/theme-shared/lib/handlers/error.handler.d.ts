import { ApplicationRef, ComponentFactoryResolver, Injector, NgZone, RendererFactory2 } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, Store } from '@ngxs/store';
import { ErrorComponent } from '../components/error/error.component';
import { ConfirmationService } from '../services/confirmation.service';
export declare const DEFAULT_ERROR_MESSAGES: {
    defaultError: {
        title: string;
        details: string;
    };
    defaultError401: {
        title: string;
        details: string;
    };
    defaultError403: {
        title: string;
        details: string;
    };
    defaultError404: {
        title: string;
        details: string;
    };
    defaultError500: {
        title: string;
        details: {
            key: string;
            defaultValue: string;
        };
    };
    defaultErrorUnknown: {
        title: string;
        details: {
            key: string;
            defaultValue: string;
        };
    };
};
export declare class ErrorHandler {
    private actions;
    private router;
    private ngZone;
    private store;
    private confirmationService;
    private appRef;
    private cfRes;
    private rendererFactory;
    private injector;
    constructor(actions: Actions, router: Router, ngZone: NgZone, store: Store, confirmationService: ConfirmationService, appRef: ApplicationRef, cfRes: ComponentFactoryResolver, rendererFactory: RendererFactory2, injector: Injector);
    private showError;
    private navigateToLogin;
    createErrorComponent(instance: Partial<ErrorComponent>): void;
}
