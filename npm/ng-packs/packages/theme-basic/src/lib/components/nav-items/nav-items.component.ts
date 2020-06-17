import {
  ABP,
  ApplicationConfiguration,
  AuthService,
  ConfigState,
  SessionState,
  SetLanguage,
  takeUntilDestroy,
} from '@abp/ng.core';
import {
  AfterViewInit,
  Component,
  Input,
  OnDestroy,
  TemplateRef,
  TrackByFunction,
  ViewChild,
} from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Select, Store } from '@ngxs/store';
import compare from 'just-compare';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import snq from 'snq';
import { AddNavigationElement } from '../../actions/layout.actions';
import { eNavigationElementNames } from '../../enums/navigation-element-names';
import { Layout } from '../../models/layout';
import { LayoutState } from '../../states/layout.state';

@Component({
  selector: 'abp-nav-items',
  templateUrl: 'nav-items.component.html',
})
export class NavItemsComponent implements AfterViewInit, OnDestroy {
  @Select(LayoutState.getNavigationElements)
  navElements$: Observable<Layout.NavigationElement[]>;

  @Select(ConfigState.getOne('currentUser'))
  currentUser$: Observable<ApplicationConfiguration.CurrentUser>;

  @Select(ConfigState.getDeep('localization.languages'))
  languages$: Observable<ApplicationConfiguration.Language[]>;

  @ViewChild('currentUser', { static: false, read: TemplateRef })
  currentUserRef: TemplateRef<any>;

  @ViewChild('language', { static: false, read: TemplateRef })
  languageRef: TemplateRef<any>;

  @Input()
  smallScreen: boolean;

  rightPartElements: TemplateRef<any>[] = [];

  trackByFn: TrackByFunction<ABP.Route> = (_, element) => element.name;

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

  constructor(private store: Store, private authService: AuthService) {}

  ngAfterViewInit() {
    const navigations = this.store
      .selectSnapshot(LayoutState.getNavigationElements)
      .map(({ name }) => name);

    if (navigations.indexOf(eNavigationElementNames.Language) < 0) {
      this.store.dispatch(
        new AddNavigationElement([
          { element: this.languageRef, order: 4, name: eNavigationElementNames.Language },
          { element: this.currentUserRef, order: 5, name: eNavigationElementNames.User },
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
    this.store.dispatch(new SetLanguage(cultureName));
  }

  logout() {
    this.authService.logout().subscribe(() => {
      this.store.dispatch(
        new Navigate(['/'], null, {
          state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
        }),
      );
    });
  }
}
