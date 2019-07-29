import { AbstractToasterClass } from '../abstracts/toaster';
import { Confirmation } from '../models/confirmation';
export declare class ConfirmationService extends AbstractToasterClass<Confirmation.Options> {
    protected key: string;
    protected sticky: boolean;
}
