import { AbstractToaster } from '../abstracts/toaster';
import { Confirmation } from '../models/confirmation';
export declare class ConfirmationService extends AbstractToaster<Confirmation.Options> {
    key: string;
    sticky: boolean;
}
