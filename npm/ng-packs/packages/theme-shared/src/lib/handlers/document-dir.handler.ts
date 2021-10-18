import { getLocaleDirection, LocalizationService } from '@abp/ng.core';
import { Injectable, Injector } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { LocaleDirection } from '../models/common';

@Injectable()
export class DocumentDirHandlerService {
  private dir = new BehaviorSubject<LocaleDirection>('ltr');
  dir$ = this.dir.asObservable();
  constructor(protected injector: Injector) {
    this.listenToLanguageChanges();
  }

  private listenToLanguageChanges() {
    const l10n = this.injector.get(LocalizationService);

    // will always listen, no need to unsubscribe
    l10n.languageChange$
      .pipe(
        startWith(l10n.currentLang),
        map(locale => getLocaleDirection(locale)),
      )
      .subscribe(dir => {
        this.dir.next(dir);
        this.setBodyDir(dir);
      });
  }

  private setBodyDir(dir: LocaleDirection) {
    document.body.dir = dir;
    document.dir = dir;
  }
}
