import { ConfirmationService } from '../../services/confirmation.service';
import { Toaster } from '../../models/toaster';
export declare class ConfirmationComponent {
    private confirmationService;
    confirm: Toaster.Status;
    reject: Toaster.Status;
    dismiss: Toaster.Status;
    constructor(confirmationService: ConfirmationService);
    close(status: Toaster.Status): void;
}
