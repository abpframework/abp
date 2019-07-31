import { ApplicationRef, ComponentFactoryResolver, RendererFactory2, Injector } from '@angular/core';
import { Actions, Store } from '@ngxs/store';
import { ConfirmationService } from '../services/confirmation.service';
export declare class ErrorHandler {
    private actions;
    private store;
    private confirmationService;
    private appRef;
    private cfRes;
    private rendererFactory;
    private injector;
    constructor(actions: Actions, store: Store, confirmationService: ConfirmationService, appRef: ApplicationRef, cfRes: ComponentFactoryResolver, rendererFactory: RendererFactory2, injector: Injector);
    private showError;
    private navigateToLogin;
    private show500Component;
}
