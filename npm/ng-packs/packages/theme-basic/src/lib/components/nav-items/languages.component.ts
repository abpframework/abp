import { Component, OnInit, Input } from '@angular/core';
import { Store, Select } from '@ngxs/store';
import { SetLanguage, ConfigState, ApplicationConfiguration, SessionState } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import snq from 'snq';

@Component({
  selector: 'abp-languages',
  // tslint:disable-next-line: component-max-inline-declarations
  template: `
    <div
      *ngIf="(dropdownLanguages$ | async)?.length > 0"
      class="dropdown"
      ngbDropdown
      #languageDropdown="ngbDropdown"
      display="static"
    >
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
        class="dropdown-menu dropdown-menu-right border-0 shadow-sm"
        aria-labelledby="dropdownMenuLink"
        [class.d-block]="smallScreen && languageDropdown.isOpen()"
      >
        <a
          *ngFor="let lang of dropdownLanguages$ | async"
          href="javascript:void(0)"
          class="dropdown-item"
          (click)="onChangeLang(lang.cultureName)"
          >{{ lang?.displayName }}</a
        >
      </div>
    </div>
  `,
})
export class LanguagesComponent implements OnInit {
  get smallScreen(): boolean {
    return window.innerWidth < 992;
  }

  @Select(ConfigState.getDeep('localization.languages'))
  languages$: Observable<ApplicationConfiguration.Language[]>;

  get defaultLanguage$(): Observable<string> {
    return this.languages$.pipe(
      map(
        languages =>
          snq(
            () => languages.find(lang => lang.cultureName === this.selectedLangCulture).displayName,
          ),
        '',
      ),
    );
  }

  get dropdownLanguages$(): Observable<ApplicationConfiguration.Language[]> {
    return this.languages$.pipe(
      map(
        languages =>
          snq(() => languages.filter(lang => lang.cultureName !== this.selectedLangCulture)),
        [],
      ),
    );
  }

  get selectedLangCulture(): string {
    return this.store.selectSnapshot(SessionState.getLanguage);
  }

  constructor(private store: Store) {}

  ngOnInit() {}

  onChangeLang(cultureName: string) {
    this.store.dispatch(new SetLanguage(cultureName));
  }
}
