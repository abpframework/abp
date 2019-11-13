import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { Confirmation } from '../models/confirmation';
import { MessageService } from 'primeng/components/common/messageservice';
import { fromEvent, Observable, Subject } from 'rxjs';
import { takeUntil, debounceTime, filter } from 'rxjs/operators';
import { Toaster } from '../models/toaster';

@Injectable({ providedIn: 'root' })
export class ConfirmationService extends AbstractToaster<Confirmation.Options> {
  key = 'abpConfirmation';

  sticky = true;

  destroy$ = new Subject();

  constructor(protected messageService: MessageService) {
    super(messageService);
  }

  show(
    message: string,
    title: string,
    severity: Toaster.Severity,
    options?: Confirmation.Options,
  ): Observable<Toaster.Status> {
    this.listenToEscape();
    return super.show(message, title, severity, options);
  }

  clear(status?: Toaster.Status) {
    super.clear(status);

    this.destroy$.next();
  }

  listenToEscape() {
    fromEvent(document, 'keyup')
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(150),
        filter((key: KeyboardEvent) => key && key.key === 'Escape'),
      )
      .subscribe(_ => {
        this.clear();
      });
  }
}
