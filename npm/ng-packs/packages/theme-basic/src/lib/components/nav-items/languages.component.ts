import { ConfigStateService, LanguageInfo, SessionStateService } from '@abp/ng.core';
import { Component, inject } from '@angular/core';
import { DOCUMENT, AsyncPipe } from '@angular/common';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'abp-languages',
  template: `
    @if (((dropdownLanguages$ | async)?.length || 0) > 0) {
      <div class="dropdown" ngbDropdown #languageDropdown="ngbDropdown" display="static">
        <a
          ngbDropdownToggle
          class="nav-link"
          href="javascript:void(0)"
          role="button"
          id="dropdownMenuLink"
          data-toggle="dropdown"
          aria-haspopup="true"
          aria-expanded="false"
        >
          {{ defaultLanguage$ | async }}
        </a>
        <div
          class="dropdown-menu dropdown-menu-end border-0 shadow-sm"
          aria-labelledby="dropdownMenuLink"
          [class.d-block]="smallScreen && languageDropdown.isOpen()"
        >
          @for (lang of dropdownLanguages$ | async; track $index) {
            <a
              href="javascript:void(0)"
              class="dropdown-item"
              (click)="onChangeLang(lang.cultureName || '')"
              >{{ lang?.displayName }}</a
            >
          }
        </div>
      </div>
    }
  `,
  imports: [AsyncPipe, NgbDropdownModule],
})
export class LanguagesComponent {
  private sessionState = inject(SessionStateService);
  private configState = inject(ConfigStateService);
  document = inject(DOCUMENT);

  get smallScreen(): boolean {
    return this.document.defaultView.innerWidth < 992;
  }

  languages$: Observable<LanguageInfo[]> = this.configState.getDeep$('localization.languages');

  get defaultLanguage$(): Observable<string> {
    return this.languages$.pipe(
      map(
        languages =>
          languages?.find(lang => lang.cultureName === this.selectedLangCulture)?.displayName || '',
      ),
    );
  }

  get dropdownLanguages$(): Observable<LanguageInfo[]> {
    return this.languages$.pipe(
      map(
        languages => languages?.filter(lang => lang.cultureName !== this.selectedLangCulture) || [],
      ),
    );
  }

  get selectedLangCulture(): string {
    return this.sessionState.getLanguage();
  }

  onChangeLang(cultureName: string) {
    this.sessionState.setLanguage(cultureName);
  }
}
