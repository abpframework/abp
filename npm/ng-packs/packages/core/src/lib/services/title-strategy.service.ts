import { Injectable, effect, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { TitleStrategy, RouterStateSnapshot } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { ConfigStateService } from './config-state.service';
import { LocalizationService } from './localization.service';

@Injectable({
  providedIn: 'root',
})
export class AbpTitleStrategy extends TitleStrategy {
  protected readonly title = inject(Title);
  protected readonly configState = inject(ConfigStateService);
  protected readonly localizationService = inject(LocalizationService);
  protected routerState: RouterStateSnapshot;

  projectName = toSignal(this.configState.getDeep$('localization.defaultResourceName'), {
    initialValue: 'MyProjectName',
  });
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
    this.routerState = routerState;
    let title = this.buildTitle(routerState);

    if (!title) {
      this.title.setTitle(this.projectName());
      return;
    }

    const localizedTitle = this.localizationService.instant({ key: title, defaultValue: title });
    this.title.setTitle(`${localizedTitle} | ${this.projectName()}`);
  }
}
