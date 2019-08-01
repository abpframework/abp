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
import {
  AfterViewInit,
  Component,
  OnDestroy,
  QueryList,
  TemplateRef,
  TrackByFunction,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Select, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import compare from 'just-compare';
import { fromEvent, Observable } from 'rxjs';
import { debounceTime, filter, map } from 'rxjs/operators';
import snq from 'snq';
import { LayoutAddNavigationElement } from '../../actions';
import { Layout } from '../../models/layout';
import { LayoutState } from '../../states';

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

  @ViewChildren('navbarRootDropdown', { read: NgbDropdown })
  navbarRootDropdowns: QueryList<NgbDropdown>;

  isOpenChangePassword: boolean = false;

  isOpenProfile: boolean = false;

  isDropdownChildDynamic: boolean;

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

  private checkWindowWidth() {
    setTimeout(() => {
      if (window.innerWidth < 768) {
        this.isDropdownChildDynamic = false;
        this.navbarRootDropdowns.forEach(item => {
          item.close();
        });
      } else {
        this.isDropdownChildDynamic = true;
      }
    }, 0);
  }

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

    this.checkWindowWidth();

    fromEvent(window, 'resize')
      .pipe(
        takeUntilDestroy(this),
        debounceTime(250),
      )
      .subscribe(() => {
        this.checkWindowWidth();
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
