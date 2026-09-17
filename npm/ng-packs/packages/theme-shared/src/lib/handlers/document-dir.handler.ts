import { getLocaleDirection, LocalizationService } from '@abp/ng.core';
import { Injectable, Injector, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { LocaleDirection } from '../models/common';
import { DOCUMENT } from '@angular/common';

@Injectable()
export class DocumentDirHandlerService {
  protected injector = inject(Injector);

  private dir = new BehaviorSubject<LocaleDirection>('ltr');
  dir$ = this.dir.asObservable();
  constructor() {
    this.listenToLanguageChanges();
  }

  private listenToLanguageChanges() {
    const l10n = this.injector.get(LocalizationService);
    // will always listen, no need to unsubscribe
    l10n.currentLang$.pipe(map(locale => getLocaleDirection(locale))).subscribe(dir => {
      this.dir.next(dir);
      this.setBodyDir(dir);
    });
  }

  private setBodyDir(dir: LocaleDirection) {
    this.injector.get(DOCUMENT).body.dir = dir;
    this.injector.get(DOCUMENT).dir = dir;
  }
}
