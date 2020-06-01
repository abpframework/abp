import {
  getLocaleDirection,
  LazyLoadService,
  LOADING_STRATEGY,
  LocalizationService,
} from '@abp/ng.core';
import { Injectable, Injector } from '@angular/core';
import { map, startWith } from 'rxjs/operators';
import { BOOTSTRAP } from '../constants/styles';
import { LocaleDirection } from '../models/common';
import { LAZY_STYLES } from '../tokens/lazy-styles.token';

@Injectable({
  providedIn: 'root',
})
export class LazyStyleHandler {
  private lazyLoad: LazyLoadService;
  private styles: string[];
  private _dir: LocaleDirection = 'ltr';

  set dir(dir: LocaleDirection) {
    if (dir === this._dir) return;

    this.switchCSS(dir);
    this.setBodyDir(dir);
    this._dir = dir;
  }

  get dir(): LocaleDirection {
    return this._dir;
  }

  constructor(injector: Injector) {
    this.setStyles(injector);
    this.setLazyLoad(injector);
    this.listenToLanguageChanges(injector);
  }

  private getLoadedBootstrap(): LoadedStyle {
    const href = createLazyStyleHref(BOOTSTRAP, this.dir);
    const selector = `[href$="${href}"]`;
    const link = document.querySelector<HTMLLinkElement>(selector);
    return { href, link };
  }

  private listenToLanguageChanges(injector: Injector) {
    const l10n = injector.get(LocalizationService);

    // will always listen, no need to unsubscribe
    l10n.languageChange
      .pipe(
        map(({ payload }) => payload),
        startWith(l10n.currentLang),
      )
      .subscribe(locale => {
        this.dir = getLocaleDirection(locale);
      });
  }

  private setBodyDir(dir: LocaleDirection) {
    document.body.dir = dir;
  }

  private setLazyLoad(injector: Injector) {
    this.lazyLoad = injector.get(LazyLoadService);
    const { href, link } = this.getLoadedBootstrap();
    this.lazyLoad.loaded.set(href, link);
  }

  private setStyles(injector: Injector) {
    this.styles = injector.get(LAZY_STYLES, [BOOTSTRAP]);
  }

  private switchCSS(dir: LocaleDirection) {
    this.styles.forEach(style => {
      const oldHref = createLazyStyleHref(style, this.dir);
      const newHref = createLazyStyleHref(style, dir);

      const strategy = LOADING_STRATEGY.PrependAnonymousStyleToHead(newHref);
      this.lazyLoad.load(strategy).subscribe(() => this.lazyLoad.remove(oldHref));
    });
  }
}

export function createLazyStyleHref(style: string, dir: string): string {
  return style.replace(/{{\s*dir\s*}}/g, dir);
}

export function initLazyStyleHandler(injector: Injector) {
  return () => new LazyStyleHandler(injector);
}

interface LoadedStyle {
  href: string;
  link: HTMLLinkElement;
}
