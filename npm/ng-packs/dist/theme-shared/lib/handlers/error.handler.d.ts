import { Actions, Store } from '@ngxs/store';
import { ConfirmationService } from '../services/confirmation.service';
export declare class ErrorHandler {
    private actions;
    private store;
    private confirmationService;
    constructor(actions: Actions, store: Store, confirmationService: ConfirmationService);
    private showError;
    private navigateToLogin;
}
