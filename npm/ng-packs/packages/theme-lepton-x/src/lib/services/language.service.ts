import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigStateService, LanguageInfo, SessionStateService } from '@abp/ng.core';
import { filter } from 'rxjs/operators';
import { Language, LanguageService } from '@volo/ngx-lepton-x.core';

@Injectable({
  providedIn: 'root',
})
export class AbpLanguageService {
  languages$: Observable<LanguageInfo[]> = this.configState.getDeep$('localization.languages');

  constructor(
    private configState: ConfigStateService,
    private languageService: LanguageService,
    private sessionState: SessionStateService,
  ) {}

  subscribeLanguage() {
    this.languages$.pipe(filter<LanguageInfo[]>(Boolean)).subscribe(langs => {
      this.languageService.init(langs.map(this.mapLang));
    });

    this.languageService.selectedLanguage$.pipe(filter<LanguageInfo>(Boolean)).subscribe(lang => {
      this.sessionState.setLanguage(lang.cultureName);
    });
  }

  private mapLang = (lang: LanguageInfo): Language => {
    return {
      cultureName: lang.cultureName,
      displayName: lang.displayName,
      selected: this.sessionState.getLanguage() === lang.cultureName,
    };
  };
}
