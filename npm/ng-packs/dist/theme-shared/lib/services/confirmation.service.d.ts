import { AbstractToaster } from '../abstracts/toaster';
import { Confirmation } from '../models/confirmation';
import { MessageService } from 'primeng/components/common/messageservice';
import { Observable, Subject } from 'rxjs';
import { Toaster } from '../models/toaster';
export declare class ConfirmationService extends AbstractToaster<Confirmation.Options> {
    protected messageService: MessageService;
    key: string;
    sticky: boolean;
    destroy$: Subject<unknown>;
    constructor(messageService: MessageService);
    show(message: string, title: string, severity: Toaster.Severity, options?: Confirmation.Options): Observable<Toaster.Status>;
    clear(status?: Toaster.Status): void;
    listenToEscape(): void;
}
