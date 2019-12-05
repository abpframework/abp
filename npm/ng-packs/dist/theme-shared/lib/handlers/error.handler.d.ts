import { ApplicationRef, ComponentFactoryResolver, Injector, RendererFactory2, ComponentRef } from '@angular/core';
import { Actions, Store } from '@ngxs/store';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { HttpErrorConfig, ErrorScreenErrorCodes } from '../models/common';
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
        details: string;
    };
};
export declare class ErrorHandler {
    private actions;
    private store;
    private confirmationService;
    private appRef;
    private cfRes;
    private rendererFactory;
    private injector;
    private httpErrorConfig;
    componentRef: ComponentRef<HttpErrorWrapperComponent>;
    constructor(actions: Actions, store: Store, confirmationService: ConfirmationService, appRef: ApplicationRef, cfRes: ComponentFactoryResolver, rendererFactory: RendererFactory2, injector: Injector, httpErrorConfig: HttpErrorConfig);
    private show401Page;
    private show404Page;
    private showError;
    private navigateToLogin;
    createErrorComponent(instance: Partial<HttpErrorWrapperComponent>): void;
    canCreateCustomError(status: ErrorScreenErrorCodes): boolean;
}
