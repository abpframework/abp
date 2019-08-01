import { ApplicationRef, ComponentFactoryResolver, Injector, RendererFactory2 } from '@angular/core';
import { Actions, Store } from '@ngxs/store';
import { ErrorComponent } from '../components/errors/error.component';
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
    createErrorComponent(instance: Partial<ErrorComponent>): void;
}
