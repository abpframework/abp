import {
  getLocaleDirection,
  LazyLoadService,
  LOADING_STRATEGY,
  LocalizationService,
} from '@abp/ng.core';
import { Injectable, Injector } from '@angular/core';
import { startWith } from 'rxjs/operators';
import { BOOTSTRAP } from '../constants/styles';
import { LocaleDirection } from '../models/common';
import { LAZY_STYLES } from '../tokens/lazy-styles.token';

@Injectable({
  providedIn: 'root',
})
export class LazyStyleHandler {
  private lazyLoad!: LazyLoadService;
  private styles!: string[];
  private _dir: LocaleDirection = 'ltr';

  readonly loaded = new Map<string, HTMLLinkElement>();

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

  private getHrefFromLink(link: HTMLLinkElement | null | undefined): string {
    if (!link) return '';

    const a = document.createElement('a');
    a.href = link.href;
    return a.pathname.replace(/^\//, '');
  }

  private getLoadedBootstrap(): LoadedStyle {
    const href = createLazyStyleHref(BOOTSTRAP, this.dir);
    const selector = `[href*="${href.replace(/\.css$/, '')}"]`;
    const link = document.querySelector<HTMLLinkElement>(selector);
    return { href, link };
  }

  private listenToLanguageChanges(injector: Injector) {
    const l10n = injector.get(LocalizationService);

    // will always listen, no need to unsubscribe
    l10n.languageChange$.pipe(startWith(l10n.currentLang)).subscribe(locale => {
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
      const link = this.loaded.get(newHref);
      const href = this.getHrefFromLink(link) || newHref;

      const strategy = LOADING_STRATEGY.PrependAnonymousStyleToHead(href);
      this.lazyLoad.load(strategy).subscribe(() => {
        const oldLink = this.lazyLoad.loaded.get(oldHref) as HTMLLinkElement;
        this.loaded.delete(newHref);
        this.loaded.set(oldHref, oldLink);
        const newLink = this.lazyLoad.loaded.get(href) as HTMLLinkElement;
        this.lazyLoad.loaded.delete(href);
        this.lazyLoad.loaded.set(newHref, newLink);
        this.lazyLoad.remove(oldHref);
      });
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
  link: HTMLLinkElement | null;
}
