import { MessageService } from 'primeng/components/common/messageservice';
import { Observable, Subject } from 'rxjs';
import { Toaster } from '../models/toaster';
export declare class AbstractToasterClass<T = Toaster.Options> {
    protected messageService: MessageService;
    protected status$: Subject<Toaster.Status>;
    protected key: string;
    protected sticky: boolean;
    constructor(messageService: MessageService);
    info(message: string, title: string, options?: T): Observable<Toaster.Status>;
    success(message: string, title: string, options?: T): Observable<Toaster.Status>;
    warn(message: string, title: string, options?: T): Observable<Toaster.Status>;
    error(message: string, title: string, options?: T): Observable<Toaster.Status>;
    protected show(message: string, title: string, severity: Toaster.Severity, options?: T): Observable<Toaster.Status>;
    clear(status?: Toaster.Status): void;
}
