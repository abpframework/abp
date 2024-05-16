import { Injectable, effect, inject } from '@angular/core';
import { TitleStrategy, RouterStateSnapshot } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { toSignal } from '@angular/core/rxjs-interop';
import { LocalizationService } from './localization.service';
import { DISABLE_PROJECT_NAME } from '../tokens';

@Injectable({
  providedIn: 'root',
})
export class AbpTitleStrategy extends TitleStrategy {
  protected readonly title = inject(Title);
  protected readonly localizationService = inject(LocalizationService);
  protected readonly disableProjectName = inject(DISABLE_PROJECT_NAME, { optional: true }) || false;
  protected routerState: RouterStateSnapshot;

  langugageChange = toSignal(this.localizationService.languageChange$);

  constructor() {
    super();
    effect(() => {
      if (this.langugageChange()) {
        this.updateTitle(this.routerState);
      }
    });
  }

  override updateTitle(routerState: RouterStateSnapshot) {
    // routerState assignment is necessary for the language change
    this.routerState = routerState;
    const title = this.buildTitle(routerState);

    let localizedText = '';

    const projectName = this.localizationService.instant({
      key: '::AppName',
      defaultValue: 'MyProjectName',
    });

    if (!title) {
      return this.title.setTitle(projectName);
    }
    localizedText = this.localizationService.instant({ key: title, defaultValue: title });

    if (!this.disableProjectName) {
      localizedText += ` | ${projectName}`;
    }

    this.title.setTitle(localizedText);
  }
}
