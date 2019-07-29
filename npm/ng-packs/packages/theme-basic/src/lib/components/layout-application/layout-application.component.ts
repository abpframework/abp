import {
  ABP,
  ApplicationConfiguration,
  ConfigGetAppConfiguration,
  ConfigState,
  eLayoutType,
  SessionSetLanguage,
  SessionState,
  takeUntilDestroy,
} from '@abp/ng.core';
import { Component, TrackByFunction, ViewChild, TemplateRef, AfterViewInit, OnDestroy } from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Select, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { map, distinctUntilChanged, delay, filter } from 'rxjs/operators';
import snq from 'snq';
import { LayoutAddNavigationElement } from '../../actions';
import { LayoutState } from '../../states';
import { Layout } from '../../models/layout';
import compare from 'just-compare';

@Component({
  selector: 'abp-layout-application',
  templateUrl: './layout-application.component.html',
})
export class LayoutApplicationComponent implements AfterViewInit, OnDestroy {
  // required for dynamic component
  static type = eLayoutType.application;

  @Select(ConfigState.getOne('routes'))
  routes$: Observable<ABP.FullRoute[]>;

  @Select(ConfigState.getOne('currentUser'))
  currentUser$: Observable<ApplicationConfiguration.CurrentUser>;

  @Select(ConfigState.getDeep('localization.languages'))
  languages$: Observable<ApplicationConfiguration.Language[]>;

  @Select(LayoutState.getNavigationElements)
  navElements$: Observable<Layout.NavigationElement[]>;

  @ViewChild('currentUser', { static: false, read: TemplateRef })
  currentUserRef: TemplateRef<any>;

  @ViewChild('language', { static: false, read: TemplateRef })
  languageRef: TemplateRef<any>;

  isOpenChangePassword: boolean = false;

  isOpenProfile: boolean = false;

  get visibleRoutes$(): Observable<ABP.FullRoute[]> {
    return this.routes$.pipe(map(routes => getVisibleRoutes(routes)));
  }

  get defaultLanguage$(): Observable<string> {
    return this.languages$.pipe(
      map(
        languages => snq(() => languages.find(lang => lang.cultureName === this.selectedLangCulture).displayName),
        '',
      ),
    );
  }

  get dropdownLanguages$(): Observable<ApplicationConfiguration.Language[]> {
    return this.languages$.pipe(
      map(languages => snq(() => languages.filter(lang => lang.cultureName !== this.selectedLangCulture)), []),
    );
  }

  get selectedLangCulture(): string {
    return this.store.selectSnapshot(SessionState.getLanguage);
  }

  rightPartElements: TemplateRef<any>[] = [];

  trackByFn: TrackByFunction<ABP.FullRoute> = (_, item) => item.name;

  trackElementByFn: TrackByFunction<ABP.FullRoute> = (_, element) => element;

  constructor(private store: Store, private oauthService: OAuthService) {}

  ngAfterViewInit() {
    const navigations = this.store.selectSnapshot(LayoutState.getNavigationElements).map(({ name }) => name);

    if (navigations.indexOf('LanguageRef') < 0) {
      this.store.dispatch(
        new LayoutAddNavigationElement([
          { element: this.languageRef, order: 4, name: 'LanguageRef' },
          { element: this.currentUserRef, order: 5, name: 'CurrentUserRef' },
        ]),
      );
    }

    this.navElements$
      .pipe(
        map(elements => elements.map(({ element }) => element)),
        filter(elements => !compare(elements, this.rightPartElements)),
        takeUntilDestroy(this),
      )
      .subscribe(elements => {
        setTimeout(() => (this.rightPartElements = elements), 0);
      });
  }

  ngOnDestroy() {}

  onChangeLang(cultureName: string) {
    this.store.dispatch(new SessionSetLanguage(cultureName));
    this.store.dispatch(new ConfigGetAppConfiguration());
  }

  logout() {
    this.oauthService.logOut();
    this.store.dispatch(
      new Navigate(['/account/login'], null, {
        state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
      }),
    );
    this.store.dispatch(new ConfigGetAppConfiguration());
  }
}

function getVisibleRoutes(routes: ABP.FullRoute[]) {
  return routes.reduce((acc, val) => {
    if (val.invisible) return acc;

    if (val.children && val.children.length) {
      val.children = getVisibleRoutes(val.children);
    }

    return [...acc, val];
  }, []);
}
