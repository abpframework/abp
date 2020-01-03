import { MessageService } from 'primeng/components/common/messageservice';
import { Observable, Subject } from 'rxjs';
import { Toaster } from '../models/toaster';
import { Config } from '@abp/ng.core';

export abstract class AbstractToaster<T = Toaster.Options> {
  status$: Subject<Toaster.Status>;

  key = 'abpToast';

  sticky = false;

  constructor(protected messageService: MessageService) {}

  info(message: Config.LocalizationParam, title: Config.LocalizationParam, options?: T): Observable<Toaster.Status> {
    return this.show(message, title, 'info', options);
  }

  success(message: Config.LocalizationParam, title: Config.LocalizationParam, options?: T): Observable<Toaster.Status> {
    return this.show(message, title, 'success', options);
  }

  warn(message: Config.LocalizationParam, title: Config.LocalizationParam, options?: T): Observable<Toaster.Status> {
    return this.show(message, title, 'warn', options);
  }

  error(message: Config.LocalizationParam, title: Config.LocalizationParam, options?: T): Observable<Toaster.Status> {
    return this.show(message, title, 'error', options);
  }

  protected show(
    message: Config.LocalizationParam,
    title: Config.LocalizationParam,
    severity: Toaster.Severity,
    options?: T,
  ): Observable<Toaster.Status> {
    this.messageService.clear(this.key);

    this.messageService.add({
      severity,
      detail: message || '',
      summary: title || '',
      ...options,
      key: this.key,
      ...(typeof (options || ({} as any)).sticky === 'undefined' && { sticky: this.sticky }),
    });
    this.status$ = new Subject<Toaster.Status>();
    return this.status$;
  }

  clear(status?: Toaster.Status) {
    this.messageService.clear(this.key);
    this.status$.next(status || Toaster.Status.dismiss);
    this.status$.complete();
  }
}
