import { MessageService } from 'primeng/components/common/messageservice';
import { Observable, Subject } from 'rxjs';
import { Toaster } from '../models/toaster';
import { Config } from '@abp/ng.core';
export declare abstract class AbstractToaster<T = Toaster.Options> {
    protected messageService: MessageService;
    status$: Subject<Toaster.Status>;
    key: string;
    sticky: boolean;
    constructor(messageService: MessageService);
    info(message: Config.LocalizationParam, title: Config.LocalizationParam, options?: T): Observable<Toaster.Status>;
    success(message: Config.LocalizationParam, title: Config.LocalizationParam, options?: T): Observable<Toaster.Status>;
    warn(message: Config.LocalizationParam, title: Config.LocalizationParam, options?: T): Observable<Toaster.Status>;
    error(message: Config.LocalizationParam, title: Config.LocalizationParam, options?: T): Observable<Toaster.Status>;
    protected show(message: Config.LocalizationParam, title: Config.LocalizationParam, severity: Toaster.Severity, options?: T): Observable<Toaster.Status>;
    clear(status?: Toaster.Status): void;
}
